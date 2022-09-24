namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A factory to create logger policies.
/// </summary>
/// <typeparam name="TState">The type of log entry state.</typeparam>
public interface ILoggerPolicyFactory<TState>
{
    /// <summary>
    /// Creates a policy for logging with the given category.
    /// </summary>
    /// <param name="category">The log category name.</param>
    /// <returns>The logging policy.</returns>
    ILoggerPolicy<TState> Create(string category);
}
