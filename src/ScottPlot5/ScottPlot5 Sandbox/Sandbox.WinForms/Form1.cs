using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    readonly System.Windows.Forms.Timer UpdatePlotTimer = new() { Interval = 50, Enabled = true };

    // Data for first data logger
    Coordinates[] stockValues = new double[][] { [45496.660416666666, 554.21], [45496.66111111111, 554.22], [45496.66180555556, 553.96], [45496.6625, 553.37], [45496.663194444445, 553.37], [45496.66388888889, 553.38], [45496.66458333333, 553.46], [45496.66527777778, 553.39], [45496.665972222225, 553.74] }.Select(ds => new Coordinates(ds[0], ds[1])).ToArray();

    // Data for second data logger
    Coordinates[] averageValues = new double[][] { [45496.65972222222, 554.43], [45496.660416666666, 554.32], [45496.66111111111, 554.2866666666666], [45496.66180555556, 554.205], [45496.6625, 554.038], [45496.663194444445, 553.9266666666666], [45496.66388888889, 553.8485714285714], [45496.66458333333, 553.8], [45496.66527777778, 553.7544444444444] }.Select(ds => new Coordinates(ds[0], ds[1])).ToArray();
    int stockIndex = 0;
    int averageIndex = 0;
    public Form1()
    {
        InitializeComponent();

        formsPlot1.UserInputProcessor.IsEnabled = true;
        //formsPlot1.Plot.Axes.DateTimeTicksBottom();
        //formsPlot1.Plot.Axes.Margins(1, 1);

        var stockLogger = formsPlot1.Plot.Add.DataLogger();
        var averagesLogger = formsPlot1.Plot.Add.DataLogger();


        stockLogger.LegendText = "Stock";
        averagesLogger.LegendText = "Stock Moving Averages";


        var axis1 = formsPlot1.Plot.Axes.Left;
        stockLogger.Axes.YAxis = axis1;
        averagesLogger.Axes.YAxis = axis1;

        (axis1 as ScottPlot.AxisPanels.AxisBase)?.Color(stockLogger.Color);

        //formsPlot1.Plot.ShowLegend();


        UpdatePlotTimer.Tick += (s, e) =>
        {
            if (stockIndex < stockValues.Length)
            {
                stockLogger.Add(stockValues[stockIndex++]);
            }
            if (averageIndex < averageValues.Length)
            {
                averagesLogger.Add(averageValues[averageIndex++]);
            }

            formsPlot1.Plot.Axes.AutoScale();
            formsPlot1.Refresh();
            UpdatePlotTimer.Enabled = (stockIndex < stockValues.Length) || (averageIndex < averageValues.Length);
            if (!UpdatePlotTimer.Enabled)
            {
                Console.WriteLine("Done");
                formsPlot1.Plot.SaveSvg("test_svg.svg", 800, 600);

                ScottPlot.Plot myPlot = new();

                List<PieSlice> slices = new()
{
    new PieSlice() { Value = 31.6, FillColor = Colors.Red, Label = "Category 1 - 31.6%" },
    new PieSlice() { Value = 27.2, FillColor = Colors.Orange, Label = "Category 2 - 27.2%" },
    new PieSlice() { Value = 16.2, FillColor = Colors.Gold, Label = "Category 3 - 16.2%" },
    new PieSlice() { Value = 1.4, FillColor = Colors.Green, Label = "Category 4 - 1.4%" },
    new PieSlice() { Value = 3.7, FillColor = Colors.Blue, Label = "Category 5 - 3.7%" },
    new PieSlice() { Value = 14.8, FillColor = Colors.AliceBlue, Label = "Category 6 - 31.6%" },
    new PieSlice() { Value = 3.3, FillColor = Colors.Aqua, Label = "Category 7 - 3.3%" },
    new PieSlice() { Value = 1.4, FillColor = Colors.Beige, Label = "Category 8 - 1.4%" },
    new PieSlice() { Value = 99.6, FillColor = Colors.BlueViolet, Label = "Category 9 - 99.6%" },
};

                var pie = myPlot.Add.Pie(slices);
                myPlot.Font.Set("Arial");
                pie.DonutFraction = .6;
                pie.Padding = 0;
                myPlot.Legend.FontSize = 10.666667f;
                myPlot.HideGrid();
                myPlot.Layout.Frameless();
                myPlot.ShowLegend(Edge.Right);

                myPlot.SaveSvg("pietest.svg", 500, 375);
            }
        };
    }
}
