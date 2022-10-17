namespace Microsoft.Extensions.Logging.Policies;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Policies.Core;
using Microsoft.Extensions.Options;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// A builder to configure <see cref="PolicyLoggingOptions{TEntry}"/> fluently.
/// </summary>
public class LoggingPolicyBuilder
{
    internal LoggingPolicyBuilder(IServiceCollection services)
    {
        this.Services = services;
    }

    /// <summary>
    /// Gets the logging services.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Configures using policies with the given log entry type.
    /// </summary>
    /// <typeparam name="TEntry">The log entry type.</typeparam>
    /// <param name="begin">The delegate to begin writing a log entry.</param>
    /// <param name="finish">A delegate to finish writing a log entry.</param>
    /// <returns>A typed logging policy builder.</returns>
    public LoggingPolicyBuilder<TEntry> WithEntries<TEntry>(Func<TEntry> begin, Action<TEntry>? finish = null)
    {
        ArgumentNullException.ThrowIfNull(begin, nameof(begin));

        this.Services.Configure<PolicyLoggingOptions<TEntry>>(
            opts =>
            {
                opts.Begin = begin;
                opts.Finish = finish;
            });

        this.Services.AddSingleton<ILoggerProvider, DefaultPolicyLoggerProvider<TEntry>>();

        return new(this.Services, null);
    }

    [ProviderAlias("Policies")]
    class DefaultPolicyLoggerProvider<TEntry> : PolicyLoggerProvider<TEntry>
    {
        readonly IDisposable reload;
        PolicyLoggingOptions<TEntry> options;

        public DefaultPolicyLoggerProvider(IOptionsMonitor<PolicyLoggingOptions<TEntry>> options)
        {
            this.reload = options.OnChange(this.LoadOptions);
            this.LoadOptions(options.CurrentValue);
        }

        protected override TEntry Begin()
        {
            return this.options.Begin is { } begin
                ? begin()
                : throw new InvalidOperationException(
                    $"{nameof(PolicyLoggingOptions<TEntry>)}.{nameof(this.options.Begin)} is required.");
        }

        protected override void Finish(TEntry entry)
        {
            this.options.Finish?.Invoke(entry);
        }

        [MemberNotNull(nameof(this.options))]
        void LoadOptions(PolicyLoggingOptions<TEntry> opts)
        {
            this.options = opts;
            this.SetPolicies(opts.Policies);
        }
    }
}

/// <summary>
/// A builder to configure <see cref="PolicyLoggingOptions{TEntry}"/> fluently.
/// </summary>
/// <typeparam name="TEntry">The policy log entry type.</typeparam>
public class LoggingPolicyBuilder<TEntry>
{
    internal LoggingPolicyBuilder(IServiceCollection services, LogEntryFilter? filter)
    {
        this.Services = services;
        this.Filter = filter;
    }

    /// <summary>
    /// Gets the logging services.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets the entry filter used for policies, if any.
    /// </summary>
    public LogEntryFilter? Filter { get; }

    /// <summary>
    /// Configures logging using the given filter.
    /// </summary>
    /// <param name="filter">The log entry filter.</param>
    /// <returns>A builder using the filter.</returns>
    public LoggingPolicyBuilder<TEntry> WithFilter(LogEntryFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));
        return new(this.Services, filter);
    }
}
