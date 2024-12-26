using ScottPlot;

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
        for (int i = 0; i < formsPlot1.Multiplot.Count; i++)
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
        formsPlot1.Multiplot.Layout = new ScottPlot.MultiplotLayouts.Rows();
        formsPlot1.Refresh();
    }

    void SetupColumns()
    {
        formsPlot1.Multiplot.Layout = new ScottPlot.MultiplotLayouts.Columns();
        formsPlot1.Refresh();
    }

    void SetupGrid()
    {
        formsPlot1.Multiplot.Layout = new ScottPlot.MultiplotLayouts.Grid(2, 2);
        formsPlot1.Refresh();
    }

    void SetupMultiColumnSpan()
    {
        formsPlot1.Multiplot.SetPosition(0, new ScottPlot.SubplotPositions.GridCell(0, 0, 2, 1));
        formsPlot1.Multiplot.SetPosition(1, new ScottPlot.SubplotPositions.GridCell(1, 0, 2, 2));
        formsPlot1.Multiplot.SetPosition(2, new ScottPlot.SubplotPositions.GridCell(1, 1, 2, 2));
        formsPlot1.Refresh();
    }

    void SetupPixelSizing()
    {
        // Stack 3 plots in rows but force the center row to always be 100px high
        float middlePlotHeight = 100;
        formsPlot1.Multiplot.SetPosition(0, new CustomTopRow(middlePlotHeight));
        formsPlot1.Multiplot.SetPosition(1, new CustomMiddleRow(middlePlotHeight));
        formsPlot1.Multiplot.SetPosition(2, new CustomBottomRow(middlePlotHeight));
        formsPlot1.Refresh();
    }

    class CustomTopRow(float middlePlotHeight) : ISubplotPosition
    {
        public PixelRect GetRect(PixelRect figureRect)
        {
            return new PixelRect(
                left: figureRect.Left,
                right: figureRect.Right,
                bottom: figureRect.VerticalCenter - middlePlotHeight / 2,
                top: figureRect.Top);
        }
    }

    class CustomMiddleRow(float middlePlotHeight) : ISubplotPosition
    {
        public PixelRect GetRect(PixelRect figureRect)
        {
            return new PixelRect(
                left: figureRect.Left,
                right: figureRect.Right,
                bottom: figureRect.VerticalCenter + middlePlotHeight / 2,
                top: figureRect.VerticalCenter - middlePlotHeight / 2);
        }
    }

    class CustomBottomRow(float middlePlotHeight) : ISubplotPosition
    {
        public PixelRect GetRect(PixelRect figureRect)
        {
            return new PixelRect(
                left: figureRect.Left,
                right: figureRect.Right,
                bottom: figureRect.Bottom,
                top: figureRect.VerticalCenter + middlePlotHeight / 2);
        }
    }
}
