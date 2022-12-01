namespace Microsoft.Extensions.Logging.Policies.Serialization;

using System.Buffers;

class LogBuffer<TEntry> : ILogBuffer<TEntry>
{
    readonly ArrayBufferWriter<byte> bytes = new();

    private LogBuffer(IBufferSerializerFactory<TEntry> serializers)
    {
        this.Serializer = serializers.Create(this.bytes);
    }

    public ILogSerializer<TEntry> Serializer { get; }

    public int WrittenCount => this.bytes.WrittenCount;

    public ReadOnlySpan<byte> WrittenSpan => this.bytes.WrittenSpan;

    public static ILogBuffer<TEntry>.Factory GetFactory(IBufferSerializerFactory<TEntry> serializers)
    {
        return () => new LogBuffer<TEntry>(serializers);
    }

    public void Clear()
    {
        this.bytes.Clear();
    }
}
