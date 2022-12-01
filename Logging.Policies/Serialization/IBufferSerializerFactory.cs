namespace Microsoft.Extensions.Logging.Policies.Serialization;

using System.Buffers;

interface IBufferSerializerFactory<TEntry>
{
    ILogSerializer<TEntry> Create(IBufferWriter<byte> bytes);
}
