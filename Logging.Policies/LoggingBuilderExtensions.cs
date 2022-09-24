namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// Extensions of <see cref="ILoggingBuilder"/> to configure policy logging.
/// </summary>
public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Adds policy logging to the services.
    /// </summary>
    /// <param name="builder">The logging builder.</param>
    /// <returns>A builder for policy logging.</returns>
    public static ILoggingPolicyBuilder AddPolicy(this ILoggingBuilder builder)
    {
        throw new NotImplementedException(); 
    }
}
