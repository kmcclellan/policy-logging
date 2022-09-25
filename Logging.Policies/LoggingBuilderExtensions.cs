namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// Extensions of <see cref="ILoggingBuilder"/> to configure policy logging.
/// </summary>
public static class LoggingBuilderExtensions
{
    /// <summary>
    /// Adds policy logging to the services using a custom <see cref="ILoggingPolicy{TEntry}"/>.
    /// </summary>
    /// <typeparam name="TEntry">The log entry type.</typeparam>
    /// <typeparam name="TPolicy">The logging policy type.</typeparam>
    /// <returns>A builder for log entry policies.</returns>
    public static ILogEntryPolicyBuilder<TEntry> AddPolicy<TEntry, TPolicy>(this ILoggingBuilder builder)
        where TPolicy : ILoggingPolicy<TEntry>
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds file logging to the services using a custom <see cref="ISerializationPolicy{TEntry}"/>.
    /// </summary>
    /// <typeparam name="TEntry">The log entry type.</typeparam>
    /// <typeparam name="TPolicy">The serialization policy type.</typeparam>
    /// <returns>A builder for log entry policies.</returns>
    public static ILogEntryPolicyBuilder<TEntry> AddFile<TEntry, TPolicy>(this ILoggingBuilder builder)
        where TPolicy : ISerializationPolicy<TEntry>
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds JSON file logging to the services.
    /// </summary>
    /// <typeparam name="TEntry">The log entry type.</typeparam>
    /// <returns>A builder for log entry policies.</returns>
    public static ILogEntryPolicyBuilder<TEntry> AddJsonFile<TEntry>(this ILoggingBuilder builder)
    {
        throw new NotImplementedException();
    }
}
