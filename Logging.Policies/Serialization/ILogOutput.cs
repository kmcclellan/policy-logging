namespace Microsoft.Extensions.Logging.Policies.Serialization;

interface ILogOutput
{
    Task WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken);
}
