using Microsoft.Maui.Platform;
using ScottPlot.Control;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Diagnostics;

namespace ScottPlot.Maui;

public class MauiPlot : SKCanvasView, IPlotControl
{
    public Plot Plot { get; internal set; } = new();
    public SkiaSharp.GRContext? GRContext => null;
    public IPlotInteraction Interaction { get; set; }
    public IPlotMenu? Menu { get; set; }
    public Interactivity.UserInputProcessor UserInputProcessor { get; }
    public float DisplayScale { get; set; } = 1;

    public MauiPlot()
    {
        DisplayScale = DetectDisplayScale();
        Plot = new Plot() { PlotControl = this };
        Interaction = new Interaction(this);
        UserInputProcessor = new(Plot);
        Menu = new MauiPlotMenu(this);

        PanGestureRecognizer panGestureRecognizer = new PanGestureRecognizer();
        PointerGestureRecognizer pointerGestureRecognizer = new PointerGestureRecognizer();
        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer()
        { Buttons = ButtonsMask.Secondary };
        TapGestureRecognizer twoTapGestureRecognizer = new TapGestureRecognizer()
        { Buttons = ButtonsMask.Primary, NumberOfTapsRequired = 2 };
        PinchGestureRecognizer pinchGestureRecognizer = new PinchGestureRecognizer();

        panGestureRecognizer.PanUpdated += (s, e) =>
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    Interaction.MouseDown(Pixel.Zero, MouseButton.Left);
                    break;

                case GestureStatus.Running:
                    Interaction.OnMouseMove(new Pixel(e.TotalX, e.TotalY));
                    break;

                case GestureStatus.Completed:
                    Interaction.MouseUp(new Pixel(e.TotalX, e.TotalY), MouseButton.Left);
                    break;
            }
        };

        pinchGestureRecognizer.PinchUpdated += (s, e) =>
        {
            if (e.Status == GestureStatus.Running && e.Scale != 1)
                Interaction.MouseWheelVertical(new Pixel(Width / 2, Height / 2), e.Scale > 1 ? 1 : -1);
        };

        pointerGestureRecognizer.PointerMoved += (s, e) =>
        {
            var pixel = e.GetPosition(null)?.ToPixel() ?? Pixel.NaN;
            Interaction.OnMouseMove(pixel);
        };

        pointerGestureRecognizer.PointerPressed += (s, e) =>
        {
            var pixel = e.GetPosition(null)?.ToPixel() ?? Pixel.NaN;
            Interaction.MouseDown(pixel, MouseButton.Left);
        };

        pointerGestureRecognizer.PointerReleased += (s, e) =>
        {
            var pixel = e.GetPosition(null)?.ToPixel() ?? Pixel.NaN;
            Interaction.MouseUp(pixel, MouseButton.Left);
        };

        tapGestureRecognizer.Tapped += (s, e) =>
        {
            var pixel = e.GetPosition(null)?.ToPixel() ?? Pixel.NaN;
            ShowContextMenu(pixel);
        };

        twoTapGestureRecognizer.Tapped += (s, e) =>
        {
            var pixel = e.GetPosition(this)?.ToPixel() ?? Pixel.NaN;
            Interaction.MouseWheelVertical(pixel, 1);
        };

        GestureRecognizers.Add(pointerGestureRecognizer);
        GestureRecognizers.Add(tapGestureRecognizer);
        GestureRecognizers.Add(twoTapGestureRecognizer);
        GestureRecognizers.Add(pinchGestureRecognizer);
        GestureRecognizers.Add(panGestureRecognizer);
    }

    public void Reset()
    {
        Reset(new Plot());
    }

    public void Reset(Plot plot)
    {
        Plot = plot;
    }

    public void Refresh()
    {
        InvalidateSurface();
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu?.ShowContextMenu(position);
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        var _info = e.Info;

        e.Surface.Canvas.Clear();
        Plot.Render(e.Surface.Canvas, _info.Width, _info.Height);
    }

    public float DetectDisplayScale()
    {
        if (Parent is VisualElement parent)
        {
            Plot.ScaleFactor = parent.Scale;
            DisplayScale = (float)parent.Scale;
        }

        return DisplayScale;
    }
}
