namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A policy for filtering log entries.
/// </summary>
public class LogFilterPolicy
{
    /// <summary>
    /// Gets or sets the log category filter.
    /// </summary>
    /// <remarks>
    /// Supports prefix or wildcard (<c>*</c>) matching.
    /// </remarks>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the minimum log level.
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// Gets or sets a specific value for <see cref="EventId.Id"/>, or zero for all.
    /// </summary>
    public int EventId { get; set; }

    /// <summary>
    /// Gets or sets a specific value for <see cref="EventId.Name"/>, or <see langword="null"/> for all.
    /// </summary>
    public string? EventName { get; set; }

    /// <summary>
    /// Gets or sets whether to include log scope information.
    /// </summary>
    public bool IncludeScopes { get; set; }
}
