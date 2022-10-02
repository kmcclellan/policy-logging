namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A policy for capturing log scopes using entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public interface ILogScopePolicy<in TEntry>
{
    /// <summary>
    /// Creates a new scope policy including the typed state.
    /// </summary>
    /// <typeparam name="TState">The scope state type.</typeparam>
    /// <param name="state">The scope state.</param>
    /// <returns>The new scope policy, or <see langword="null"/> if unchanged.</returns>
    ILogScopePolicy<TEntry>? Include<TState>(TState state);

    /// <summary>
    /// Captures the scope.
    /// </summary>
    /// <param name="entry">The target log entry.</param>
    void OnEntry(TEntry entry);
}
