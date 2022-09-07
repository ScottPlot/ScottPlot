using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ScottPlot5_WinForms_Demo;

public partial class SkiaButton : UserControl
{
    readonly SKGLControl SKElement;

    public string Title { get; set; } = "Title";

    public string Description { get; set; } = "Description";

    public string RegularBackgroundColor { get; set; } = "#71297f";

    public string HighlightColor { get; set; } = "#9a4993";

    public string ForegroundColor { get; set; } = "#FFFFFF";

    private bool _isHighlighted = false;

    public bool IsHighlighted
    {
        get => _isHighlighted;
        set
        {
            _isHighlighted = value;
            SKElement.Invalidate();
        }
    }

    private SKColor SKBackground => SKColor.Parse(IsHighlighted ? HighlightColor : RegularBackgroundColor);

    private SKColor SKForeground => SKColor.Parse(ForegroundColor);

    public SkiaButton()
    {
        SKElement = new() { Dock = DockStyle.Fill, VSync = true };
        SKElement.PaintSurface += SKElement_PaintSurface;
        SKElement.MouseEnter += (s, e) => IsHighlighted = true;
        SKElement.MouseLeave += (s, e) => IsHighlighted = false;
        Resize += (s, e) => SKElement.Invalidate();
        Controls.Add(SKElement);
    }

    private void DrawText(string text, SKPoint pt, float maxWidth, SKSurface surface, SKPaint paint)
    {
        IEnumerable<string> words = text.Split(' ');

        List<string> lines = new();

        int lineWordCount = 0;
        while (words.Any())
        {
            lineWordCount += 1;
            string line = string.Join(" ", words.Take(lineWordCount));
            float textWidth = paint.MeasureText(line);
            if (textWidth > maxWidth)
            {
                string lastLine = string.Join(" ", words.Take(lineWordCount - 1));
                lines.Add(lastLine.Trim());
                words = words.Skip(lineWordCount - 1);
                lineWordCount = 0;
            }
            else if (words.Count() <= lineWordCount)
            {
                lines.Add(string.Join(" ", words).Trim());
                break;
            }
        }

        for (int i = 0; i < lines.Count(); i++)
        {
            SKPoint pt2 = new(pt.X, pt.Y + paint.FontSpacing * i);
            surface.Canvas.DrawText(lines[i], pt2, paint);
        }
    }

    private void SKElement_PaintSurface(object? sender, SKPaintGLSurfaceEventArgs e)
    {
        string title = "Title";
        string description = "This is an example of a really long description " +
            "created to evaluate how SkiaSharp handles multi-line wrapping";
        float width = e.Surface.Canvas.LocalClipBounds.Width;
        float height = e.Surface.Canvas.LocalClipBounds.Height;

        SKPaint paint = new()
        {
            Color = SKBackground,
            IsAntialias = true,
            TextSize = 22
        };

        e.Surface.Canvas.DrawRect(0, 0, width, height, paint);

        paint.Color = SKColors.White;
        e.Surface.Canvas.DrawText(title, new(10, 10 + paint.FontSpacing), paint);
        float descriptionOffsetY = paint.FontSpacing + 30;

        paint.TextSize = 16;
        DrawText(description, new SKPoint(10, descriptionOffsetY), width - 20, e.Surface, paint);
    }
}
