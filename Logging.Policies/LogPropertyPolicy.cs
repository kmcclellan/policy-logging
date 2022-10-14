namespace Microsoft.Extensions.Logging.Policies;

using Microsoft.Extensions.Logging.Policies.Core;

/// <summary>
/// A base policy for capturing named log properties (i.e. message template arguments).
/// </summary>
/// <typeparam name="TEntry">The log entry type.</typeparam>
public abstract class LogPropertyPolicy<TEntry> : ILogScopePolicy<TEntry>, ILogStatePolicy<TEntry>
{
    /// <inheritdoc/>
    public ILogScope<TEntry> Create()
    {
        return new PropertyScope(this, Array.Empty<KeyValuePair<string, object?>>());
    }

    /// <inheritdoc/>
    public void OnEntry<TState>(TEntry entry, TState state)
    {
        if (state is IReadOnlyList<KeyValuePair<string, object?>> props)
        {
            for (var i = 0; i < props.Count; i++)
            {
                this.OnProperty(entry, props[i].Key, props[i].Value);
            }
        }
    }

    /// <summary>
    /// Captures a log property.
    /// </summary>
    /// <param name="entry">The target log entry.</param>
    /// <param name="name">The property name.</param>
    /// <param name="value">The property value.</param>
    protected abstract void OnProperty(TEntry entry, string name, object? value);

    class PropertyScope : ILogScope<TEntry>
    {
        readonly LogPropertyPolicy<TEntry> policy;
        readonly KeyValuePair<string, object?>[] properties;

        public PropertyScope(LogPropertyPolicy<TEntry> policy, KeyValuePair<string, object?>[] properties)
        {
            this.policy = policy;
            this.properties = properties;
        }

        public ILogScope<TEntry>? Include<TState>(TState state)
        {
            if (state is IReadOnlyList<KeyValuePair<string, object?>> props)
            {
                var included = new KeyValuePair<string, object?>[this.properties.Length + props.Count];
                this.properties.CopyTo(included, 0);

                for (var i = 0; i < props.Count; i++)
                {
                    included[this.properties.Length + i] = props[i];
                }

                return new PropertyScope(this.policy, included);
            }

            return null;
        }

        public void OnEntry(TEntry entry)
        {
            foreach (var (key, value) in this.properties)
            {
                this.policy.OnProperty(entry, key, value);
            }
        }
    }
}
