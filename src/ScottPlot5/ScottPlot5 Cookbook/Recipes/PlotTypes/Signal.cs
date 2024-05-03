namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Signal : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Signal Plot";
    public string CategoryDescription => "Signal plots display evenly-spaced data";

    public class SignalQuickstart : RecipeBase
    {
        public override string Name => "Signal Plot Quickstart";
        public override string Description => "Signal plots are best for extremely large datasets. " +
            "They use render using special optimizations that allow highspeed interactivity " +
            "with plots containing millions of data points.";

        [Test]
        public override void Execute()
        {
            double[] values = Generate.RandomWalk(1_000_000);

            myPlot.Add.Signal(values);

            myPlot.Title("Signal Plot with 1 Million Points");
        }
    }

    public class SignalStyling : RecipeBase
    {
        public override string Name => "Signal Plot Styling";
        public override string Description => "Signal plots can be styled in a variety of ways.";

        [Test]
        public override void Execute()
        {
            var sig1 = myPlot.Add.Signal(Generate.Sin());
            sig1.Color = Colors.Magenta;
            sig1.LineWidth = 10;
            sig1.LegendText = "Sine";

            var sig2 = myPlot.Add.Signal(Generate.Cos());
            sig2.Color = Colors.Green;
            sig2.LineWidth = 5;
            sig2.LegendText = "Cosine";

            myPlot.ShowLegend();
        }
    }

    public class SignalOffset : RecipeBase
    {
        public override string Name => "Signal Offset";
        public override string Description => "Signal plots can be offset by a given X and Y value.";

        [Test]
        public override void Execute()
        {
            double[] values = ScottPlot.Generate.Sin(51);

            var sig1 = myPlot.Add.Signal(values);
            sig1.LegendText = "Default";

            var sig2 = myPlot.Add.Signal(values);
            sig2.Data.XOffset = 10;
            sig2.Data.YOffset = .25;
            sig2.LegendText = "Offset";

            myPlot.Legend.IsVisible = true;
        }
    }

    public class SignalScaleY : RecipeBase
    {
        public override string Name => "Signal Scaling";
        public override string Description => "Signal plots can be scaled vertically according to a user-defined amount.";

        [Test]
        public override void Execute()
        {
            // plot values between -1 and 1
            double[] values = ScottPlot.Generate.Sin(51);
            var signal = myPlot.Add.Signal(values);

            // increase the vertical scaling
            signal.Data.YScale = 500;
        }
    }

    public class SignalMarkerSize : RecipeBase
    {
        public override string Name => "Signal Marker Size";
        public override string Description => "Signal plots can have markers displayed at each point " +
            "which are only visible when the plot is zoomed in.";

        [Test]
        public override void Execute()
        {
            var sig1 = myPlot.Add.Signal(Generate.Cos());
            sig1.LegendText = "Default";
            sig1.Data.YOffset = 3;

            var sig2 = myPlot.Add.Signal(Generate.Cos());
            sig2.LegendText = "Large Markers";
            sig2.MaximumMarkerSize = 20;
            sig2.Data.YOffset = 2;

            var sig3 = myPlot.Add.Signal(Generate.Cos());
            sig3.LegendText = "Hidden Markers";
            sig3.MaximumMarkerSize = 0;
            sig3.Data.YOffset = 1;

            myPlot.Legend.IsVisible = true;
        }
    }

    public class SignalRenderIndexes : RecipeBase
    {
        public override string Name => "Partial Signal Rendering";
        public override string Description => "Even if a signal plot references a " +
            "large array of data, rendering can be limited to a range of values. If set," +
            "only the range of data between the minimum and maximum render indexes will be displayed.";

        [Test]
        public override void Execute()
        {
            double[] values = Generate.RandomWalk(1000);

            var sigAll = myPlot.Add.Signal(values);
            sigAll.LegendText = "Full";
            sigAll.Data.YOffset = 80;

            var sigLeft = myPlot.Add.Signal(values);
            sigLeft.LegendText = "Left";
            sigLeft.Data.YOffset = 60;
            sigLeft.Data.MaximumIndex = 700;

            var sigRight = myPlot.Add.Signal(values);
            sigRight.LegendText = "Right";
            sigRight.Data.YOffset = 40;
            sigRight.Data.MinimumIndex = 300;

            var sigMid = myPlot.Add.Signal(values);
            sigMid.LegendText = "Mid";
            sigMid.Data.YOffset = 20;
            sigMid.Data.MinimumIndex = 300;
            sigMid.Data.MaximumIndex = 700;

            myPlot.ShowLegend(Alignment.UpperRight);
            myPlot.Axes.Margins(top: .5);
        }
    }

    public class SignalGeneric : RecipeBase
    {
        public override string Name => "Signal Generic";
        public override string Description => "Signal plots support generic data types, " +
            "although double is typically the most performant.";

        [Test]
        public override void Execute()
        {
            int[] values = Generate.RandomIntegers(1000, -100, 100);

            myPlot.Add.Signal(values);
        }
    }

    public class SignalDateTime : RecipeBase
    {
        public override string Name => "Signal DateTime";
        public override string Description => "A signal plot may use DateTime units but " +
            "be sure to setup the respective axis to display using DateTime format.";

        [Test]
        public override void Execute()
        {
            DateTime start = new(2024, 1, 1);
            double[] ys = Generate.RandomWalk(200);

            var sig = myPlot.Add.Signal(ys);
            sig.Data.XOffset = start.ToOADate();
            sig.Data.Period = 1.0; // one day between each point

            myPlot.Axes.DateTimeTicksBottom();
        }
    }
}
