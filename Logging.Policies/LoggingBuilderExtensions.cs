namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// Extensions of <see cref="ILoggingBuilder"/> to configure policy logging.
/// </summary>
public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Adds a policy logger provider to the services.
    /// </summary>
    /// <param name="builder">The logging builder.</param>
    /// <param name="providerName">The provider name, for log level filtering.</param>
    /// <returns>A builder for the logging policy.</returns>
    public static LoggingPolicyBuilder AddPolicyProvider(this ILoggingBuilder builder, string providerName = "Policies")
    {
        throw new NotImplementedException();
    }
}
