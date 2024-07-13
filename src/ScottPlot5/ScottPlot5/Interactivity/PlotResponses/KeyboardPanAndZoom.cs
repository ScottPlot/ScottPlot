namespace ScottPlot.Interactivity.PlotResponses;

public class KeyboardPanAndZoom : IPlotResponse
{
    public Key PanLeftKey { get; set; } = StandardKeys.Left;
    public Key PanRightKey { get; set; } = StandardKeys.Right;
    public Key PanDownKey { get; set; } = StandardKeys.Down;
    public Key PanUpKey { get; set; } = StandardKeys.Up;

    /// <summary>
    /// When this key is held, pan actions will zoom instead
    /// </summary>
    public Key ZoomModifierKey { get; set; } = StandardKeys.Control;

    /// <summary>
    /// When this key is held, panning will occur in larger steps
    /// </summary>
    public Key LargeStepKey { get; set; } = StandardKeys.Shift;

    /// <summary>
    /// When this key is held, panning will occur in single pixel steps
    /// </summary>
    public Key FineStepKey { get; set; } = StandardKeys.Alt;

    public float StepDistance { get; set; } = 20;
    public float LargeStepDistance { get; set; } = 100;
    public float FineStepDistance { get; set; } = 1;

    public double DeltaZoomIn { get; set; } = 0.85f;
    public double DeltaZoomOut { get; set; } = 1.15f;

    public PlotResponseResult Execute(Plot plot, IUserAction userInput, KeyState keys)
    {
        if (userInput is UserActions.KeyDown keyDown)
        {
            if (keys.IsPressed(ZoomModifierKey))
            {
                if (keyDown.Key == PanLeftKey) return ApplyZoom(plot, DeltaZoomIn, 1);
                else if (keyDown.Key == PanRightKey) return ApplyZoom(plot, DeltaZoomOut, 1);
                else if (keyDown.Key == PanDownKey) return ApplyZoom(plot, 1, DeltaZoomIn);
                else if (keyDown.Key == PanUpKey) return ApplyZoom(plot, 1, DeltaZoomOut);
                else return PlotResponseResult.NoActionTaken;
            }
            else
            {
                float delta = StepDistance;
                if (keys.IsPressed(LargeStepKey)) delta = LargeStepDistance;
                if (keys.IsPressed(FineStepKey)) delta = FineStepDistance;

                if (keyDown.Key == PanLeftKey) return ApplyPan(plot, -delta, 0);
                else if (keyDown.Key == PanRightKey) return ApplyPan(plot, delta, 0);
                else if (keyDown.Key == PanDownKey) return ApplyPan(plot, 0, -delta);
                else if (keyDown.Key == PanUpKey) return ApplyPan(plot, 0, delta);
                else return PlotResponseResult.NoActionTaken;
            }
        }

        return PlotResponseResult.NoActionTaken;
    }

    private PlotResponseResult ApplyPan(Plot plot, float dX, float dY)
    {
        PixelOffset pxOffset = new(dX, dY);
        plot.Axes.Pan(pxOffset);

        return new PlotResponseResult()
        {
            Summary = $"Applied pan X={dX}, y={dY}",
            RefreshRequired = true,
        };
    }

    private PlotResponseResult ApplyZoom(Plot plot, double dX, double dY)
    {
        plot.Axes.Zoom(dX, dY);

        return new PlotResponseResult()
        {
            Summary = $"Applied zoom X={dX}, y={dY}",
            RefreshRequired = true,
        };
    }
}
