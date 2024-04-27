using ScottPlot.Control;

namespace ScottPlot;

public interface IPlotInteraction
{
    IPlotControl PlotControl { get; }

    /// <summary>
    /// Disable all mouse interactivity
    /// </summary>
    void Disable();

    /// <summary>
    /// Enable mouse interactivity using the default mouse actions
    /// </summary>
    void Enable();

    /// <summary>
    /// Enable mouse interactivity using custom mouse actions
    /// </summary>
    void Enable(PlotActions customActions);

    void OnMouseMove(Pixel newPosition);

    void KeyUp(Key key);

    void KeyDown(Key key);

    void MouseDown(Pixel position, MouseButton button);

    void MouseUp(Pixel position, MouseButton button);

    void DoubleClick();

    void MouseWheelVertical(Pixel pixel, float delta);

    /// <summary>
    /// If enabled, mouse interactions over a single axis will be applied to
    /// all axes with the same orientation.
    /// </summary>
    public bool ChangeOpposingAxesTogether { get; set; }
}
