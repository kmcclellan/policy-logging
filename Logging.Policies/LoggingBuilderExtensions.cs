namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// Extensions of <see cref="ILoggingBuilder"/> to configure policy logging.
/// </summary>
public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Adds the default <see cref="PolicyLoggerProvider{TEntry}"/> to the services.
    /// </summary>
    /// <param name="builder">The logging builder.</param>
    /// <returns>A builder for the logging policies.</returns>
    public static LoggingPolicyBuilder AddPolicyProvider(this ILoggingBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        return new(builder.Services);
    }
}
