using System;

namespace Marketplace.Domain
{
    public record Price : Money
    {
        public Price(decimal amount, string currencyCode, ICurrencyLookup currencyLookup) : base(amount, currencyCode, currencyLookup)
        {
            if (amount < 0)
                throw new ArgumentException("Price cannot be nagative", nameof(amount));
        }

        public new static Price FromDecimal(
            decimal amount,
            string currency,
            ICurrencyLookup currencyLookup) => new Price(amount, currency, currencyLookup);
        public new static Price FromString(
            string amount,
            string currency,
            ICurrencyLookup currencyLookup) => new Price(decimal.Parse(amount), currency, currencyLookup);
    }
}