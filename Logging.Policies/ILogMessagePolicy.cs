namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A policy to collect log messages.
/// </summary>
/// <typeparam name="TState">The type of log entry state.</typeparam>
public interface ILogMessagePolicy<in TState>
{
    /// <summary>
    /// Collects a log message.
    /// </summary>
    /// <param name="entry">The log entry state.</param>
    /// <param name="message">The log message.</param>
    void AddMessage(TState entry, string message);
}
