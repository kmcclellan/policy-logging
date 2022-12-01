namespace Microsoft.Extensions.Logging.Policies.Serialization;

using System.Text.Json;
using System.Text;

/// <summary>
/// Options for serializing log entries as JSON.
/// </summary>
public class JsonLoggingOptions
{
    /// <summary>
    /// Gets or sets the JSON serializer options.
    /// </summary>
    public JsonSerializerOptions Serializer { get; set; } = JsonSerializerOptions.Default;

    /// <summary>
    /// Gets or sets the bytes delimiting log entries.
    /// </summary>
    /// <remarks>
    /// Default is <see cref="Environment.NewLine"/> (UTF-8).
    /// </remarks>
    public byte[] Delimiter { get; set; } = Encoding.UTF8.GetBytes(Environment.NewLine);
}
