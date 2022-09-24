namespace Microsoft.Extensions.Logging.Policies;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// A builder of policy for writing log entries.
/// </summary>
public class LoggingPolicyBuilder
{
    internal LoggingPolicyBuilder(IServiceCollection services, string providerName)
    {
        this.Services = services;
        this.ProviderName = providerName;
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
    /// Gets or sets the current entry filter used for policies, if any.
    /// </summary>
    public LogEntryFilter? Filter { get; set; }
}

/// <summary>
/// A builder of policy for writing log entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public class LoggingPolicyBuilder<TEntry> : LoggingPolicyBuilder
{
    internal LoggingPolicyBuilder(IServiceCollection services, string providerName)
        : base(services, providerName)
    {
    }
}
