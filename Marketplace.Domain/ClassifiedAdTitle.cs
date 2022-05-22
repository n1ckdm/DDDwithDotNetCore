namespace Marketplace.Domain
{
    public record ClassifiedAdTitle
    {
        public static ClassifiedAdTitle FromString(string title) => new ClassifiedAdTitle(title);
        public string Value { get; }

        public ClassifiedAdTitle(string title)
        {
            if (title.Length > 100)
                throw new ArgumentOutOfRangeException("Title cannot be longer thn 100 characters", nameof(title));
            
            Value = title;
        }

        public static implicit operator string(ClassifiedAdTitle self) => self.Value;
        public override string ToString() => Value.ToString();
    }
}