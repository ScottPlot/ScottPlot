using Eto.Forms;

namespace ScottPlot.Eto;

internal static class EtoPlotExtensions
{
    public static void ProcessMouseDown(this Interactivity.UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel pixel = e.Pixel();

        Interactivity.IUserAction action = e.Buttons switch
        {
            MouseButtons.Primary => new Interactivity.UserActions.LeftMouseDown(pixel),
            MouseButtons.Middle => new Interactivity.UserActions.MiddleMouseDown(pixel),
            MouseButtons.Alternate => new Interactivity.UserActions.RightMouseDown(pixel),
            _ => new Interactivity.UserActions.Unknown(e.Buttons.ToString(), "pressed"),
        };

        processor.Process(action);
    }

    public static void ProcessMouseUp(this Interactivity.UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel pixel = e.Pixel();

        Interactivity.IUserAction action = e.Buttons switch
        {
            MouseButtons.Primary => new Interactivity.UserActions.LeftMouseUp(pixel),
            MouseButtons.Middle => new Interactivity.UserActions.MiddleMouseUp(pixel),
            MouseButtons.Alternate => new Interactivity.UserActions.RightMouseUp(pixel),
            _ => new Interactivity.UserActions.Unknown(e.Buttons.ToString(), "released"),
        };

        processor.Process(action);
    }

    public static void ProcessMouseMove(this Interactivity.UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel pixel = e.Pixel();
        Interactivity.IUserAction action = new Interactivity.UserActions.MouseMove(pixel);
        processor.Process(action);
    }

    public static void ProcessMouseWheel(this Interactivity.UserInputProcessor processor, MouseEventArgs e)
    {
        Pixel pixel = e.Pixel();

        Interactivity.IUserAction action = e.Delta.Height > 0
            ? new Interactivity.UserActions.MouseWheelUp(pixel)
            : new Interactivity.UserActions.MouseWheelDown(pixel);

        processor.Process(action);
    }

    public static void ProcessKeyDown(this Interactivity.UserInputProcessor processor, KeyEventArgs e)
    {
        Interactivity.Key key = e.ToKey();
        Interactivity.IUserAction action = new Interactivity.UserActions.KeyDown(key);
        processor.Process(action);
    }

    public static void ProcessKeyUp(this Interactivity.UserInputProcessor processor, KeyEventArgs e)
    {
        Interactivity.Key key = e.ToKey();
        Interactivity.IUserAction action = new Interactivity.UserActions.KeyUp(key);
        processor.Process(action);
    }

    public static Interactivity.Key ToKey(this KeyEventArgs e)
    {
        return e.Key switch
        {
            Keys.LeftControl => Interactivity.StandardKeys.Control,
            Keys.RightControl => Interactivity.StandardKeys.Control,
            Keys.LeftAlt => Interactivity.StandardKeys.Alt,
            Keys.RightAlt => Interactivity.StandardKeys.Alt,
            Keys.LeftShift => Interactivity.StandardKeys.Shift,
            Keys.RightShift => Interactivity.StandardKeys.Shift,
            _ => new Interactivity.Key(e.ToString() ?? string.Empty),
        };
    }

    internal static Pixel Pixel(this MouseEventArgs e)
    {
        double x = e.Location.X;
        double y = e.Location.Y;
        return new Pixel((float)x, (float)y);
    }
}
