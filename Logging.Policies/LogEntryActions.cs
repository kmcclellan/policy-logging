namespace Microsoft.Extensions.Logging.Policies;

class LogEntryActions<TEntry, TState>
{
    Action<TEntry, string, LogLevel, EventId>? OnEntry { get; set; }

    Action<TEntry, string>? OnMessage { get; set; }

    Action<TEntry, Exception>? OnException { get; set; }

    Action<TEntry, TState>? OnState { get; set; }

    LogEntryFilter? Filter { get; set; }
}
