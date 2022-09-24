namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// A policy to collect log properties.
/// </summary>
/// <typeparam name="TState">The type of log entry state.</typeparam>
public interface ILogPropertyPolicy<in TState>
{
    /// <summary>
    /// Collects a log property.
    /// </summary>
    /// <param name="entry">The log entry state.</param>
    /// <param name="name">The log property name.</param>
    /// <param name="value">The log property value.</param>
    void AddProperty(TState entry, string name, object? value);
}
