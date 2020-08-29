using System;

namespace ScottPlot.Renderer
{
    public interface IRenderer : IDisposable
    {
        float Width { get; }
        float Height { get; }

        void AntiAlias(bool antiAlias);

        void Clip(Point point, Size size);
        void ClipReset();

        void Clear(Color color);

        Size MeasureText(string text, string fontName, float fontSize);
        void DrawText(Point point, string text, Color color, string fontName, float fontSize);

        void FillRectangle(Point point, Size size, Color color);
        void DrawRectangle(Point point, Size size, Color color, float width);
        void DrawLines(System.Drawing.PointF[] points, Color color, float width);
    }
}
