namespace Microsoft.Extensions.Logging.Policies;

interface ILogBuffer<TEntry>
{
    ReadOnlySpan<byte> Bytes { get; }

    void Write(ref TEntry entry);

    void Reset();
}
