namespace Microsoft.Extensions.Logging.Policies;

using System.Threading.Tasks.Dataflow;

/// <summary>
/// A base serializer for log entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public abstract class LogSerializer<TEntry> : IAsyncDisposable
{
    readonly BufferBlock<TEntry> buffer = new();
    readonly Task task;

    /// <summary>
    /// Initializes the serializer.
    /// </summary>
    public LogSerializer()
    {
        this.task = this.Run();
    }

    /// <summary>
    /// Gets or sets the number of bytes to be buffered before triggering a flush.
    /// </summary>
    protected abstract int BufferBytes { get; }

    /// <summary>
    /// Gets or sets the time interval to be buffered before triggering a flush.
    /// </summary>
    protected abstract TimeSpan BufferInterval { get; }

    /// <summary>
    /// Serializes a log entry.
    /// </summary>
    /// <param name="entry">The log entry.</param>
    public void Serialize(TEntry entry)
    {
        if (!this.buffer.Post(entry))
        {
            throw new ObjectDisposedException(null);
        }
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return this.DisposeAsync(disposing: true);
    }

    /// <summary>
    /// Writes an entry a buffer, returning the number of bytes written.
    /// </summary>
    protected abstract int Write(TEntry entry);

    /// <summary>
    /// Asynchronously flushes the buffered log data.
    /// </summary>
    protected abstract Task FlushAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Disposes and/or finalizes the instance asynchronously.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> to dispose and finalize, <see langword="false"/> to finalize only.
    /// </param>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    protected async ValueTask DisposeAsync(bool disposing)
    {
        if (disposing)
        {
            this.buffer.Complete();

            await this.buffer.Completion.ConfigureAwait(false);
            await this.task.ConfigureAwait(false);
        }
    }

    async Task Run()
    {
        try
        {
            var writtenBytes = 0;

            while (await buffer.OutputAvailableAsync().ConfigureAwait(false))
            {
                var timestamp = DateTime.UtcNow;

                do
                {
                    TimeSpan delay;

                    if (buffer.TryReceive(out var entry))
                    {
                        writtenBytes += this.Write(entry);
                    }
                    else if (writtenBytes < this.BufferBytes &&
                        (delay = DateTime.UtcNow - timestamp + this.BufferInterval) > TimeSpan.Zero)
                    {
                        try
                        {
                            await buffer.OutputAvailableAsync().WaitAsync(delay).ConfigureAwait(false);
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
        catch (Exception exception)
        {
            ((IDataflowBlock)this.buffer).Fault(exception);
            throw;
        }
    }
}
