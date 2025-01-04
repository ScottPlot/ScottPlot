using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class MultiplotCollapsed : Form, IDemoWindow
{
    public string Title => "Multiplot with Draggable Subplots";

    public string Description => "Subplots may be placed very close together by setting their padding to zero. " +
        "This example uses an advanced Layout system to enable mouse drag resizing of subplots.";

    public MultiplotCollapsed()
    {
        InitializeComponent();

        // setup a multiplot with 3 subplots
        formsPlot1.Multiplot.AddPlots(3);

        // add sample price data to the first plot
        Plot pricePlot = formsPlot1.Multiplot.GetPlot(0);
        pricePlot.Axes.Right.Label.Text = "Price";
        List<OHLC> ohlcs = Generate.RandomOHLCs(50);
        var candlestick = pricePlot.Add.Candlestick(ohlcs);
        candlestick.Axes.YAxis = pricePlot.Axes.Right;
        candlestick.Sequential = true;

        // add sample RSI data to the second plot
        Plot rsiPlot = formsPlot1.Multiplot.GetPlot(1);
        rsiPlot.Axes.Right.Label.Text = "RSI";
        double[] rsiValues = Generate.RandomWalk(ohlcs.Count);
        var rsiSig = rsiPlot.Add.Signal(rsiValues);
        rsiSig.Axes.YAxis = rsiPlot.Axes.Right;
        rsiSig.LineWidth = 2;

        // add sample RSI data to the second plot
        Plot volumePlot = formsPlot1.Multiplot.GetPlot(2);
        volumePlot.Axes.Right.Label.Text = "Volume";
        double[] volumes = Generate.RandomSample(50, 10, 90);
        var bars = volumePlot.Add.Bars(volumes);
        bars.Axes.YAxis = volumePlot.Axes.Right;
        volumePlot.Axes.Margins(bottom: 0);

        // give all plots identical left and right padding to ensure alignment
        float padLeft = 20;
        float padRight = 65;

        // set padding so there is no space between the middle and adjacent plots
        pricePlot.Layout.Fixed(new PixelPadding(padLeft, padRight, bottom: 0, top: 25));
        rsiPlot.Layout.Fixed(new PixelPadding(padLeft, padRight, bottom: 0, top: 0));
        volumePlot.Layout.Fixed(new PixelPadding(padLeft, padRight, bottom: 40, top: 0));

        // disable tick generation for axes that don't need them
        pricePlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.EmptyTickGenerator();
        rsiPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.EmptyTickGenerator();

        // update grids to use ticks from the bottom plot
        pricePlot.Grid.XAxis = volumePlot.Axes.Bottom;
        rsiPlot.Grid.XAxis = volumePlot.Axes.Bottom;

        // update grids to use ticks from the right axis
        pricePlot.Grid.YAxis = pricePlot.Axes.Right;
        rsiPlot.Grid.YAxis = rsiPlot.Axes.Right;
        volumePlot.Grid.YAxis = volumePlot.Axes.Right;

        // link horizontal axes across all plots
        formsPlot1.Multiplot.ShareX([pricePlot, rsiPlot, volumePlot]);

        // use custom logic to tell the multiplot how large to make each plot
        CustomLayout customLayout = new();
        formsPlot1.Multiplot.Layout = customLayout;

        // wire mouse events to enable dragging dividers that separate plots
        formsPlot1.MouseDown += (s, e) =>
        {
            customLayout.DividerBeingDragged = customLayout.GetDividerUnder(e.Y);
            if (customLayout.DividerBeingDragged > 0)
            {
                customLayout.RememberAxisLimits();
                formsPlot1.UserInputProcessor.Disable();
            }
        };

        formsPlot1.MouseUp += (s, e) =>
        {
            customLayout.DividerBeingDragged = 0;
            formsPlot1.UserInputProcessor.Enable();
        };

        formsPlot1.MouseMove += (s, e) =>
        {
            if (customLayout.DividerBeingDragged > 0)
            {
                customLayout.ProcessMouseDrag(e.Y);
                customLayout.RecallAxisLimits();
                formsPlot1.Refresh();
            }
            else
            {
                Cursor = customLayout.IsOverDivider(e.Y) ? Cursors.SizeNS : Cursors.Default;
            }
        };

        // force a render in memory so axis changes are registered at startup
        formsPlot1.Multiplot.Render(formsPlot1.Width, formsPlot1.Height);
    }

    class CustomLayout() : IMultiplotLayout
    {
        readonly CustomRowPosition PositionRowA = new(0);
        readonly CustomRowPosition PositionRowB = new(1);
        readonly CustomRowPosition PositionRowC = new(2);
        List<CustomRowPosition> PositionRows => [PositionRowA, PositionRowB, PositionRowC];

        Plot PlotA = null!;
        Plot PlotB = null!;
        Plot PlotC = null!;

        MultiAxisLimits MouseDownLimitsA = null!;
        MultiAxisLimits MouseDownLimitsB = null!;
        MultiAxisLimits MouseDownLimitsC = null!;

        public int DividerBeingDragged = 0;

        public float SnapDistance { get; set; } = 5;

        public void ResetAllPositions(Multiplot multiplot)
        {
            if (multiplot.Count != 3)
                throw new InvalidOperationException("Expect exactly 3 plots");

            if (PlotA is null)
            {
                PlotA = multiplot.GetPlot(0);
                PlotB = multiplot.GetPlot(1);
                PlotC = multiplot.GetPlot(2);
            }

            multiplot.SetPosition(0, PositionRowA);
            multiplot.SetPosition(1, PositionRowB);
            multiplot.SetPosition(2, PositionRowC);
        }

        public bool IsOverDivider(float yPixel) => GetDividerUnder(yPixel) > 0;
        public bool IsOverDivider1(float yPixel) => Math.Abs(yPixel - PositionRowA.LastRect.Bottom) <= SnapDistance;
        public bool IsOverDivider2(float yPixel) => Math.Abs(yPixel - PositionRowB.LastRect.Bottom) <= SnapDistance;
        public int GetDividerUnder(float yPixel)
        {
            if (IsOverDivider1(yPixel)) return 1;
            else if (IsOverDivider2(yPixel)) return 2;
            else return 0;
        }

        public void RememberAxisLimits()
        {
            MouseDownLimitsA = new(PlotA);
            MouseDownLimitsB = new(PlotB);
            MouseDownLimitsC = new(PlotC);
        }

        public void RecallAxisLimits()
        {
            MouseDownLimitsA.Recall();
            MouseDownLimitsB.Recall();
            MouseDownLimitsC.Recall();
        }

        public void ProcessMouseDrag(float yPixel)
        {
            float lastFigureHeight = PositionRows.Select(x => x.LastRect.Height).Sum();
            float minimumHeight = 50;

            if (DividerBeingDragged == 1)
            {
                float bottomPlotHeight = PositionRowC.LastRect.Height;
                float topPlotHeight = Math.Clamp(yPixel, minimumHeight, lastFigureHeight - bottomPlotHeight - minimumHeight);
                float middlePlotHeight = lastFigureHeight - bottomPlotHeight - topPlotHeight;
                PositionRows.ForEach(x =>
                {
                    x.MiddlePlotHeight = middlePlotHeight;
                    x.BottomPlotHeight = bottomPlotHeight;
                });
            }

            if (DividerBeingDragged == 2)
            {
                float topPlotHeight = PositionRowA.LastRect.Height;
                float bottomPlotHeight = Math.Clamp(lastFigureHeight - yPixel, minimumHeight, lastFigureHeight - topPlotHeight - minimumHeight);
                float middlePlotHeight = lastFigureHeight - bottomPlotHeight - topPlotHeight;
                PositionRows.ForEach(x =>
                {
                    x.MiddlePlotHeight = middlePlotHeight;
                    x.BottomPlotHeight = bottomPlotHeight;
                });
            }
        }
    }

    class CustomRowPosition(int rowIndex) : ISubplotPosition
    {
        public float MiddlePlotHeight { get; set; } = 100;
        public float BottomPlotHeight { get; set; } = 100;

        public PixelRect LastRect { get; private set; }

        public PixelRect GetRect(PixelRect figureRect)
        {
            float bottomPlotTop = figureRect.Bottom - BottomPlotHeight;
            float middlePlotTop = bottomPlotTop - MiddlePlotHeight;

            LastRect = rowIndex switch
            {
                0 => new(figureRect.Left, figureRect.Right, bottom: middlePlotTop, top: figureRect.Top),
                1 => new(figureRect.Left, figureRect.Right, bottom: bottomPlotTop, top: middlePlotTop),
                2 => new(figureRect.Left, figureRect.Right, bottom: figureRect.Bottom, top: bottomPlotTop),
                _ => throw new IndexOutOfRangeException(nameof(rowIndex)),
            };

            return LastRect;
        }
    }
}
