namespace ScottPlot
{
    public class PixelRect
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public PixelRect(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Pixel TopLeft => new(X, Y);
        public Pixel TopRight => new(X, Y + Width);
        public Pixel BottomLeft => new(X, Y + Height);
        public Pixel BottomRight => new(X + Width, Y + Height);
    }
}
