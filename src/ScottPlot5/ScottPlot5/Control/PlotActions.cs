namespace ScottPlot.Control;

/// <summary>
/// This object holds actions which manipulate the plot.
/// To customize plot manipulation behavior, replace these delegates with custom ones.
/// </summary>
public class PlotActions
{
    public Action<IPlotControl, Pixel, LockedAxes> ZoomIn = delegate { };
    public Action<IPlotControl, Pixel, LockedAxes> ZoomOut = delegate { };
    public Action<IPlotControl> PanUp = delegate { };
    public Action<IPlotControl> PanDown = delegate { };
    public Action<IPlotControl> PanLeft = delegate { };
    public Action<IPlotControl> PanRight = delegate { };
    public Action<IPlotControl, MouseDrag, LockedAxes> DragPan = delegate { };
    public Action<IPlotControl, MouseDrag, LockedAxes> DragZoom = delegate { };
    public Action<IPlotControl, MouseDrag, LockedAxes> DragZoomRectangle = delegate { };
    public Action<IPlotControl> ZoomRectangleClear = delegate { };
    public Action<IPlotControl> ZoomRectangleApply = delegate { };
    public Action<IPlotControl> ToggleBenchmark = delegate { };
    public Action<IPlotControl, Pixel> AutoScale = delegate { };
    public Action<IPlotControl, Pixel> ShowContextMenu = delegate { };

    public static PlotActions Standard()
    {
        return new PlotActions()
        {
            ZoomIn = StandardActions.ZoomIn,
            ZoomOut = StandardActions.ZoomOut,
            PanUp = StandardActions.PanUp,
            PanDown = StandardActions.PanDown,
            PanLeft = StandardActions.PanLeft,
            PanRight = StandardActions.PanRight,
            DragPan = StandardActions.DragPan,
            DragZoom = StandardActions.DragZoom,
            DragZoomRectangle = StandardActions.DragZoomRectangle,
            ZoomRectangleClear = StandardActions.ZoomRectangleClear,
            ZoomRectangleApply = StandardActions.ZoomRectangleApply,
            ToggleBenchmark = StandardActions.ToggleBenchmark,
            AutoScale = StandardActions.AutoScale,
            ShowContextMenu = StandardActions.ShowContextMenu,
        };
    }

    public static PlotActions NonInteractive()
    {
        return new PlotActions()
        {
            ZoomIn = delegate { },
            ZoomOut = delegate { },
            PanUp = delegate { },
            PanDown = delegate { },
            PanLeft = delegate { },
            PanRight = delegate { },
            DragPan = delegate { },
            DragZoom = delegate { },
            DragZoomRectangle = delegate { },
            ZoomRectangleClear = delegate { },
            ZoomRectangleApply = delegate { },
            ToggleBenchmark = delegate { },
            AutoScale = delegate { },
            ShowContextMenu = delegate { },
        };
    }
}
