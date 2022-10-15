namespace Microsoft.Extensions.Logging.Policies;

using System.Collections.Concurrent;

/// <summary>
/// A base provider of loggers using logging policies.
/// </summary>
/// <typeparam name="TEntry">The policy log entry type.</typeparam>
public abstract class PolicyLoggerProvider<TEntry> : ILoggerProvider
{
    readonly ConcurrentDictionary<string, PolicyLogger> loggers = new();
    readonly ScopeStack<TEntry> scopes = new();

    IEnumerable<LoggingPolicy<TEntry>>? policies;

    /// <inheritdoc/>
    public virtual ILogger CreateLogger(string categoryName)
    {
        return this.loggers.GetOrAdd(
            categoryName,
            (cat, prov) => new PolicyLogger(prov, cat),
            this);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Begins writing a log entry.
    /// </summary>
    /// <returns>The log entry.</returns>
    protected abstract TEntry Begin();

    /// <summary>
    /// Finishes writing a log entry.
    /// </summary>
    /// <param name="entry">The log entry.</param>
    protected virtual void Finish(TEntry entry)
    {
    }

    /// <summary>
    /// Sets the logging policies used by loggers.
    /// </summary>
    /// <param name="policies">The logging policies.</param>
    protected void SetPolicies(IEnumerable<LoggingPolicy<TEntry>> policies)
    {
        this.policies = policies ?? throw new ArgumentNullException(nameof(policies));

        foreach (var logger in this.loggers.Values)
        {
            logger.SetPolicies(policies);
        }
    }

    /// <summary>
    /// Disposes and/or finalizes the instance.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> to dispose and finalize, <see langword="false"/> to finalize only.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
    }

    class PolicyLogger : ILogger
    {
        readonly PolicyLoggerProvider<TEntry> provider;
        readonly string category;

        LoggingPolicy<TEntry>[]? policies;

        public PolicyLogger(PolicyLoggerProvider<TEntry> provider, string category)
        {
            this.provider = provider;
            this.category = category;
        }

        public void SetPolicies(IEnumerable<LoggingPolicy<TEntry>> policies)
        {
            var filtered = new List<LoggingPolicy<TEntry>>();

            foreach (var policy in policies)
            {
                if (policy.Filter == null || this.Matches(policy.Filter.Category))
                {
                    filtered.Add(policy);
                }
            }

            this.policies = filtered.Count > 0 ? filtered.ToArray() : null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return this.policies != null;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            if (this.policies != null)
            {
                var poppers = default(List<IDisposable>);

                foreach (var policy in this.policies)
                {
                    if (policy.Scopes != null)
                    {
                        var scope = this.provider.scopes.Peek(policy.Scopes) ?? policy.Scopes.Create();
                        scope = scope.Include(state);

                        if (scope != null)
                        {
                            poppers ??= new();
                            poppers.Add(this.provider.scopes.Push(policy.Scopes, scope));
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
            if (this.policies != null)
            {
                var entry = default(TEntry?);
                var message = default(string);

                foreach (var policy in this.policies)
                {
                    if (policy.Filter == null ||
                        policy.Filter.EventId == eventId.Id ||
                        policy.Filter.EventName == eventId.Name)
                    {
                        entry ??= this.provider.Begin();

                        if (policy.Scopes != null)
                        {
                            this.provider.scopes.Peek(policy.Scopes)?.OnEntry(entry);
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
                    this.provider.Finish(entry);
                }
            }
        }

        bool Matches(ReadOnlySpan<char> pattern)
        {
            const char wildcard = '*';
            const StringComparison cmp = StringComparison.OrdinalIgnoreCase;

            var split = pattern.IndexOf(wildcard);

            if (split == -1)
            {
                return this.category.AsSpan().StartsWith(pattern, cmp);
            }

            var suffix = pattern[(split + 1)..];

            return suffix.IndexOf(wildcard) == -1
                ? category.AsSpan().StartsWith(pattern[..split], cmp) && category.AsSpan().EndsWith(suffix, cmp)
                : throw new ArgumentException(
                    "Only one wildcard character is allowed in category name.",
                    nameof(pattern));
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
}
