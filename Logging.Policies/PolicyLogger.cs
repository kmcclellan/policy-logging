namespace Microsoft.Extensions.Logging.Policies;

class PolicyLogger<TEntry> : ILogger
{
    public PolicyLogger(ScopeStack<TEntry> scopes, string category)
    {
        throw new NotImplementedException();
    }

    public void SetOptions(PolicyLoggingOptions<TEntry> options)
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        throw new NotImplementedException();
    }
}
