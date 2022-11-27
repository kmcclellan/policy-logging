namespace Microsoft.Extensions.Logging.Policies.Serialization;

interface ILogSerializer<TEntry>
{
    void Write(ref TEntry entry);
}
