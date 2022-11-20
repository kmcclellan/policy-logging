namespace Microsoft.Extensions.Logging.Policies;

abstract class LogSerializer<TWriter, TEntry> : IAsyncDisposable
{
    readonly TWriter[] writers;
    ValueTask flush;

    public LogSerializer()
    {
        this.writers = new[] { this.GetWriter(), this.GetWriter() };
    }

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

    protected abstract TWriter GetWriter();

    protected abstract int Write(TWriter writer, ref TEntry entry);

    protected abstract ref ReadOnlySpan<byte> GetPayload(TWriter writer);

    protected abstract Task Flush(ReadOnlySpan<byte> payload, CancellationToken cancellationToken);
}
