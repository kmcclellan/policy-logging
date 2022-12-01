namespace Microsoft.Extensions.Logging.Policies.Serialization;

using Microsoft.Extensions.Options;

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

class JsonSerializerFactory<TEntry> : IBufferSerializerFactory<TEntry>
{
    readonly IOptions<JsonLoggingOptions> options;

    public JsonSerializerFactory(IOptions<JsonLoggingOptions> options)
    {
        this.options = options;
    }

    public ILogSerializer<TEntry> Create(IBufferWriter<byte> bytes)
    {
        return new JsonLogSerializer(this.options, bytes);
    }

    class JsonLogSerializer : ILogSerializer<TEntry>
    {
        readonly IOptions<JsonLoggingOptions> options;
        readonly IBufferWriter<byte> bytes;

        Utf8JsonWriter writer;
        JsonSerializerOptions serializerOptions;

        public JsonLogSerializer(IOptions<JsonLoggingOptions> options, IBufferWriter<byte> bytes)
        {
            this.options = options;
            this.bytes = bytes;

            this.SetWriter(options.Value.Serializer);
        }

        public void Serialize(ref TEntry entry)
        {
            var opts = this.options.Value;

            if (opts.Serializer != this.serializerOptions)
            {
                SetWriter(opts.Serializer);
            }

            JsonSerializer.Serialize(this.writer, entry, opts.Serializer);

            if (opts.Delimiter.Length > 0)
            {
                this.writer.WriteRawValue(opts.Delimiter, skipInputValidation: true);
            }

            this.writer.Flush();
        }

        [MemberNotNull(nameof(writer), nameof(serializerOptions))]
        void SetWriter(JsonSerializerOptions opts)
        {
            var writerOpts = new JsonWriterOptions
            {
                Encoder = opts.Encoder,
                Indented = opts.WriteIndented,
                MaxDepth = opts.MaxDepth == 0 ? 64 : opts.MaxDepth,
#if !DEBUG
                SkipValidation = true,
#endif
            };

            // No need to dispose writer without underlying stream.
            this.writer = new(this.bytes, writerOpts);
            this.serializerOptions = opts;
        }
    }
}
