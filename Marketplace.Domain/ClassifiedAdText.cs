using System;

namespace Marketplace.Domain
{
    public record ClassifiedAdText
    {
        public static ClassifiedAdText FromString(string text) => new ClassifiedAdText(text);
        private readonly string _value;

        private ClassifiedAdText(string value)
        {
            _value = value;
        }
    }
}