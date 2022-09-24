namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A builder to define logging policy.
/// </summary>
public interface ILoggingPolicyBuilder
{
    /// <summary>
    /// Configures policy logging using a custom <see cref="ILoggerPolicyFactory{T}"/>.
    /// </summary>
    /// <typeparam name="TState">The type of log entry state.</typeparam>
    /// <typeparam name="TFactory">The type of logger policy factory.</typeparam>
    /// <returns>The same builder instance, for chaining.</returns>
    ILoggingPolicyBuilder AddFactory<TState, TFactory>()
        where TFactory : ILoggerPolicyFactory<TState>;
}
