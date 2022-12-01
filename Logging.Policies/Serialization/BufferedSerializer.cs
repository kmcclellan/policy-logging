namespace Microsoft.Extensions.Logging.Policies.Serialization;

using Microsoft.Extensions.Options;

using System.Buffers;

class BufferedSerializer<TEntry> : ILogSerializer<TEntry>, IAsyncDisposable
{
    readonly IOptions<LogBufferOptions> options;
    readonly ILogBuffer<TEntry>.Factory factory;
    readonly ILogOutput output;

    readonly object sync = new();

    ILogBuffer<TEntry>? writing, flushing;
    DateTime? start;
    Task flush = Task.CompletedTask;

    public BufferedSerializer(
        IOptions<LogBufferOptions> options,
        ILogBuffer<TEntry>.Factory factory,
        ILogOutput output)
    {
        this.options = options;
        this.factory = factory;
        this.output = output;
    }

    public void Serialize(ref TEntry entry)
    {
        lock (this.sync)
        {
            if (this.writing == null)
            {
                var bytes = new ArrayBufferWriter<byte>();
                this.writing = this.factory();
            }

            var buffer = this.writing;
            buffer.Serializer.Serialize(ref entry);

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

    bool TryRotate(ILogBuffer<TEntry> buffer)
    {
        this.start ??= DateTime.UtcNow;
        var opts = this.options.Value;

        if (DateTime.UtcNow >= this.start + opts.Interval || buffer.WrittenCount >= opts.Size)
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
            await this.output.WriteAsync(buffer.WrittenSpan, CancellationToken.None).ConfigureAwait(false);
            buffer.Clear();

            lock (this.sync)
            {
                active = this.writing != null && this.TryRotate(buffer = this.writing);
            }
        }
        while (active);
    }
}
