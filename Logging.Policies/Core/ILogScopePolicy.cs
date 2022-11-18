namespace Microsoft.Extensions.Logging.Policies.Core;

/// <summary>
/// A policy for capturing log scopes using entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public interface ILogScopePolicy<TEntry>
{
    /// <summary>
    /// Creates a new scope.
    /// </summary>
    /// <returns>The log scope.</returns>
    ILogScope<TEntry> Create();
}
