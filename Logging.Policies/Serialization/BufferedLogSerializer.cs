namespace Microsoft.Extensions.Logging.Policies.Serialization;

using System.Buffers;

abstract class BufferedLogSerializer<TEntry> : ILogSerializer<TEntry>, IAsyncDisposable
{
    readonly object sync = new();

    BufferNode? writing, flushing;
    DateTime? start;
    Task flush = Task.CompletedTask;

    protected virtual int BufferSize { get; } = 4096;

    protected virtual TimeSpan FlushInterval { get; } = TimeSpan.FromSeconds(1);

    public void Write(ref TEntry entry)
    {
        lock (this.sync)
        {
            if (this.writing == null)
            {
                var bytes = new ArrayBufferWriter<byte>();
                this.writing = new(bytes, this.Create(bytes));
            }

            var node = this.writing;
            node.Entries.Write(ref entry);

            if (this.flush.IsCompletedSuccessfully && this.TryRotate(node))
            {
                this.flush = this.FlushLoop(node);
            }
        }
    }

    public ValueTask DisposeAsync()
    {
        return new(this.flush);
    }

    protected abstract ILogSerializer<TEntry> Create(IBufferWriter<byte> buffer);

    protected abstract Task Flush(ReadOnlySpan<byte> payload, CancellationToken cancellationToken);

    bool TryRotate(BufferNode node)
    {
        this.start ??= DateTime.UtcNow;

        if (DateTime.UtcNow >= this.start + this.FlushInterval || node.Bytes.WrittenCount >= this.BufferSize)
        {
            this.writing = this.flushing;
            this.flushing = node;
            this.start = null;
            return true;
        }

        return false;
    }

    async Task FlushLoop(BufferNode node)
    {
        bool active;

        do
        {
            await this.Flush(node.Bytes.WrittenSpan, CancellationToken.None).ConfigureAwait(false);
            node.Bytes.Clear();

            lock (this.sync)
            {
                active = this.writing != null && this.TryRotate(node = this.writing);
            }
        }
        while (active);
    }

    class BufferNode
    {
        public BufferNode(ArrayBufferWriter<byte> bytes, ILogSerializer<TEntry> entries)
        {
            this.Bytes = bytes;
            this.Entries = entries;
        }

        public ArrayBufferWriter<byte> Bytes { get; }

        public ILogSerializer<TEntry> Entries { get; }
    }
}
