namespace Microsoft.Extensions.Logging.Policies;

abstract class LogSerializer<TEntry> : IAsyncDisposable
{
    readonly object sync = new();

    ILogBuffer<TEntry>? writing;
    ILogBuffer<TEntry>? flushing;
    DateTime? start;
    Task flush = Task.CompletedTask;

    protected virtual int FlushBytes { get; } = 4096;

    protected virtual TimeSpan FlushInterval { get; } = TimeSpan.FromSeconds(1);

    public void Write(ref TEntry entry)
    {
        lock (this.sync)
        {
            var buffer = this.writing ??= this.GetBuffer();
            buffer.Write(ref entry);

            if (this.flush.IsCompletedSuccessfully && this.TryRotate(buffer))
            {
                this.flush = this.FlushLoop(buffer);
            }
        }
    }

    public ValueTask DisposeAsync()
    {
        return new(this.flush);
    }

    protected abstract ILogBuffer<TEntry> GetBuffer();

    protected abstract Task Flush(in ReadOnlySpan<byte> payload, CancellationToken cancellationToken);

    bool TryRotate(ILogBuffer<TEntry> buffer)
    {
        this.start ??= DateTime.UtcNow;

        if (DateTime.UtcNow >= this.start + this.FlushInterval || buffer.Bytes.Length >= this.FlushBytes)
        {
            this.writing = this.flushing;
            this.flushing = buffer;
            this.start = null;
            return true;
        }

        return false;
    }

    async Task FlushLoop(ILogBuffer<TEntry> buffer)
    {
        bool active;

        do
        {
            await this.Flush(buffer.Bytes, CancellationToken.None);
            buffer.Reset();

            lock (this.sync)
            {
                active = this.writing != null && this.TryRotate(buffer = this.writing);
            }
        }
        while (active);
    }
}
