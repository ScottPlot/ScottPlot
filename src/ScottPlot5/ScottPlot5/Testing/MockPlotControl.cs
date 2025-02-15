using ScottPlot.Interactivity;
using ScottPlot.Interactivity.UserActions;

namespace ScottPlot.Testing;

/// <summary>
/// A plot control that renders in-memory and has
/// functionality useful for testing interactivity.
/// </summary>
public class MockPlotControl : IPlotControl
{
    public int Width { get; set; } = 400;
    public int Height { get; set; } = 300;

    public Pixel Center => new(Width / 2, Height / 2);

    public Plot Plot { get; private set; }

    public IMultiplot Multiplot { get; set; }
    public GRContext? GRContext => null;

    public UserInputProcessor UserInputProcessor { get; }

    public IPlotMenu? Menu // TODO: mock menu
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public MockPlotControl()
    {
        Plot = new() { PlotControl = this };
        Multiplot = new Multiplot(Plot);
        UserInputProcessor = new(this) { IsEnabled = true };

        // force a render on startup so we can immediately use pixel drag actions
        Refresh();
    }

    public float DisplayScale { get; set; } = 1;
    public float DetectDisplayScale() => DisplayScale;

    public int RefreshCount { get; private set; } = 0;
    public void Refresh()
    {
        RefreshCount += 1;
        Multiplot.Render(Width, Height);
    }

    public void Reset()
    {
        Reset(new Plot() { PlotControl = this });
    }

    public void Reset(Plot plot)
    {
        Plot oldPlot = Plot;
        Plot = plot;
        Plot.PlotControl = this;
        oldPlot.Dispose();
        Multiplot.Reset(plot);
    }

    public int ContextMenuLaunchCount { get; private set; } = 0;
    public void ShowContextMenu(Pixel position) => ContextMenuLaunchCount += 1;

    public void ScrollWheelUp(Pixel pixel)
    {
        IUserAction action = new MouseWheelUp(pixel);
        UserInputProcessor.Process(action);
    }

    public void ScrollWheelDown(Pixel pixel)
    {
        IUserAction action = new MouseWheelDown(pixel);
        UserInputProcessor.Process(action);
    }

    public void PressShift() => PressKey(StandardKeys.Shift);
    public void ReleaseShift() => ReleaseKey(StandardKeys.Shift);
    public void PressCtrl() => PressKey(StandardKeys.Control);
    public void ReleaseCtrl() => ReleaseKey(StandardKeys.Control);
    public void PressAlt() => PressKey(StandardKeys.Alt);
    public void ReleaseAlt() => ReleaseKey(StandardKeys.Alt);
    public void TapRightArrow() => TapKey(StandardKeys.Right);
    public void TapLeftArrow() => TapKey(StandardKeys.Left);
    public void TapUpArrow() => TapKey(StandardKeys.Up);
    public void TapDownArrow() => TapKey(StandardKeys.Down);

    public void PressKey(Key key)
    {
        IUserAction action = new KeyDown(key);
        UserInputProcessor.Process(action);
    }

    public void ReleaseKey(Key key)
    {
        IUserAction action = new KeyUp(key);
        UserInputProcessor.Process(action);
    }

    public void TapKey(Key key)
    {
        PressKey(key);
        ReleaseKey(key);
    }

    public void MoveMouse(Pixel px)
    {
        IUserAction action = new MouseMove(px);
        UserInputProcessor.Process(action);
    }

    public void LeftMouseDown(Pixel px)
    {
        IUserAction action = new LeftMouseDown(px);
        UserInputProcessor.Process(action);
    }

    public void LeftMouseUp(Pixel px)
    {
        IUserAction action = new LeftMouseUp(px);
        UserInputProcessor.Process(action);
    }

    public void LeftClick(Pixel px)
    {
        LeftMouseDown(px);
        LeftMouseUp(px);
    }

    public void LeftClickDrag(Pixel px1, Pixel px2)
    {
        LeftMouseDown(px1);
        MoveMouse(px2);
        LeftMouseUp(px2);
    }

    public void RightMouseDown(Pixel px)
    {
        IUserAction action = new RightMouseDown(px);
        UserInputProcessor.Process(action);
    }

    public void RightMouseUp(Pixel px)
    {
        IUserAction action = new RightMouseUp(px);
        UserInputProcessor.Process(action);
    }

    public void RightClick(Pixel px)
    {
        RightMouseDown(px);
        RightMouseUp(px);
    }

    public void RightClickDrag(Pixel px1, Pixel px2)
    {
        RightMouseDown(px1);
        MoveMouse(px2);
        RightMouseDown(px2);
    }

    public void MiddleMouseDown(Pixel px)
    {
        IUserAction action = new MiddleMouseDown(px);
        UserInputProcessor.Process(action);
    }

    public void MiddleMouseUp(Pixel px)
    {
        IUserAction action = new MiddleMouseUp(px);
        UserInputProcessor.Process(action);
    }

    public void MiddleClick(Pixel px)
    {
        MiddleMouseDown(px);
        MiddleMouseUp(px);
    }

    public void MiddleClickDrag(Pixel px1, Pixel px2)
    {
        MiddleMouseDown(px1);
        MoveMouse(px2);
        MiddleMouseUp(px2);
    }
}
