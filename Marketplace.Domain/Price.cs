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
    }
}