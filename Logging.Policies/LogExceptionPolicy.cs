namespace Microsoft.Extensions.Logging.Policies;

class LogExceptionPolicy<TEntry> : ILogExceptionPolicy<TEntry>
{
    readonly Action<TEntry, Exception> action;

    public LogExceptionPolicy(Action<TEntry, Exception> action)
    {
        this.action = action;
    }

    public void OnEntry(TEntry entry, Exception exception)
    {
        this.action(entry, exception);
    }
}
