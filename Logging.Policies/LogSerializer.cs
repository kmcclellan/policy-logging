namespace Microsoft.Extensions.Logging.Policies;

using System.Threading.Tasks.Dataflow;

/// <summary>
/// A queued serializer of log entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public abstract class LogSerializer<TEntry> : LogTarget<TEntry>
{
    /// <summary>
    /// Gets or sets the maximum number of bytes to be buffered in memory.
    /// </summary>
    protected abstract int BufferBytes { get; }

    /// <summary>
    /// Gets or sets the maximum log time interval to be buffered in memory.
    /// </summary>
    protected abstract TimeSpan BufferInterval { get; }

    /// <inheritdoc/>
    protected sealed override async Task ProcessAsync(IReceivableSourceBlock<TEntry> source)
    {
        var writtenBytes = 0;

        while (await source.OutputAvailableAsync().ConfigureAwait(false))
        {
            var timestamp = DateTime.UtcNow;

            do
            {
                TimeSpan delay;

                if (source.TryReceive(out var entry))
                {
                    writtenBytes += this.Write(entry);
                }
                else if (writtenBytes < this.BufferBytes &&
                    (delay = DateTime.UtcNow - timestamp + this.BufferInterval) > TimeSpan.Zero)
                {
                    try
                    {
                        await source.OutputAvailableAsync().WaitAsync(delay).ConfigureAwait(false);
                    }
                    catch (TimeoutException)
                    {
                    }
                }
                else
                {
                    await this.FlushAsync(CancellationToken.None).ConfigureAwait(false);
                    writtenBytes = 0;
                }
            }
            while (writtenBytes > 0);
        }
    }

    /// <summary>
    /// Writes a log entry to the memory buffer, returning the number of bytes written.
    /// </summary>
    protected abstract int Write(TEntry entry);

    /// <summary>
    /// Asynchronously flushes the buffered log data.
    /// </summary>
    protected abstract Task FlushAsync(CancellationToken cancellationToken);
}
