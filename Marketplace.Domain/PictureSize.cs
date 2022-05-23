using Marketplace.Framework;

namespace Marketplace.Domain
{
    public record PictureSize
    {
        public int Width { get; internal set; }
        public int Height { get; internal set; }

        public PictureSize(int width, int height)
        {
            if (Width <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(width), "Picture width must have a positive number"
                );
            if (Height <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(width), "Picture height must have a positive number"
                );
            Width = width;
            Height = height;
        }

        internal PictureSize() {}
    }
}