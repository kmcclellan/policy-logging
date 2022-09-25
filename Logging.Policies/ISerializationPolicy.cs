namespace Microsoft.Extensions.Logging.Policies;

using System.Buffers;

/// <summary>
/// A policy for serializing log entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public interface ISerializationPolicy<TEntry> : ILoggingPolicy<TEntry>
{
    /// <summary>
    /// Flushes the log entry bytes.
    /// </summary>
    /// <param name="entry">The log entry.</param>
    /// <param name="writer">The target buffer.</param>
    void Flush(TEntry entry, IBufferWriter<byte> writer);
}
