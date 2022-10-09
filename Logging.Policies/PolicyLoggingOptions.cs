namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// Options for logging using logging policies.
/// </summary>
/// <typeparam name="TEntry">The policy log entry type.</typeparam>
public class PolicyLoggingOptions<TEntry>
{
    /// <summary>
    /// Gets or sets the delegate to begin writing a log entry.
    /// </summary>
    public Func<TEntry>? Begin { get; set; }

    /// <summary>
    /// Gets or sets the delegate to finish writing a log entry.
    /// </summary>
    public Action<TEntry>? Finish { get; set; }

    /// <summary>
    /// Gets the logging policy collection.
    /// </summary>
    public ICollection<LoggingPolicy<TEntry>> Policies { get; } = new HashSet<LoggingPolicy<TEntry>>();
}
