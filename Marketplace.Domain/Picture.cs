using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class Picture : Entity<PictureId>
    {
        internal PictureSize Size { get; set; }
        internal Uri Location { get; set; }
        internal int Order { get; set; }

        protected override void When(object @event)
        {
        }

        protected override void EnsureValidState()
        {
        }
    }

    public record PictureId
    {
        public Guid Value { get; }
        public PictureId(Guid value) => Value = value;
    }
}