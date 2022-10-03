namespace Microsoft.Extensions.Logging.Policies;

class ScopeStack<TState>
{
    readonly AsyncLocal<LocalStack> stacks = new();

    LocalStack Stack => this.stacks.Value ??= new();

    public IDisposable Begin(TState state)
    {
        this.Stack.Push(state);
        return this.Stack.GetPopper();
    }

    public Stack<TState>.Enumerator GetEnumerator()
    {
        return this.Stack.GetEnumerator();
    }

    class LocalStack : Stack<TState>
    {
        readonly List<Popper> poppers = new();

        public IDisposable GetPopper()
        {
            for (var i = this.poppers.Count; i < this.Count; i++)
            {
                this.poppers.Add(new(this, i));
            }

            var popper = this.poppers[this.Count - 1];
            popper.Reset();

            return popper;
        }

        class Popper : IDisposable
        {
            readonly Stack<TState> stack;
            readonly int target;

            bool disposed;

            public Popper(Stack<TState> stack, int target)
            {
                this.stack = stack;
                this.target = target;
            }

            public void Reset()
            {
                this.disposed = false;
            }

            public void Dispose()
            {
                while (!disposed && this.stack.Count > this.target)
                {
                    this.stack.Pop();
                }

                disposed = true;
            }
        }
    }
}
