namespace Microsoft.Extensions.Logging.Policies;

using Microsoft.Extensions.Logging;

class LogFieldPolicy<TEntry> : ILogFieldPolicy<TEntry>
{
    readonly Action<TEntry, string, LogLevel, EventId> action;

    public LogFieldPolicy(Action<TEntry, string, LogLevel, EventId> action)
    {
        this.action = action;
    }

    public void OnEntry(TEntry entry, string category, LogLevel level, EventId id)
    {
        this.action(entry, category, level, id);
    }
}
