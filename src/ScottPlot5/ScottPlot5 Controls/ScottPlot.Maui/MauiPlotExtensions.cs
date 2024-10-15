namespace ScottPlot.Maui;

internal static class MauiPlotExtensions
{
    internal static Pixel ToPixel(this PointerEventArgs e, MauiPlot? plot)
    {
        Point? position = e.GetPosition(plot);

        if (position is null) return Pixel.NaN;
        if (plot is null) return new(position.Value.X, position.Value.Y);
        return new(position.Value.X * plot.DisplayScale, position.Value.Y * plot.DisplayScale);
    }

    internal static Pixel ToPixel(this TappedEventArgs e, MauiPlot? plot)
    {
        Point? position = e.GetPosition(plot);

        if (position is null) return Pixel.NaN;
        if (plot is null) return new(position.Value.X, position.Value.Y);
        return new(position.Value.X * plot.DisplayScale, position.Value.Y * plot.DisplayScale);
    }

    internal static Pixel ToPixel(this PanUpdatedEventArgs e)
    {
        return new Pixel(e.TotalX, e.TotalY);
    }

    internal static void ProcessPanUpdated(this Interactivity.UserInputProcessor processor, MauiPlot plot, PanUpdatedEventArgs e)
    {
        Interactivity.IUserAction action = e.StatusType switch
        {
            GestureStatus.Started => new Interactivity.UserActions.LeftMouseDown(Pixel.Zero),
            GestureStatus.Running => new Interactivity.UserActions.MouseMove(e.ToPixel()),
            GestureStatus.Completed => new Interactivity.UserActions.LeftMouseUp(plot.LastPixel), //When StatusType == Completed, e.ToPixel returns (0,0)
            _ => new Interactivity.UserActions.Unknown(),
        };
        plot.LastPixel = e.ToPixel();

        processor.Process(action);
    }

    internal static void ProcessPinchUpdated(this Interactivity.UserInputProcessor processor, MauiPlot plot, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Running && e.Scale != 1.0)
        {
            Interactivity.IUserAction action = e.Scale > 1
                ? new ScottPlot.Interactivity.UserActions.MouseWheelUp(new Pixel(plot.Width / 2, plot.Height / 2))
                : new ScottPlot.Interactivity.UserActions.MouseWheelDown(new Pixel(plot.Width / 2, plot.Height / 2));

            processor.Process(action);
        }
    }

    internal static void ProcessPointerPressed(this Interactivity.UserInputProcessor processor, MauiPlot plot, PointerEventArgs e)
    {
        Pixel pixel = e.ToPixel(plot);

        //ProcessKeyModifiersPressed(processor, plot, e);
        Interactivity.IUserAction action = new Interactivity.UserActions.LeftMouseDown(pixel);
        processor.Process(action);
    }

    internal static void ProcessPointerMoved(this Interactivity.UserInputProcessor processor, MauiPlot plot, PointerEventArgs e)
    {
        Pixel pixel = e.ToPixel(plot);

        Interactivity.IUserAction action = new Interactivity.UserActions.MouseMove(pixel);
        processor.Process(action);
    }

    internal static void ProcessPointerReleased(this Interactivity.UserInputProcessor processor, MauiPlot plot, PointerEventArgs e)
    {
        Pixel pixel = e.ToPixel(plot);

        Interactivity.IUserAction action = new Interactivity.UserActions.LeftMouseUp(pixel);
        processor.Process(action);
    }

    internal static void ProcessContext(this Interactivity.UserInputProcessor processor, MauiPlot plot, TappedEventArgs e)
    {
        Pixel pixel = e.ToPixel(plot);

        Interactivity.IUserAction action = new Interactivity.UserActions.RightMouseDown(pixel);
        processor.Process(action);
        action = new Interactivity.UserActions.RightMouseUp(pixel);
        processor.Process(action);
    }

    internal static void ProcessZoomAll(this Interactivity.UserInputProcessor processor, MauiPlot plot, TappedEventArgs e)
    {
        Pixel pixel = e.ToPixel(plot);

        Interactivity.IUserAction action = new Interactivity.UserActions.MouseWheelUp(pixel);
        processor.Process(action);
    }
}

