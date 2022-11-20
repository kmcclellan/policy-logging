namespace Microsoft.Extensions.Logging.Policies;

abstract class LogSerializer<TEntry> : IAsyncDisposable
{
    readonly ILogBuffer<TEntry>?[] buffers = new ILogBuffer<TEntry>?[2];

    int index;
    ValueTask flush;

    protected virtual int FlushBytes { get; } = 4096;

    protected virtual TimeSpan FlushInterval { get; } = TimeSpan.FromSeconds(1);

    public void Write(ref TEntry entry)
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        return this.flush;
    }

    protected abstract ILogBuffer<TEntry> GetBuffer();

    protected abstract Task Flush(in ReadOnlySpan<byte> payload, CancellationToken cancellationToken);
}
