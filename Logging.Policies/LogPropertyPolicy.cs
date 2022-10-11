namespace Microsoft.Extensions.Logging.Policies;

class LogPropertyPolicy<TEntry> : ILogScopePolicy<TEntry>, ILogStatePolicy<TEntry>
{
    readonly Action<TEntry, string, object?> action;

    public LogPropertyPolicy(Action<TEntry, string, object?> action)
    {
        this.action = action;
    }

    public ILogScope<TEntry> Create()
    {
        return new PropertyScope(this.action, Array.Empty<KeyValuePair<string, object?>>());
    }

    public void OnEntry<TState>(TEntry entry, TState state)
    {
        if (state is IReadOnlyList<KeyValuePair<string, object?>> props)
        {
            for (var i = 0; i < props.Count; i++)
            {
                this.action(entry, props[i].Key, props[i].Value);
            }
        }
    }

    class PropertyScope : ILogScope<TEntry>
    {
        readonly Action<TEntry, string, object?> action;
        readonly KeyValuePair<string, object?>[] properties;

        public PropertyScope(Action<TEntry, string, object?> action, KeyValuePair<string, object?>[] properties)
        {
            this.action = action;
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

                return new PropertyScope(this.action, included);
            }

            return null;
        }

        public void OnEntry(TEntry entry)
        {
            foreach (var (key, value) in this.properties)
            {
                this.action(entry, key, value);
            }
        }
    }
}
