namespace ScottPlot.Control;

public class Backend
{
    public InputBindings Bindings { get; set; } = InputBindings.Standard();

    public IPlotActions Actions { get; private set; }

    public PlotActionCommander ActionCommander { get; private set; }


    private readonly Plot Plot;

    private readonly KeyboardState Keyboard = new();

    private readonly MouseState Mouse = new();


    public Backend(IPlotControl control)
    {
        Plot = control.Plot;
        Actions = new PlotActions(control);
        ActionCommander = new PlotActionCommander(Actions, Bindings, Mouse, Keyboard);
    }

    public Coordinates GetMouseCoordinates(Axes.IAxis? xAxis = null, Axes.IAxis? yAxis = null)
    {
        return Plot.GetCoordinate(Mouse.LastPosition, xAxis, yAxis);
    }

    public void MouseDown(Pixel position, MouseButton button)
    {
        // TODO: invoke actions chooser?
        Mouse.Down(position, button, Plot.GetAxisLimits());
    }

    public void KeyDown(Key key)
    {
        // TODO: invoke actions chooser?
        Keyboard.Down(key);
    }

    public void KeyUp(Key key)
    {
        // TODO: invoke actions chooser?
        Keyboard.Up(key);
    }

    public void MouseUp(Pixel position, MouseButton button)
    {
        ActionCommander.MouseUp(position, button);
        Mouse.Up(button);
    }

    public void MouseMove(Pixel newPosition)
    {
        Mouse.LastPosition = newPosition;
        ActionCommander.MouseMove(newPosition);
    }

    public void DoubleClick()
    {
        ActionCommander.DoubleClick();
    }

    public void MouseWheelVertical(Pixel pixel, float delta)
    {
        ActionCommander.MouseWheelVertical(pixel, delta);
    }
}
