namespace Microsoft.Extensions.Logging.Policies;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Policies.Core;

/// <summary>
/// Extensions of <see cref="LoggingPolicyBuilder{TEntry}"/>.
/// </summary>
public static class LoggingPolicyBuilderExtensions
{
    /// <summary>
    /// Configures an <see cref="ILogFieldPolicy{TEntry}"/> using the given action.
    /// </summary>
    /// <typeparam name="TEntry">The log entry type.</typeparam>
    /// <param name="builder">The target builder.</param>
    /// <param name="action">The log field action.</param>
    /// <returns>The same builder instance, for chaining.</returns>
    public static LoggingPolicyBuilder<TEntry> WithFields<TEntry>(
        this LoggingPolicyBuilder<TEntry> builder,
        Action<TEntry, string, LogLevel, EventId> action)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        return WithPolicy(builder, new() { Fields = new ActionFieldPolicy<TEntry>(action) });
    }

    /// <summary>
    /// Configures an <see cref="ILogMessagePolicy{TEntry}"/> using the given action.
    /// </summary>
    /// <typeparam name="TEntry">The log entry type.</typeparam>
    /// <param name="builder">The target builder.</param>
    /// <param name="action">The message action.</param>
    /// <returns>The same builder instance, for chaining.</returns>
    public static LoggingPolicyBuilder<TEntry> WithMessages<TEntry>(
        this LoggingPolicyBuilder<TEntry> builder,
        Action<TEntry, string> action)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        return WithPolicy(builder, new() { Messages = new ActionMessagePolicy<TEntry>(action) });
    }

    /// <summary>
    /// Configures an <see cref="ILogExceptionPolicy{TEntry}"/> using the given action.
    /// </summary>
    /// <typeparam name="TEntry">The log entry type.</typeparam>
    /// <param name="builder">The target builder.</param>
    /// <param name="action">The exception action.</param>
    /// <returns>The same builder instance, for chaining.</returns>
    public static LoggingPolicyBuilder<TEntry> WithExceptions<TEntry>(
        this LoggingPolicyBuilder<TEntry> builder,
        Action<TEntry, Exception> action)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        return WithPolicy(builder, new() { Exceptions = new ActionExceptionPolicy<TEntry>(action) });
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

        var policy = new ActionPropertyPolicy<TEntry>(action);
        return WithPolicy(builder, new() { State = policy, Scopes = includeScopes ? policy : null });
    }

    static LoggingPolicyBuilder<TEntry> WithPolicy<TEntry>(
        LoggingPolicyBuilder<TEntry> builder,
        LoggingPolicy<TEntry> policy)
    {
        policy.Filter ??= builder.Filter;
        builder.Services.Configure<PolicyLoggingOptions<TEntry>>(x => x.Policies.Add(policy));

        return builder;
    }

    class ActionFieldPolicy<TEntry> : ILogFieldPolicy<TEntry>
    {
        readonly Action<TEntry, string, LogLevel, EventId> action;

        public ActionFieldPolicy(Action<TEntry, string, LogLevel, EventId> action)
        {
            this.action = action;
        }

        public void OnEntry(TEntry entry, string category, LogLevel level, EventId id)
        {
            this.action(entry, category, level, id);
        }
    }

    class ActionMessagePolicy<TEntry> : ILogMessagePolicy<TEntry>
    {
        readonly Action<TEntry, string> action;

        public ActionMessagePolicy(Action<TEntry, string> action)
        {
            this.action = action;
        }

        public void OnEntry(TEntry entry, string category)
        {
            this.action(entry, category);
        }
    }

    class ActionExceptionPolicy<TEntry> : ILogExceptionPolicy<TEntry>
    {
        readonly Action<TEntry, Exception> action;

        public ActionExceptionPolicy(Action<TEntry, Exception> action)
        {
            this.action = action;
        }

        public void OnEntry(TEntry entry, Exception exception)
        {
            this.action(entry, exception);
        }
    }

    class ActionPropertyPolicy<TEntry> : LogPropertyPolicy<TEntry>
    {
        readonly Action<TEntry, string, object?> action;

        public ActionPropertyPolicy(Action<TEntry, string, object?> action)
        {
            this.action = action;
        }

        protected override void OnProperty(TEntry entry, string name, object? value)
        {
            this.action(entry, name, value);
        }
    }
}
