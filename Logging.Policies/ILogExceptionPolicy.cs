namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A policy to collect logged exceptions.
/// </summary>
/// <typeparam name="TState">The type of log entry state.</typeparam>
public interface ILogExceptionPolicy<in TState>
{
    /// <summary>
    /// Collects a logged exception.
    /// </summary>
    /// <param name="entry">The log entry state.</param>
    /// <param name="exception">The logged exception.</param>
    void AddException(TState entry, Exception exception);
}
