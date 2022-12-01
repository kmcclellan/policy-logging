namespace Microsoft.Extensions.Logging.Policies.Serialization;

interface ILogBuffer<TEntry>
{
    delegate ILogBuffer<TEntry> Factory();

    ILogSerializer<TEntry> Serializer { get; }

    int WrittenCount { get; }

    ReadOnlyMemory<byte> WrittenMemory { get; }

    void Clear();
}
