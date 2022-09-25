namespace Microsoft.Extensions.Logging.Policies;

/// <summary>
/// Extensions of <see cref="ILogEntryPolicyBuilder{TEntry}"/>.
/// </summary>
public static class LogEntryPolicyBuilderExtensions
{
    /// <summary>
    /// Configures log entry formatting using a log property.
    /// </summary>
    /// <typeparam name="TEntry">The log entry type.</typeparam>
    /// <param name="builder">The log entry policy builder.</param>
    /// <param name="format">The format action.</param>
    /// <returns>The same policy builder instance, for chaining.</returns>
    public static ILogEntryPolicyBuilder<TEntry> OnProperty<TEntry>(
        this ILogEntryPolicyBuilder<TEntry> builder,
        Action<TEntry, string, object?> format)
    {
        throw new NotImplementedException();
    }
}
