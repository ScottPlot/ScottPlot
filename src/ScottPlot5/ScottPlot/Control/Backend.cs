namespace ScottPlot.Control;

/// <summary>
/// Developers porting ScottPlot to new user controls can realize all interaction
/// control and customization features by wiring control events to these functions.
/// </summary>
public class Backend
{
    public readonly Interaction Interaction;

    public Backend(IPlotControl control)
    {
        Interaction = new Interaction(control);
    }

    public void KeyDown(Key key)
    {
        Interaction.KeyDown(key);
    }

    public void KeyUp(Key key)
    {
        Interaction.KeyUp(key);
    }

    public void MouseDown(Pixel position, MouseButton button)
    {
        Interaction.MouseDown(position, button);
    }

    public void MouseUp(Pixel position, MouseButton button)
    {
        Interaction.MouseUp(position, button);
    }

    public void MouseMove(Pixel newPosition)
    {
        Interaction.MouseMove(newPosition);
    }

    public void DoubleClick()
    {
        Interaction.DoubleClick();
    }

    public void MouseWheelVertical(Pixel pixel, float delta)
    {
        Interaction.MouseWheelVertical(pixel, delta);
    }
}
