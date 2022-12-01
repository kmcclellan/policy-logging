namespace Microsoft.Extensions.Logging.Policies.Serialization;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

class FileOutput : ILogOutput
{
    static readonly FileStreamOptions StreamOptions = new()
    {
        // We do our own buffering in the serializer.
        BufferSize = 0,
        Mode = FileMode.Append,
        Access = FileAccess.Write,
        Share = FileShare.Read,
        Options = FileOptions.Asynchronous,
    };

    readonly IOptions<FileLoggingOptions> options;
    readonly IHostEnvironment env;

    public FileOutput(IOptions<FileLoggingOptions> options, IHostEnvironment env)
    {
        this.options = options;
        this.env = env;
    }

    public async Task WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
    {
        var path = Path.Combine(
            this.env.ContentRootPath,
            string.Format(
                CultureInfo.InvariantCulture,
                this.options.Value.Path,
                this.env.ApplicationName,
                this.env.EnvironmentName));

        using var stream = File.Open(path, StreamOptions);
        await stream.WriteAsync(buffer, cancellationToken);
    }
}
