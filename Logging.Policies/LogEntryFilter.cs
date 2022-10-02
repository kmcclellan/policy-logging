namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A filter for log entries.
/// </summary>
public class LogEntryFilter
{
    /// <summary>
    /// Gets or sets the log category pattern.
    /// </summary>
    /// <remarks>
    /// Supports prefix or wildcard (<c>*</c>) matching.
    /// </remarks>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets a specific value for <see cref="EventId.Id"/>, or zero for all.
    /// </summary>
    public int EventId { get; set; }

    /// <summary>
    /// Gets or sets a specific value for <see cref="EventId.Name"/>, or <see langword="null"/> for all.
    /// </summary>
    public string? EventName { get; set; }
}
