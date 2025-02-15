using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

public static class FormsPlotExtensions
{
    internal static Pixel Pixel(this MouseEventArgs e)
    {
        return new Pixel(e.X, e.Y);
    }

    public static void ProcessMouseDown(this Interactivity.UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel mousePixel = new(e.X, e.Y);

        Interactivity.IUserAction action = e.Button switch
        {
            MouseButtons.Left => new Interactivity.UserActions.LeftMouseDown(mousePixel),
            MouseButtons.Right => new Interactivity.UserActions.RightMouseDown(mousePixel),
            MouseButtons.Middle => new Interactivity.UserActions.MiddleMouseDown(mousePixel),
            _ => new Interactivity.UserActions.Unknown("mouse button", e.ToString()!),
        };

        processor.Process(action);
    }

    public static void ProcessMouseUp(this Interactivity.UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel mousePixel = new(e.X, e.Y);

        Interactivity.IUserAction action = e.Button switch
        {
            MouseButtons.Left => new Interactivity.UserActions.LeftMouseUp(mousePixel),
            MouseButtons.Right => new Interactivity.UserActions.RightMouseUp(mousePixel),
            MouseButtons.Middle => new Interactivity.UserActions.MiddleMouseUp(mousePixel),
            _ => new Interactivity.UserActions.Unknown("mouse button", e.ToString()!),
        };

        processor.Process(action);
    }

    public static void ProcessMouseMove(this Interactivity.UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel mousePixel = new(e.X, e.Y);
        Interactivity.IUserAction action = new Interactivity.UserActions.MouseMove(mousePixel);
        processor.Process(action);
    }

    public static void ProcessMouseWheel(this Interactivity.UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel mousePixel = new(e.X, e.Y);

        Interactivity.IUserAction action = e.Delta > 0
            ? new ScottPlot.Interactivity.UserActions.MouseWheelUp(mousePixel)
            : new ScottPlot.Interactivity.UserActions.MouseWheelDown(mousePixel);

        processor.Process(action);
    }

    public static void ProcessKeyDown(this Interactivity.UserInputProcessor processor, KeyEventArgs e)
    {
        Interactivity.Key key = e.GetKey();
        Interactivity.IUserAction action = new Interactivity.UserActions.KeyDown(key);
        processor.Process(action);
    }

    public static void ProcessKeyDown(this Interactivity.UserInputProcessor processor, PreviewKeyDownEventArgs e)
    {
        Interactivity.Key key = e.GetKey();
        Interactivity.IUserAction action = new Interactivity.UserActions.KeyDown(key);
        processor.Process(action);
    }

    public static void ProcessKeyUp(this Interactivity.UserInputProcessor processor, KeyEventArgs e)
    {
        Interactivity.Key key = e.GetKey();
        Interactivity.IUserAction action = new Interactivity.UserActions.KeyUp(key);
        processor.Process(action);
    }

    public static void ProcessLostFocus(this Interactivity.UserInputProcessor processor)
    {
        processor.ProcessLostFocus();
    }

    internal static Interactivity.Key GetKey(this KeyEventArgs e)
    {
        return GetKey(e.KeyCode);
    }

    internal static Interactivity.Key GetKey(this PreviewKeyDownEventArgs e)
    {
        return GetKey(e.KeyCode);
    }

    public static Interactivity.Key GetKey(this Keys keys)
    {
        // strip modifiers
        Keys keyCode = keys & ~Keys.Modifiers;

        Interactivity.Key key = keyCode switch
        {
            Keys.Alt => Interactivity.StandardKeys.Alt,
            Keys.Menu => Interactivity.StandardKeys.Alt,
            Keys.Shift => Interactivity.StandardKeys.Shift,
            Keys.ShiftKey => Interactivity.StandardKeys.Shift,
            Keys.LShiftKey => Interactivity.StandardKeys.Shift,
            Keys.RShiftKey => Interactivity.StandardKeys.Shift,
            Keys.Control => Interactivity.StandardKeys.Control,
            Keys.ControlKey => Interactivity.StandardKeys.Control,
            Keys.Down => Interactivity.StandardKeys.Down,
            Keys.Up => Interactivity.StandardKeys.Up,
            Keys.Left => Interactivity.StandardKeys.Left,
            Keys.Right => Interactivity.StandardKeys.Right,
            _ => Interactivity.StandardKeys.Unknown,
        };

        if (key != Interactivity.StandardKeys.Unknown)
            return key;

        string keyName = keyCode.ToString();
        return (keyName.Length == 1)
            ? new Interactivity.Key(keyName)
            : new Interactivity.Key($"Unknown modifier key {keyName}");
    }

    internal static Bitmap GetBitmap(this Plot plot, int width, int height)
    {
        byte[] bytes = plot.GetImage(width, height).GetImageBytes();
        using MemoryStream ms = new(bytes);
        System.Drawing.Bitmap bmp = new(ms);
        return bmp;
    }

    public static System.Drawing.Bitmap GetBitmap(this Image img)
    {
        byte[] bytes = img.GetImageBytes(ImageFormat.Bmp);
        MemoryStream ms = new(bytes);
        return new Bitmap(ms);
    }
}
