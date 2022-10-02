namespace Microsoft.Extensions.Logging.Policies;

class PolicyLogger<TEntry, TState> : ILogger
{
    readonly string category;

    public PolicyLogger(string category, PolicyLoggerOptions<TEntry, TState> options)
    {
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
