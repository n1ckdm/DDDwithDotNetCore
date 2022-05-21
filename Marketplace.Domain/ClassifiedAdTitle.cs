using System;

namespace Marketplace.Domain
{
    public record ClassifiedAdTitle
    {
        public static ClassifiedAdTitle FromString(string title) => new ClassifiedAdTitle(title);
        private readonly string _value;

        private ClassifiedAdTitle(string value)
        {
            if (value.Length > 100)
                throw new ArgumentOutOfRangeException("Title cannot be longer thn 100 characters", nameof(value));
            
            _value = value;
        }

        public static implicit operator string(ClassifiedAdTitle self) => self._value;
    }
}