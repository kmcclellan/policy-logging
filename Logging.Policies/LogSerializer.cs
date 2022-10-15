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
        this.task = this.Flush();
    }

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
    public async ValueTask DisposeAsync()
    {
        this.buffer.Complete();

        await this.buffer.Completion.ConfigureAwait(false);
        await this.task.ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the serializer options.
    /// </summary>
    protected abstract LogSerializerOptions<TEntry> Options { get; }

    async Task Flush()
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
                        writtenBytes += this.Options.Write?.Invoke(entry) ?? 0;
                    }
                    else if (writtenBytes < this.Options.BufferBytes &&
                        (delay = DateTime.UtcNow - timestamp + this.Options.BufferInterval) > TimeSpan.Zero)
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
                        if (this.Options.Flush is { } flush)
                        {
                            await flush(CancellationToken.None).ConfigureAwait(false);
                        }

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
