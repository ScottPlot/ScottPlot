using ScottPlot.Renderable;
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
        void Rotate(float angle, Point center);
        void RotateReset();

        void Clear(Color color);

        Size MeasureText(string text, Font font);
        void DrawText(Point point, string text, Color color, Font font);

        void FillRectangle(Point point, Size size, Color color);
        void DrawRectangle(Point point, Size size, Color color, float width);
        void DrawLines(Point[] points, Color color, float width, bool rounded = true);
        void DrawLine(Point pt1, Point pt2, Color color, float width, bool rounded = true);
    }
}
