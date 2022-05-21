using System;

namespace Marketplace.Domain
{
    public record ClassifiedAdText
    {
        public static ClassifiedAdText FromString(string text) => new ClassifiedAdText(text);
        public string Value { get; }

        internal ClassifiedAdText(string text) => Value = text;

        public static implicit operator string(ClassifiedAdText self) => self.Value;
    }
}