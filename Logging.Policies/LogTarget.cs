namespace Microsoft.Extensions.Logging.Policies;

using System.Threading.Tasks.Dataflow;

/// <summary>
/// A queued processor of log entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public abstract class LogTarget<TEntry> : IAsyncDisposable
{
    readonly BufferBlock<TEntry> buffer = new();
    readonly Task task;

    /// <summary>
    /// Initializes the target.
    /// </summary>
    public LogTarget()
    {
        // Propagate faults upstream.
        this.task = this.ProcessAsync(this.buffer)
            .ContinueWith(
                (t, obj) => ((IDataflowBlock)obj!).Fault(t.Exception!.Flatten()),
                this.buffer,
                TaskContinuationOptions.OnlyOnFaulted);
    }

    /// <summary>
    /// Queues a log entry to be processed by the target.
    /// </summary>
    /// <param name="entry">The log entry.</param>
    public void Send(in TEntry entry)
    {
        if (!this.buffer.Post(entry))
        {
            if (this.buffer.Completion.IsFaulted)
            {
                throw this.buffer.Completion.Exception!.Flatten();
            }

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
    /// Processes the log queue asynchronously.
    /// </summary>
    /// <remarks>
    /// This method is invoked once for the lifetime of the instance.
    /// <para>
    /// Implementers should observe the source's <see cref="IDataflowBlock.Completion"/> to conclude processing.
    /// </para>
    /// </remarks>
    /// <param name="source">A dataflow block containing the queued log entries.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected abstract Task ProcessAsync(IReceivableSourceBlock<TEntry> source);

    /// <summary>
    /// Disposes and/or finalizes the instance asynchronously.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> to dispose and finalize, <see langword="false"/> to finalize (unmanaged) only.
    /// </param>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    protected virtual async ValueTask DisposeAsync(bool disposing)
    {
        this.buffer.Complete();

        await this.buffer.Completion.ConfigureAwait(false);
        await this.task.ConfigureAwait(false);
    }
}
