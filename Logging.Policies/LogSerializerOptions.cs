namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// Options for serializing log entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public class LogSerializerOptions<TEntry>
{
    /// <summary>
    /// Gets or sets the number of bytes to be buffered before triggering a flush.
    /// </summary>
    public int BufferBytes { get; set; }

    /// <summary>
    /// Gets or sets the time interval to be buffered before triggering a flush.
    /// </summary>
    public TimeSpan BufferInterval { get; set; }

    /// <summary>
    /// Gets or sets the write action, returning the number of bytes written.
    /// </summary>
    public Func<TEntry, int>? Write { get; set; }

    /// <summary>
    /// Gets or sets the asynchronous flush action.
    /// </summary>
    public Func<CancellationToken, Task>? Flush { get; set; }
}
