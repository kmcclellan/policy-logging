namespace Microsoft.Extensions.Logging.Policies.Core;

/// <summary>
/// A policy for capturing logged exceptions using entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public interface ILogExceptionPolicy<in TEntry>
{
    /// <summary>
    /// Captures a logged exception.
    /// </summary>
    /// <param name="entry">The target log entry.</param>
    /// <param name="exception">The logged exception.</param>
    void OnEntry(TEntry entry, Exception exception);
}
