namespace Microsoft.Extensions.Logging.Policies;

class PolicyLoggerOptions<TEntry, TState>
{
    public ILoggingPolicy<TEntry>? Policy { get; set; }

    public List<LogEntryActions<TEntry, TState>> Actions { get; } = new();
}
