namespace Microsoft.Extensions.Logging.Policies;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// A builder of policy for writing log entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public class LoggingPolicyBuilder<TEntry>
{
    internal LoggingPolicyBuilder(IServiceCollection services, string providerName, LogEntryFilter? filter = null)
    {
        this.Services = services;
        this.ProviderName = providerName;
        this.Filter = filter;
    }

    /// <summary>
    /// Gets the logging services.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets the name of the logger provider.
    /// </summary>
    public string ProviderName { get; }

    /// <summary>
    /// Gets the entry filter used for policies, if any.
    /// </summary>
    public LogEntryFilter? Filter { get; }

    /// <summary>
    /// Configures logging using the given filter.
    /// </summary>
    /// <param name="filter">The log entry filter.</param>
    /// <returns>A builder using the filter.</returns>
    public LoggingPolicyBuilder<TEntry> WithFilter(LogEntryFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));
        return new(this.Services, this.ProviderName, filter);
    }
}
