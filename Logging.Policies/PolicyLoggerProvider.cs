namespace Microsoft.Extensions.Logging.Policies;

using System.Collections.Concurrent;

/// <summary>
/// A logger provider using logging policies.
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public abstract class PolicyLoggerProvider<TEntry> : ILoggerProvider
{
    readonly ConcurrentDictionary<string, PolicyLogger<TEntry>> loggers = new();
    readonly ScopeStack<TEntry> scopes = new();

    PolicyLoggingOptions<TEntry>? options;

    /// <inheritdoc/>
    public ILogger CreateLogger(string categoryName)
    {
        return this.loggers.GetOrAdd(
            categoryName,
            (cat, prov) =>
            {
                var logger = new PolicyLogger<TEntry>(prov.scopes, cat);

                if (prov.options != null)
                {
                    logger.SetOptions(prov.options);
                }

                return logger;
            },
            this);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Sets the options for policy logging.
    /// </summary>
    /// <param name="options">The logging options.</param>
    protected void SetOptions(PolicyLoggingOptions<TEntry> options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));

        foreach (var logger in this.loggers.Values)
        {
            logger.SetOptions(options);
        }
    }

    /// <summary>
    /// Disposes and/or finalizes the instance.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> to dispose and finalize, <see langword="false"/> to finalize only.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
    }
}
