namespace Microsoft.Extensions.Logging.Policies;

using Microsoft.Extensions.Options;

class PolicyLoggerProvider<TEntry, TState> : ILoggerProvider
{
    public PolicyLoggerProvider(IOptionsMonitor<PolicyLoggerOptions<TEntry, TState>> options, string providerName)
    {
        throw new NotImplementedException();
    }

    public ILogger CreateLogger(string categoryName)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
