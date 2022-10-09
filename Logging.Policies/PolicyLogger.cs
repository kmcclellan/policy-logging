namespace Microsoft.Extensions.Logging.Policies;

class PolicyLogger<TEntry> : ILogger
{
    readonly ScopeStack<TEntry> scopes;
    readonly string category;

    FilteredOptions? options;

    public PolicyLogger(ScopeStack<TEntry> scopes, string category)
    {
        this.scopes = scopes;
        this.category = category;
    }

    public void SetOptions(PolicyLoggingOptions<TEntry> options)
    {
        var policies = new List<LoggingPolicy<TEntry>>();

        foreach (var policy in options.Policies)
        {
            if (policy.Filter == null || this.Matches(policy.Filter.Category))
            {
                policies.Add(policy);
            }
        }

        this.options = options.Begin != null && policies.Count > 0
            ? new(options.Begin, options.Finish, policies.ToArray())
            : null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return this.options != null;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        if (this.options != null)
        {
            var poppers = default(List<IDisposable>);

            foreach (var policy in this.options.Policies)
            {
                if (policy.Scopes != null)
                {
                    var scope = this.scopes.Peek(policy.Scopes) ?? policy.Scopes.Create();
                    scope = scope.Include(state);

                    if (scope != null)
                    {
                        poppers ??= new();
                        poppers.Add(this.scopes.Push(policy.Scopes, scope));
                    }
                }
            }

            if (poppers != null)
            {
                return new MergedScope(poppers);
            }
        }

        return MergedScope.Null;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (this.options != null)
        {
            var entry = default(TEntry?);
            var message = default(string);

            foreach (var policy in this.options.Policies)
            {
                if (policy.Filter == null ||
                    policy.Filter.EventId == eventId.Id ||
                    policy.Filter.EventName == eventId.Name)
                {
                    entry ??= this.options.Begin();

                    if (policy.Scopes != null)
                    {
                        this.scopes.Peek(policy.Scopes)?.OnEntry(entry);
                    }

                    policy.Fields?.OnEntry(entry, this.category, logLevel, eventId);
                    policy.Messages?.OnEntry(entry, message ??= formatter(state, exception));

                    if (exception != null)
                    {
                        policy.Exceptions?.OnEntry(entry, exception);
                    }

                    policy.State?.OnEntry(entry, state);
                }
            }

            if (entry != null)
            {
                this.options.Finish?.Invoke(entry);
            }
        }
    }

    bool Matches(ReadOnlySpan<char> pattern)
    {
        const char wildcard = '*';
        const StringComparison cmp = StringComparison.OrdinalIgnoreCase;

        var split = pattern.IndexOf(wildcard);
        if (split < 0)
        {
            return this.category.AsSpan().StartsWith(pattern, cmp);
        }

        var suffix = pattern[(split + 1)..];
        if (suffix.IndexOf(wildcard) > 0)
        {
            throw new ArgumentException(
                "Only one wildcard character is allowed in category name.",
                nameof(pattern));
        }

        return category.AsSpan().StartsWith(pattern[..split], cmp) && category.AsSpan().EndsWith(suffix, cmp);
    }

    class FilteredOptions
    {
        public FilteredOptions(Func<TEntry> begin, Action<TEntry>? finish, LoggingPolicy<TEntry>[] policies)
        {
            this.Begin = begin;
            this.Finish = finish;
            this.Policies = policies;
        }

        public Func<TEntry> Begin { get; }

        public Action<TEntry>? Finish { get; }

        public LoggingPolicy<TEntry>[] Policies { get; }
    }

    class MergedScope : IDisposable
    {
        public static MergedScope Null = new(null);

        readonly List<IDisposable>? scopes;

        public MergedScope(List<IDisposable>? scopes)
        {
            this.scopes = scopes;
        }

        public void Dispose()
        {
            if (this.scopes != null)
            {
                foreach (var scope in this.scopes)
                {
                    scope.Dispose();
                }
            }
        }
    }
}
