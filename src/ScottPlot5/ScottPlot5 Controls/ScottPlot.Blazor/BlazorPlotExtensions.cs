﻿using Microsoft.AspNetCore.Components.Web;

namespace ScottPlot.Blazor;

public static class BlazorPlotExtensions
{
    public static Pixel ToPixel(this WheelEventArgs args)
    {
        return new Pixel((float)args.OffsetX, (float)args.OffsetY);
    }

    public static Pixel ToPixel(this PointerEventArgs args)
    {
        return new Pixel((float)args.OffsetX, (float)args.OffsetY);
    }

    public static void ProcessMouseMove(this Interactivity.UserInputProcessor processor, PointerEventArgs e)
    {
        Pixel pixel = e.ToPixel();
        Interactivity.IUserAction action = new Interactivity.UserActions.MouseMove(pixel);
        processor.Process(action);
    }

    public static void ProcessMouseDown(this Interactivity.UserInputProcessor processor, PointerEventArgs e)
    {
        Pixel pixel = e.ToPixel();

        Interactivity.IUserAction action = e.Button switch
        {
            0 => new Interactivity.UserActions.LeftMouseDown(pixel),
            1 => new Interactivity.UserActions.MiddleMouseDown(pixel),
            2 => new Interactivity.UserActions.RightMouseDown(pixel),
            _ => new Interactivity.UserActions.Unknown(e.Button.ToString(), "down"),
        };

        processor.Process(action);
    }

    public static void ProcessMouseUp(this Interactivity.UserInputProcessor processor, PointerEventArgs e)
    {
        Pixel pixel = e.ToPixel();

        Interactivity.IUserAction action = e.Button switch
        {
            0 => new Interactivity.UserActions.LeftMouseUp(pixel),
            1 => new Interactivity.UserActions.MiddleMouseUp(pixel),
            2 => new Interactivity.UserActions.RightMouseUp(pixel),
            _ => new Interactivity.UserActions.Unknown(e.Button.ToString(), "up"),
        };

        processor.Process(action);
    }

    public static void ProcessMouseWheel(this Interactivity.UserInputProcessor processor, WheelEventArgs e)
    {
        Pixel pixel = e.ToPixel();
        double delta = -(float)e.DeltaY;

        Interactivity.IUserAction action = delta > 0
            ? new Interactivity.UserActions.MouseWheelUp(pixel)
            : new Interactivity.UserActions.MouseWheelDown(pixel);

        processor.Process(action);
    }

    public static void ProcessKeyDown(this Interactivity.UserInputProcessor processor, KeyboardEventArgs e)
    {
        Interactivity.Key key = e.ToKey();
        Interactivity.IUserAction action = new Interactivity.UserActions.KeyDown(key);
        processor.Process(action);
    }

    public static void ProcessKeyUp(this Interactivity.UserInputProcessor processor, KeyboardEventArgs e)
    {
        Interactivity.Key key = e.ToKey();
        Interactivity.IUserAction action = new Interactivity.UserActions.KeyUp(key);
        processor.Process(action);
    }

    public static Control.MouseButton OldToButton(this MouseEventArgs args)
    {
        return args.Button switch
        {
            0 => Control.MouseButton.Left,
            1 => Control.MouseButton.Middle,
            2 => Control.MouseButton.Right,
            _ => Control.MouseButton.Unknown,
        };
    }

    public static Control.Key OldToKey(this KeyboardEventArgs e)
    {
        return e.Key switch
        {
            "Control" => Control.Key.Ctrl,
            "Alt" => Control.Key.Alt,
            "Shift" => Control.Key.Shift,
            _ => Control.Key.Unknown,
        };
    }

    public static Interactivity.Key ToKey(this KeyboardEventArgs e)
    {
        return e.Key switch
        {
            "Control" => Interactivity.StandardKeys.Control,
            "Alt" => Interactivity.StandardKeys.Alt,
            "Shift" => Interactivity.StandardKeys.Shift,
            _ => new Interactivity.Key(e.Key),
        };
    }
}
