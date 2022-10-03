namespace Microsoft.Extensions.Logging.Policies;

class PolicyLogger<TEntry, TState> : ILogger
{
    readonly AsyncLocal<Stack<TState>> scopes;
    readonly string category;

    public PolicyLogger(AsyncLocal<Stack<TState>> scopes, string category, PolicyLoggerOptions<TEntry, TState> options)
    {
        this.scopes = scopes;
        this.category = category;
        this.SetOptions(options);
    }

    public void SetOptions(PolicyLoggerOptions<TEntry, TState> options)
    {
        throw new NotImplementedException();
    }

    public IDisposable BeginScope<T>(T state)
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }

    public void Log<T>(
        LogLevel logLevel,
        EventId eventId,
        T state,
        Exception? exception,
        Func<T, Exception?, string> formatter)
    {
        throw new NotImplementedException();
    }
}
