namespace Microsoft.Extensions.Logging.Policies.Serialization;

/// <summary>
/// A serializer of typed log entries.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public interface ILogSerializer<TEntry>
{
    /// <summary>
    /// Serializes a log entry to underlying storage.
    /// </summary>
    /// <param name="entry">The log entry.</param>
    void Serialize(ref TEntry entry);
}
