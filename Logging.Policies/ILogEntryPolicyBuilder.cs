namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A builder of policies for formatting log entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public interface ILogEntryPolicyBuilder<out TEntry>
{
    /// <summary>
    /// Configures log entry formatting using log category, level, and event ID.
    /// </summary>
    /// <param name="format">The format action.</param>
    /// <returns>The same policy builder instance, for chaining.</returns>
    ILogEntryPolicyBuilder<TEntry> OnEntry(Action<TEntry, string, LogLevel, EventId> format);

    /// <summary>
    /// Configures log entry formatting using the log message.
    /// </summary>
    /// <param name="format">The format action.</param>
    /// <returns>The same policy builder instance, for chaining.</returns>
    ILogEntryPolicyBuilder<TEntry> OnMessage(Action<TEntry, string> format);

    /// <summary>
    /// Configures log entry formatting using a logged exception.
    /// </summary>
    /// <param name="format">The format action.</param>
    /// <returns>The same policy builder instance, for chaining.</returns>
    ILogEntryPolicyBuilder<TEntry> OnException(Action<TEntry, Exception> format);

    /// <summary>
    /// Configures log entry formatting using log/scope state of a given type.
    /// </summary>
    /// <typeparam name="T">The type of state.</typeparam>
    /// <param name="format">The format action.</param>
    /// <returns>The same policy builder instance, for chaining.</returns>
    ILogEntryPolicyBuilder<TEntry> OnState<T>(Action<TEntry, T> format);

    /// <summary>
    /// Configures log entry filtering.
    /// </summary>
    /// <param name="policy">The log filter policy.</param>
    /// <returns>A builder of policies to apply to matching entries.</returns>
    ILogEntryPolicyBuilder<TEntry> Filter(LogFilterPolicy policy);
}
