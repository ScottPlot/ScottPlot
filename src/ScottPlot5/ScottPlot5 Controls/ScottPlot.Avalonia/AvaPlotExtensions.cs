using Avalonia;
using Avalonia.Input;
using ScottPlot.Interactivity;
using System;
using AvaKey = Avalonia.Input.Key;
using AvaCursor = Avalonia.Input.Cursor;

namespace ScottPlot.Avalonia;

internal static class AvaPlotExtensions
{
    internal static Pixel ToPixel(this PointerEventArgs e, Visual visual)
    {
        float x = (float)e.GetPosition(visual).X;
        float y = (float)e.GetPosition(visual).Y;
        return new Pixel(x, y);
    }

    internal static void ProcessMouseDown(this UserInputProcessor processor, Pixel pixel, PointerUpdateKind kind)
    {
        Interactivity.IUserAction action = kind switch
        {
            PointerUpdateKind.LeftButtonPressed => new Interactivity.UserActions.LeftMouseDown(pixel),
            PointerUpdateKind.MiddleButtonPressed => new Interactivity.UserActions.MiddleMouseDown(pixel),
            PointerUpdateKind.RightButtonPressed => new Interactivity.UserActions.RightMouseDown(pixel),
            _ => new Interactivity.UserActions.Unknown("mouse down", kind.ToString()),
        };

        processor.Process(action);
    }

    internal static void ProcessMouseUp(this UserInputProcessor processor, Pixel pixel, PointerUpdateKind kind)
    {
        Interactivity.IUserAction action = kind switch
        {
            PointerUpdateKind.LeftButtonReleased => new Interactivity.UserActions.LeftMouseUp(pixel),
            PointerUpdateKind.MiddleButtonReleased => new Interactivity.UserActions.MiddleMouseUp(pixel),
            PointerUpdateKind.RightButtonReleased => new Interactivity.UserActions.RightMouseUp(pixel),
            _ => new Interactivity.UserActions.Unknown("mouse up", kind.ToString()),
        };

        processor.Process(action);
    }

    internal static void ProcessMouseMove(this UserInputProcessor processor, Pixel pixel)
    {
        processor.Process(new Interactivity.UserActions.MouseMove(pixel));
    }

    internal static void ProcessMouseWheel(this UserInputProcessor processor, Pixel pixel, double delta)
    {
        Interactivity.IUserAction action = delta > 0
            ? new Interactivity.UserActions.MouseWheelUp(pixel)
            : new Interactivity.UserActions.MouseWheelDown(pixel);

        processor.Process(action);
    }

    internal static void ProcessKeyDown(this UserInputProcessor processor, KeyEventArgs e)
    {
        Interactivity.Key key = GetKey(e.Key);
        IUserAction action = new Interactivity.UserActions.KeyDown(key);
        processor.Process(action);
    }

    internal static void ProcessKeyUp(this UserInputProcessor processor, KeyEventArgs e)
    {
        Interactivity.Key key = GetKey(e.Key);
        IUserAction action = new Interactivity.UserActions.KeyUp(key);
        processor.Process(action);
    }

    public static Interactivity.Key GetKey(AvaKey avaKey)
    {
        return avaKey switch
        {
            AvaKey.LeftAlt => StandardKeys.Alt,
            AvaKey.RightAlt => StandardKeys.Alt,
            AvaKey.LeftShift => StandardKeys.Shift,
            AvaKey.RightShift => StandardKeys.Shift,
            AvaKey.LeftCtrl => StandardKeys.Control,
            AvaKey.RightCtrl => StandardKeys.Control,
            _ => new Interactivity.Key(avaKey.ToString()),
        };
    }

    public static AvaCursor GetCursor(this Cursor cursor)
    {
        return cursor switch
        {
            Cursor.Arrow => new(StandardCursorType.Arrow),
            Cursor.No => new(StandardCursorType.No),
            Cursor.Wait => new(StandardCursorType.Wait),
            Cursor.Hand => new(StandardCursorType.Hand),
            Cursor.Cross => new(StandardCursorType.Cross),
            Cursor.SizeAll => new(StandardCursorType.SizeAll),
            Cursor.SizeNorthSouth => new(StandardCursorType.SizeNorthSouth),
            Cursor.SizeWestEast => new(StandardCursorType.SizeWestEast),
            _ => throw new NotImplementedException(cursor.ToString()),
        };
    }
}
