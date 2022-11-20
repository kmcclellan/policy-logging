namespace Microsoft.Extensions.Logging.Policies;

interface ILogBuffer<TEntry>
{
    ref ReadOnlySpan<byte> Buffer { get; }

    void Write(ref TEntry entry);
}
