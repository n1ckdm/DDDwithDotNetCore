namespace Marketplace.Framework
{
    public abstract class AggregateRoot<TId>
    {
        public TId Id { get; protected set; }

        protected abstract void When(object @event);
        private readonly List<object> _changes;

        protected AggregateRoot() => _changes = new List<object>();

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _changes.Add(@event);
        }

        public IEnumerable<object> GetChanges() => _changes.AsEnumerable();

        protected abstract void EnsureValidState();
    }
}