namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A policy to collect information from log entries.
/// </summary>
/// <typeparam name="TState">The type of log entry state.</typeparam>
public interface ILoggerPolicy<TState>
{
    /// <summary>
    /// Gets the policy for log messages, if any.
    /// </summary>
    ILogMessagePolicy<TState>? Messages { get; }

    /// <summary>
    /// Gets the policy for logged exceptions, if any.
    /// </summary>
    ILogExceptionPolicy<TState>? Exceptions { get; }

    /// <summary>
    /// Gets the policy for log properties, if any.
    /// </summary>
    ILogPropertyPolicy<TState>? Properties { get; }

    /// <summary>
    /// Checks whether logging is enabled for the given level.
    /// </summary>
    /// <remarks>
    /// Avoids side-effects of calling <see cref="TryBegin"/>. No need to invoke before collecting.
    /// </remarks>
    /// <param name="level">The log level.</param>
    /// <returns><see langword="true"/> if enabled, otherwise <see langword="false"/>.</returns>
    bool IsEnabled(LogLevel level);

    /// <summary>
    /// Begins collecting from a log entry, if enabled for the given level and event ID.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <param name="id">The log event ID.</param>
    /// <param name="entry">State to be used to continue collecting for the entry.</param>
    /// <returns><see langword="true"/> if enabled, otherwise <see langword="false"/>.</returns>
    bool TryBegin(LogLevel level, EventId id, out TState entry);

    /// <summary>
    /// Finishes collecting from a log entry.
    /// </summary>
    /// <param name="entry">The log entry state.</param>
    void Finish(TState entry);
}
