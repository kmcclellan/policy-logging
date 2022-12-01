namespace Microsoft.Extensions.Logging.Policies.Serialization;
/// <summary>
/// Options for serializing log entries to a file.
/// </summary>
public class FileLoggingOptions
{
    /// <summary>
    /// The path to the output log file.
    /// </summary>
    /// <remarks>
    /// Supports application and environment as format items. Default is <c>{0}.{1}.log</c>.
    /// </remarks>
    public string Path { get; set; } = "{0}.{1}.log";
}
