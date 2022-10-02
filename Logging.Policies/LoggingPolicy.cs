namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A policy for writing log entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public class LoggingPolicy<TEntry>
{
    /// <summary>
    /// Gets or sets the log field policy, if any.
    /// </summary>
    public ILogFieldPolicy<TEntry>? Fields { get; set; }

    /// <summary>
    /// Gets or sets the log message policy, if any.
    /// </summary>
    public ILogMessagePolicy<TEntry>? Messages { get; set; }

    /// <summary>
    /// Gets or sets the log exception policy, if any.
    /// </summary>
    public ILogExceptionPolicy<TEntry>? Exceptions { get; set; }

    /// <summary>
    /// Gets or sets the log state policy, if any.
    /// </summary>
    public ILogStatePolicy<TEntry>? State { get; set; }

    /// <summary>
    /// Gets or sets the log scope policy, if any.
    /// </summary>
    public ILogScopePolicy<TEntry>? Scopes { get; set; }

    /// <summary>
    /// Gets or sets the log entry filter, if any.
    /// </summary>
    public LogEntryFilter? Filter { get; set; }
}
