namespace Microsoft.Extensions.Logging.Policies.Core;

/// <summary>
/// A policy for capturing log messages using entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public interface ILogMessagePolicy<TEntry>
{
    /// <summary>
    /// Captures a log message.
    /// </summary>
    /// <param name="entry">The target log entry.</param>
    /// <param name="message">The log message.</param>
    void OnEntry(ref TEntry entry, string message);
}
