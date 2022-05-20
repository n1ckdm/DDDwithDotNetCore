namespace Marketplace.Domain
{
    public record Money
    {
        public decimal Amount { get; }
        public Money(decimal amount) => Amount = amount;
        public Money Add(Money summand) => new Money(Amount + summand.Amount);
        public Money Subract(Money subtrahend) => new Money(Amount - subtrahend.Amount);

        public static Money operator +(Money sum1, Money sum2) => sum1.Add(sum2);
        public static Money operator -(Money minuend, Money subtrahend) => minuend.Subract(subtrahend);
    }
}