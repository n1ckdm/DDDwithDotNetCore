namespace Marketplace.Domain
{
    public record Money
    {
        public decimal Amount { get; }
        public Money(decimal amount) => Amount = amount;
    }
}