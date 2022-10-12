namespace Microsoft.Extensions.Logging.Policies;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions of <see cref="LoggingPolicyBuilder{TEntry}"/>.
/// </summary>
public static class LoggingPolicyBuilderExtensions
{
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
