namespace Microsoft.Extensions.Logging.Policies.Core;

/// <summary>
/// A policy for capturing standard log fields using entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public interface ILogFieldPolicy<in TEntry>
{
    /// <summary>
    /// Captures standard log fields.
    /// </summary>
    /// <param name="entry">The target log entry.</param>
    /// <param name="category">The log category name.</param>
    /// <param name="level">The log level.</param>
    /// <param name="id">The log event ID.</param>
    void OnEntry(TEntry entry, string category, LogLevel level, EventId id);
}
