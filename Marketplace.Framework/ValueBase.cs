namespace Marketplace.Framework
{
    public abstract record ValueBase<T> where T : ValueBase<T>
    {
        public T? Value { get; protected set; }

        public override string ToString() => Value == null ? "" : Value.ToString();
    }
}