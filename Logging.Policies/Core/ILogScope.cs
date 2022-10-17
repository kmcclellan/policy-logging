namespace Microsoft.Extensions.Logging.Policies.Core;

/// <summary>
/// A mutable scope to capture ambient log information using entries of a given type.
/// </summary>
public interface ILogScope<in TEntry>
{
    /// <summary>
    /// Creates a new scope from the current scope, including the typed state.
    /// </summary>
    /// <typeparam name="TState">The scope state type.</typeparam>
    /// <param name="state">The scope state.</param>
    /// <returns>The log scope, or <see langword="null"/> if unchanged.</returns>
    ILogScope<TEntry>? Include<TState>(TState state);

    /// <summary>
    /// Captures the scope.
    /// </summary>
    /// <param name="entry">The target log entry.</param>
    void OnEntry(TEntry entry);
}
