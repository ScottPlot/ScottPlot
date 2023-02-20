namespace ScottPlot.Control;

public struct LinkedPlotControl
{
    public IPlotControl PlotControl;
    public bool LinkHorizontal;
    public bool LinkVertical;
    public bool LinkLayout;

    public LinkedPlotControl(IPlotControl plotControl, bool linkHorizontal, bool linkVertical, bool linkLayout)
    {
        PlotControl = plotControl;
        LinkVertical = linkVertical;
        LinkHorizontal = linkHorizontal;
        LinkLayout = linkLayout;
    }
};
