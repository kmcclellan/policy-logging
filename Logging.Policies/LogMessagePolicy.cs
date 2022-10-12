namespace Microsoft.Extensions.Logging.Policies;

class LogMessagePolicy<TEntry> : ILogMessagePolicy<TEntry>
{
    readonly Action<TEntry, string> action;

    public LogMessagePolicy(Action<TEntry, string> action)
    {
        this.action = action;
    }

    public void OnEntry(TEntry entry, string category)
    {
        this.action(entry, category);
    }
}
