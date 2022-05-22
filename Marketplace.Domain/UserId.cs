using System;

namespace Marketplace.Domain
{
    public class UserId
    {
        public Guid Value { get; }

        public UserId(Guid value)
        {
            if (value == default)
                throw new ArgumentNullException(nameof(value), "UserId id cannot be empty");
            Value = value;
        }

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(UserId self) => self.Value;
        
        public static implicit operator UserId(string value) 
            => new UserId(Guid.Parse(value));
    }
}