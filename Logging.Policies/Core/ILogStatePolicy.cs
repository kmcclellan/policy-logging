namespace Microsoft.Extensions.Logging.Policies.Core;

/// <summary>
/// A policy for capturing log state using entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public interface ILogStatePolicy<TEntry>
{
    /// <summary>
    /// Captures log state.
    /// </summary>
    /// <typeparam name="TState">The log state type.</typeparam>
    /// <param name="entry">The target log entry.</param>
    /// <param name="state">The log state.</param>
    void OnEntry<TState>(ref TEntry entry, TState state);
}
