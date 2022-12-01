namespace Microsoft.Extensions.Logging.Policies.Serialization;

interface ILogOutput
{
    Task WriteAsync(ReadOnlySpan<byte> buffer, CancellationToken cancellationToken);
}
