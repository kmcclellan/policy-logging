namespace Microsoft.Extensions.Logging.Policies.Serialization;

/// <summary>
/// Options for buffering serialized log entries.
/// </summary>
public class LogBufferOptions
{
    /// <summary>
    /// Gets or sets the number of bytes to buffer before flushing.
    /// </summary>
    public int Size { get; set; } = 4096;

    /// <summary>
    /// Gets or sets the period of time to buffer before flushing.
    /// </summary>
    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(1);
}
