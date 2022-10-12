namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// Extensions of <see cref="ILoggingBuilder"/> to configure policy logging.
/// </summary>
public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Adds a policy logger provider to the services.
    /// </summary>
    /// <typeparam name="TEntry">The log entry type for policies.</typeparam>
    /// <param name="builder">The logging builder.</param>
    /// <param name="providerName">The provider name, for log level filtering.</param>
    /// <returns>A builder for the logging policy.</returns>
    public static LoggingPolicyBuilder<TEntry> AddPolicyProvider<TEntry>(this ILoggingBuilder builder, string providerName = "Policies")
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        return new(builder.Services, providerName);
    }
}
