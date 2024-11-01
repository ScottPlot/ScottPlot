using SkiaSharp;
using SkiaSharp.Views.Maui;
using ScottPlot.Interactivity;

namespace ScottPlot.Maui;

internal static class MauiPlotExtensions
{
    internal static Pixel ToPixel(this PanUpdatedEventArgs e) => new Pixel(e.TotalX, e.TotalY);

    internal static Pixel ToPixel(this Point e) => new Pixel(e.X, e.Y);

    internal static Pixel ToPixelScaled(this Point e, float width, float height) => new Pixel(e.X * width, e.Y * height);

    internal static Pixel ToPixel(this SKPoint e) => new Pixel(e.X, e.Y);

    internal static void ProcessPanUpdated(this UserInputProcessor processor, MauiPlot plot, PanUpdatedEventArgs e)
    {
        IUserAction action = e.StatusType switch
        {
            GestureStatus.Started => new Interactivity.UserActions.LeftMouseDown(Pixel.Zero),
            GestureStatus.Running => new Interactivity.UserActions.MouseMove(e.ToPixel()),
            GestureStatus.Completed => new Interactivity.UserActions.LeftMouseUp(plot.LastPixel),
            _ => new Interactivity.UserActions.Unknown(),
        };
        plot.LastPixel = e.ToPixel();

        processor.Process(action);
    }

    internal static void ProcessPinchUpdated(this UserInputProcessor processor, MauiPlot plot, PinchGestureUpdatedEventArgs e, float width, float height)
    {
        if (e.Status == GestureStatus.Running)
        {
            Pixel pixel = e.ScaleOrigin.ToPixelScaled(width, height);

            IUserAction action = e.Scale > 1
                ? new ScottPlot.Interactivity.UserActions.MouseWheelUp(pixel)
                : new ScottPlot.Interactivity.UserActions.MouseWheelDown(pixel);

            processor.Process(action);
        }
    }

    internal static void ProcessMouseDown(this UserInputProcessor processor, MauiPlot plot, SKTouchEventArgs e)
    {
        Pixel pixel = e.Location.ToPixel();

        IUserAction action = e.MouseButton switch
        {
            SKMouseButton.Left => new Interactivity.UserActions.LeftMouseDown(pixel),
            SKMouseButton.Middle => new Interactivity.UserActions.MiddleMouseDown(pixel),
            SKMouseButton.Right => new Interactivity.UserActions.RightMouseDown(pixel),
            _ => new Interactivity.UserActions.Unknown()
        };
        processor.Process(action);
    }

    internal static void ProcessMouseUp(this UserInputProcessor processor, MauiPlot plot, SKTouchEventArgs e)
    {
        Pixel pixel = e.Location.ToPixel();

        IUserAction action = e.MouseButton switch
        {
            SKMouseButton.Left => new Interactivity.UserActions.LeftMouseUp(pixel),
            SKMouseButton.Middle => new Interactivity.UserActions.MiddleMouseUp(pixel),
            SKMouseButton.Right => new Interactivity.UserActions.RightMouseUp(pixel),
            _ => new Interactivity.UserActions.Unknown()
        };
        processor.Process(action);
    }

    internal static void ProcessMouseMove(this UserInputProcessor processor, MauiPlot plot, SKTouchEventArgs e)
    {
        Pixel pixel = e.Location.ToPixel();

        IUserAction action = new Interactivity.UserActions.MouseMove(pixel);
        processor.Process(action);
    }

    internal static void ProcessWheelChanged(this UserInputProcessor processor, MauiPlot plot, SKTouchEventArgs e)
    {
        Pixel pixel = e.Location.ToPixel();

        IUserAction action = e.WheelDelta > 0
          ? new ScottPlot.Interactivity.UserActions.MouseWheelUp(pixel)
          : new ScottPlot.Interactivity.UserActions.MouseWheelDown(pixel);

        processor.Process(action);
    }
}
