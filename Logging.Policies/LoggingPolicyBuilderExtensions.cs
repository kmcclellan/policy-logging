namespace Microsoft.Extensions.Logging.Policies;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions of <see cref="LoggingPolicyBuilder"/>.
/// </summary>
public static class LoggingPolicyBuilderExtensions
{
    /// <summary>
    /// Configures logging using the given entry type.
    /// </summary>
    /// <typeparam name="TEntry">The log entry type.</typeparam>
    /// <param name="builder">The target builder.</param>
    /// <param name="begin">The delegate to begin writing a log entry.</param>
    /// <param name="finish">A delegate to finish writing a log entry (optional).</param>
    /// <returns>A builder for the entry type.</returns>
    public static LoggingPolicyBuilder<TEntry> WithEntries<TEntry>(
        this LoggingPolicyBuilder builder,
        Func<TEntry> begin,
        Action<TEntry>? finish = null)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        ArgumentNullException.ThrowIfNull(begin, nameof(begin));

        builder.Services.AddSingleton<ILoggerProvider>(
            sp => ActivatorUtilities.CreateInstance<PolicyLoggerProvider<TEntry>>(sp, builder.ProviderName));

        builder.Services.Configure<PolicyLoggingOptions<TEntry>>(
            builder.ProviderName,
            opts =>
            {
                opts.Begin = begin;
                opts.Finish = finish;
            });

        return new(builder.Services, builder.ProviderName) { Filter = builder.Filter };
    }

    /// <summary>
    /// Configures logging using the given policy.
    /// </summary>
    /// <typeparam name="TEntry">The log entry type.</typeparam>
    /// <param name="builder">The target builder.</param>
    /// <param name="policy">The logging policy.</param>
    /// <returns>The same builder instance, for chaining.</returns>
    public static LoggingPolicyBuilder<TEntry> WithPolicy<TEntry>(
        this LoggingPolicyBuilder<TEntry> builder,
        LoggingPolicy<TEntry> policy)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        ArgumentNullException.ThrowIfNull(policy, nameof(policy));

        policy.Filter ??= builder.Filter;
        builder.Services.Configure<PolicyLoggingOptions<TEntry>>(builder.ProviderName, x => x.Policies.Add(policy));

        return builder;
    }

    /// <summary>
    /// Configures capturing named log properties.
    /// </summary>
    /// <typeparam name="TEntry">The log entry type.</typeparam>
    /// <param name="builder">The target builder.</param>
    /// <param name="action">The delegate to capture a log property.</param>
    /// <param name="includeScopes">
    /// <see langword="true"/> to include properties from log scopes, otherwise <see langword="false"/>.
    /// </param>
    /// <returns>The same builder instance, for chaining.</returns>
    public static LoggingPolicyBuilder<TEntry> WithProperties<TEntry>(
        this LoggingPolicyBuilder<TEntry> builder,
        Action<TEntry, string, object?> action,
        bool includeScopes)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        var policy = new LogPropertyPolicy<TEntry>(action);
        return WithPolicy(builder, new() { State = policy, Scopes = includeScopes ? policy : null });
    }
}
