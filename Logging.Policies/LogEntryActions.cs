namespace Microsoft.Extensions.Logging.Policies;

class LogEntryActions<TEntry, TState>
{
    public Action<TEntry, string, LogLevel, EventId>? OnEntry { get; set; }

    public Action<TEntry, string>? OnMessage { get; set; }

    public Action<TEntry, Exception>? OnException { get; set; }

    public Action<TEntry, TState>? OnState { get; set; }

    public ILogScopePolicy<TEntry, TState>? OnScope { get; set; }

    public LogEntryFilter? Filter { get; set; }
}
