namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A builder of log entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public interface ILogEntryBuilder<out TEntry>
{
    /// <summary>
    /// Configures intializing the entry with log category, level, and event ID.
    /// </summary>
    /// <param name="action">The initialize action.</param>
    /// <returns>The same builder instance, for chaining.</returns>
    ILogEntryBuilder<TEntry> Initialize(Action<TEntry, string, LogLevel, EventId> action);

    /// <summary>
    /// Configures adding log message to the entry.
    /// </summary>
    /// <param name="action">The add action.</param>
    /// <returns>The same builder instance, for chaining.</returns>
    ILogEntryBuilder<TEntry> AddMessage(Action<TEntry, string> action);

    /// <summary>
    /// Configures adding a logged exception to the entry.
    /// </summary>
    /// <param name="action">The add action.</param>
    /// <returns>The same builder instance, for chaining.</returns>
    ILogEntryBuilder<TEntry> AddException(Action<TEntry, Exception> action);

    /// <summary>
    /// Configures adding log/scope state of a given type to the entry.
    /// </summary>
    /// <typeparam name="T">The type of state.</typeparam>
    /// <param name="action">The add action.</param>
    /// <returns>The same builder instance, for chaining.</returns>
    ILogEntryBuilder<TEntry> AddState<T>(Action<TEntry, T> action);
}
