namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A policy for writing log entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public interface ILoggingPolicy<TEntry>
{
    /// <summary>
    /// Initializes and begins writing a log entry.
    /// </summary>
    /// <returns>The log entry.</returns>
    TEntry Begin();

    /// <summary>
    /// Finishes writing a log entry.
    /// </summary>
    /// <param name="entry">The log entry.</param>
    void Finish(TEntry entry);
}
