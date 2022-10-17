namespace Microsoft.Extensions.Logging.Policies;

using Microsoft.Extensions.Logging.Policies.Core;

class PolicyLoggingOptions<TEntry>
{
    public Func<TEntry>? Begin { get; set; }

    public Action<TEntry>? Finish { get; set; }

    public ICollection<LoggingPolicy<TEntry>> Policies { get; } = new HashSet<LoggingPolicy<TEntry>>();
}
