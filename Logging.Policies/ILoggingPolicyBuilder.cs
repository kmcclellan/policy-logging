namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A builder of policy for writing log entries of a given type.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public interface ILoggingPolicyBuilder<TEntry>
{
    /// <summary>
    /// Sets the logging policy factory for the provider.
    /// </summary>
    /// <param name="factory">The policy factory.</param>
    /// <returns>The same builder instance, for chaining.</returns>
    ILoggingPolicyBuilder<TEntry> SetFactory(Func<IServiceProvider, ILoggingPolicy<TEntry>> factory);

    /// <summary>
    /// Configures policy log entries using a log entry builder.
    /// </summary>
    /// <param name="configure">The configure action.</param>
    /// <param name="filter">An optional log entry filter.</param>
    /// <returns>The same builder instance, for chaining.</returns>
    ILoggingPolicyBuilder<TEntry> ConfigureEntries(
        Action<ILogEntryBuilder<TEntry>> configure,
        LogEntryFilter? filter = null);
}
