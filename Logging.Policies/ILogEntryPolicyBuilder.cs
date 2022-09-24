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
    /// Configures log entry formatting using a log property.
    /// </summary>
    /// <param name="format">The format action.</param>
    /// <returns>The same policy builder instance, for chaining.</returns>
    ILogEntryPolicyBuilder<TEntry> OnProperty(Action<TEntry, string, object?> format);

    /// <summary>
    /// Configures log entry filtering.
    /// </summary>
    /// <param name="category">The log category filter. Supports prefix or wildcard (<c>*</c>) matching.</param>
    /// <param name="level">The minimum log level.</param>
    /// <param name="eventId">A specific value for <see cref="EventId.Id"/>, or <see langword="null"/> for all.</param>
    /// <param name="eventName">A specific value for <see cref="EventId.Name"/>, or <see langword="null"/> for all.</param>
    /// <returns>A builder of policies to apply to matching entries.</returns>
    ILogEntryPolicyBuilder<TEntry> Filter(
        string category = "*",
        LogLevel level = LogLevel.Trace,
        int? eventId = null,
        string? eventName = null);
}

