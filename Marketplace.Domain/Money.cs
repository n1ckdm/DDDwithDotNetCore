namespace Marketplace.Domain
{
    public record Money
    {
        private const string DefaultCurrency = "EUR";
        public static Money FromDecimal(
            decimal amount,
            string currency,
            ICurrencyLookup currencyLookup) => new Money(amount, currency, currencyLookup);
        public static Money FromString(
            string amount,
            string currency,
            ICurrencyLookup currencyLookup) => new Money(decimal.Parse(amount), currency, currencyLookup);
        protected Money(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
        {
            if (string.IsNullOrEmpty(currencyCode))
                throw new ArgumentNullException("Currency code must be specified");

            var currency = currencyLookup.FindCurrency(currencyCode);

            if (!currency.InUse)
                throw new ArgumentException($"Currency {currencyCode} is no valid");

            if (decimal.Round(amount, currency.DecimalPlaces) != amount)
                throw new ArgumentOutOfRangeException($"Amount cannot have more than {currency.DecimalPlaces} decimals", nameof(amount));
            
            Amount = amount;
            Currency = currency;
        }

        private Money(decimal amount, CurrencyDetails currency)
        {
            Amount = amount;
            Currency = currency;
        }
        public decimal Amount { get; }
        public CurrencyDetails Currency { get; }
        public Money Add(Money summand)
        {
            if (Currency != summand.Currency)
                throw new CurrencyMissmatchException("Cannot add amounts with different currencies");
            return new Money(Amount + summand.Amount, Currency);
        }
        public Money Subract(Money subtrahend)
        {
            if (Currency != subtrahend.Currency)
                throw new CurrencyMissmatchException("Cannot add amounts with different currencies");
            return new Money(Amount - subtrahend.Amount, Currency);
        }

        public static Money operator +(Money sum1, Money sum2) => sum1.Add(sum2);
        public static Money operator -(Money minuend, Money subtrahend) => minuend.Subract(subtrahend);
        
        public override string ToString() => $"{Currency.CurrencyCode} {Amount}";
    }

    public class CurrencyMissmatchException : Exception
    {
        public CurrencyMissmatchException(string message) : base(message) {}
    }
}