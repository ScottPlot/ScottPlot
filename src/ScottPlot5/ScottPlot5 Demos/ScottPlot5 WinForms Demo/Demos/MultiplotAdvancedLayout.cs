using ScottPlot;
using ScottPlot.MultiplotLayouts;

namespace WinForms_Demo.Demos;

public partial class MultiplotAdvancedLayout : Form, IDemoWindow
{
    public string Title => "Multiplot with Advanced Layout";

    public string Description => "Custom multi-plot layouts may be achieved " +
        "by assigning fractional rectangle dimensions to each subplot";

    public MultiplotAdvancedLayout()
    {
        InitializeComponent();

        // setup a multiplot with 3 subplots
        formsPlot1.Multiplot.AddPlots(3);

        // add sample data to each subplot
        for (int i = 0; i < formsPlot1.Multiplot.Subplots.Count; i++)
        {
            double[] ys = ScottPlot.Generate.Sin(oscillations: i + 1);
            formsPlot1.Multiplot.GetPlot(i).Add.Signal(ys);
        }

        // use a fixed layout to ensure all plots remain aligned
        PixelPadding padding = new(50, 20, 40, 20);
        foreach (Plot plot in formsPlot1.Multiplot.GetPlots())
            plot.Layout.Fixed(padding);

        // wire button clicks to layout changes
        btnRows.Click += (s, e) => SetupRows();
        btnColumns.Click += (s, e) => SetupColumns();
        btnGrid.Click += (s, e) => SetupGrid();
        btnMultiColumnSpan.Click += (s, e) => SetupMultiColumnSpan();
        btnPixelSizing.Click += (s, e) => SetupPixelSizing();
    }

    void SetupRows()
    {
        formsPlot1.Multiplot.Layout = new Rows();
        formsPlot1.Refresh();
    }

    void SetupColumns()
    {
        formsPlot1.Multiplot.Layout = new Columns();
        formsPlot1.Refresh();
    }

    void SetupGrid()
    {
        formsPlot1.Multiplot.Layout = new Grid(2, 2);
        formsPlot1.Refresh();
    }

    void SetupMultiColumnSpan()
    {
        CustomGrid customGrid = new();
        customGrid.Set(formsPlot1.Multiplot.GetPlot(0), new GridCell(0, 0, 2, 1));
        customGrid.Set(formsPlot1.Multiplot.GetPlot(1), new GridCell(1, 0, 2, 2));
        customGrid.Set(formsPlot1.Multiplot.GetPlot(2), new GridCell(1, 1, 2, 2));

        formsPlot1.Multiplot.Layout = customGrid;

        formsPlot1.Refresh();
    }

    void SetupPixelSizing()
    {
        formsPlot1.Multiplot.Layout = new FixedTopRowLayout(100);
        formsPlot1.Refresh();
    }

    /// <summary>
    /// Stack 3 plots in rows but force the center row to have a fixed pixel height
    /// </summary>
    class FixedTopRowLayout(int middlePlotHeight = 100) : IMultiplotLayout
    {
        int MiddlePlotHeight = middlePlotHeight;

        public PixelRect[] GetSubplotRectangles(SubplotCollection subplots, PixelRect figureRect)
        {
            PixelRect[] rectangles = new PixelRect[subplots.Count];

            PixelSize middlePlotSize = new(figureRect.Width, MiddlePlotHeight);
            PixelSize otherPlotSize = new(figureRect.Width, (figureRect.Height - MiddlePlotHeight) / 2);

            rectangles[0] = new PixelRect(otherPlotSize).WithDelta(0, 0);
            rectangles[1] = new PixelRect(middlePlotSize).WithDelta(0, rectangles[0].Bottom);
            rectangles[2] = new PixelRect(otherPlotSize).WithDelta(0, rectangles[1].Bottom);

            return rectangles;
        }
    }
}
