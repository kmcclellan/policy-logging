namespace Microsoft.Extensions.Logging.Policies;

class ScopeStack<TEntry>
{
    readonly AsyncLocal<Dictionary<ILogScopePolicy<TEntry>, Stack<ILogScope<TEntry>>>> stacks = new();

    public ILogScope<TEntry>? Peek(ILogScopePolicy<TEntry> policy)
    {
        return this.stacks.Value != null &&
            this.stacks.Value.TryGetValue(policy, out var stack) &&
            stack.TryPeek(out var scope)
            ? scope
            : null;
    }

    public IDisposable Push(ILogScopePolicy<TEntry> policy, ILogScope<TEntry> scope)
    {
        this.stacks.Value ??= new();

        if (!this.stacks.Value.TryGetValue(policy, out var stack))
        {
            stack = new();
            this.stacks.Value.Add(policy, stack);
        }

        stack.Push(scope);
        return new Popper(this.stacks.Value, policy, stack.Count - 1);
    }

    class Popper : IDisposable
    {
        readonly Dictionary<ILogScopePolicy<TEntry>, Stack<ILogScope<TEntry>>> stacks;
        readonly ILogScopePolicy<TEntry> policy;
        readonly int target;

        public Popper(
            Dictionary<ILogScopePolicy<TEntry>, Stack<ILogScope<TEntry>>> stacks,
            ILogScopePolicy<TEntry> policy,
            int target)
        {
            this.stacks = stacks;
            this.policy = policy;
            this.target = target;
        }

        public void Dispose()
        {
            if (this.stacks.TryGetValue(this.policy, out var stack))
            {
                while (stack.Count > this.target)
                {
                    stack.Pop();
                }

                if (this.target == 0)
                {
                    this.stacks.Remove(this.policy);
                }
            }
        }
    }
}
