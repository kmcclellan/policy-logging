namespace Microsoft.Extensions.Logging.Policies;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System.Collections.Concurrent;

static class PolicyLoggerProvider
{
    public static ServiceDescriptor Describe<TEntry, TState>(string providerName)
    {
        return ServiceDescriptor.Singleton<ILoggerProvider>(
            sp => ActivatorUtilities.CreateInstance<PolicyLoggerProvider<TEntry, TState>>(sp, providerName));
    }
}

class PolicyLoggerProvider<TEntry, TState> : ILoggerProvider
{
    readonly IOptionsMonitor<PolicyLoggerOptions<TEntry, TState>> options;
    readonly string providerName;
    readonly ConcurrentDictionary<string, PolicyLogger<TEntry, TState>> loggers = new();
    readonly ScopeStack<TState> scopes = new();
    readonly IDisposable reload;

    public PolicyLoggerProvider(IOptionsMonitor<PolicyLoggerOptions<TEntry, TState>> options, string providerName)
    {
        this.options = options;
        this.providerName = providerName;

        this.reload = options.OnChange(
            (opts, name) =>
            {
                if (name == providerName)
                {
                    foreach (var logger in this.loggers.Values)
                    {
                        logger.SetOptions(opts);
                    }
                }
            });
    }

    public ILogger CreateLogger(string categoryName)
    {
        return this.loggers.GetOrAdd(
            categoryName,
            (s, p) => new PolicyLogger<TEntry, TState>(p.scopes, s, p.options.Get(p.providerName)),
            this);
    }

    public void Dispose()
    {
        this.reload.Dispose();
    }
}
