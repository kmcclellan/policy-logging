namespace Microsoft.Extensions.Logging.Policies;

interface ILogScopePolicy<in TEntry, in TState>
{
    void Push(TState state);

    void Pop();

    void OnEntry(TEntry entry);
}
