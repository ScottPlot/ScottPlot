namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Pie : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Pie";
    public string CategoryDescription => "Pie charts illustrate numerical proportions as slices of a circle.";

    public class PieQuickstart : RecipeBase
    {
        public override string Name => "Pie Chart from Values";
        public override string Description => "A pie chart can be created from a few values.";

        [Test]
        public override void Execute()
        {
            double[] values = { 5, 2, 8, 4, 8 };
            var pie = myPlot.Add.Pie(values);
            pie.ExplodeFraction = .1;

            // hide unnecessary plot components
            myPlot.Axes.Frameless();
            myPlot.HideGrid();
        }
    }

    public class PieSlices : RecipeBase
    {
        public override string Name => "Pie Chart from Slices";
        public override string Description => "A pie chart can be created from a collection of slices.";

        [Test]
        public override void Execute()
        {
            List<PieSlice> slices =
            [
                new PieSlice() { Value = 5, FillColor = Colors.Red, Label = "Red", LegendText = "R" },
                new PieSlice() { Value = 2, FillColor = Colors.Orange, Label = "Orange" },
                new PieSlice() { Value = 8, FillColor = Colors.Gold, Label = "Yellow" },
                new PieSlice() { Value = 4, FillColor = Colors.Green, Label = "Green", LegendText = "G" },
                new PieSlice() { Value = 8, FillColor = Colors.Blue, Label = "Blue", LegendText = "B" },
            ];

            var pie = myPlot.Add.Pie(slices);
            pie.ExplodeFraction = .1;
            pie.SliceLabelDistance = 1.4;

            myPlot.ShowLegend();

            // hide unnecessary plot components
            myPlot.Axes.Frameless();
            myPlot.HideGrid();
        }
    }

    public class PieDonut : RecipeBase
    {
        public override string Name => "Donut from Slices";
        public override string Description => "A donut chart is a pie chart with an open center. " +
            "Donut charts can be created from a collection of slices.";

        [Test]
        public override void Execute()
        {
            List<PieSlice> slices = new()
            {
                new PieSlice() { Value = 5, FillColor = Colors.Red, Label = "Red" },
                new PieSlice() { Value = 2, FillColor = Colors.Orange, Label = "Orange" },
                new PieSlice() { Value = 8, FillColor = Colors.Gold, Label = "Yellow" },
                new PieSlice() { Value = 4, FillColor = Colors.Green, Label = "Green" },
                new PieSlice() { Value = 8, FillColor = Colors.Blue, Label = "Blue" },
            };

            var pie = myPlot.Add.Pie(slices);
            pie.DonutFraction = .5;

            myPlot.ShowLegend();

            // hide unnecessary plot components
            myPlot.Axes.Frameless();
            myPlot.HideGrid();
        }
    }

    public class PieSliceHatch : RecipeBase
    {
        public override string Name => "Pie Slice with Hatch";
        public override string Description => "Individual slices may be given a custom hatch style";

        [Test]
        public override void Execute()
        {
            var pie = myPlot.Add.Pie([5, 4, 6]);

            // customize the hatch style for a single slice
            pie.Slices[0].Fill.Hatch = new ScottPlot.Hatches.Striped();
            pie.Slices[0].Fill.HatchColor = pie.Slices[0].Fill.Color.Lighten(.2);

            // hide unnecessary plot components
            myPlot.Axes.Frameless();
            myPlot.HideGrid();
        }
    }

    public class PieRotation : RecipeBase
    {
        public override string Name => "Pie Chart Rotation";
        public override string Description => "Pie charts may be rotated to control where the first slice begins.";

        [Test]
        public override void Execute()
        {
            double[] values = { 5, 2, 8, 4, 8 };
            var pie = myPlot.Add.Pie(values);
            pie.ExplodeFraction = .1;
            pie.Rotation = Angle.FromDegrees(90);

            // hide unnecessary plot components
            myPlot.Axes.Frameless();
            myPlot.HideGrid();
        }
    }

    public class PieSliceLabels : RecipeBase
    {
        public override string Name => "Pie Slice Labels";
        public override string Description => "Slice labels can be displayed " +
            "centered with the slice at a customizable distance from the " +
            "center of the pie.";

        [Test]
        public override void Execute()
        {
            PieSlice slice1 = new() { Value = 5, FillColor = Colors.Red, Label = "alpha" };
            PieSlice slice2 = new() { Value = 2, FillColor = Colors.Orange, Label = "beta" };
            PieSlice slice3 = new() { Value = 8, FillColor = Colors.Gold, Label = "gamma" };
            PieSlice slice4 = new() { Value = 4, FillColor = Colors.Green, Label = "delta" };
            PieSlice slice5 = new() { Value = 8, FillColor = Colors.Blue, Label = "epsilon" };

            List<PieSlice> slices = new() { slice1, slice2, slice3, slice4, slice5 };

            // setup the pie to display slice labels
            var pie = myPlot.Add.Pie(slices);
            pie.ExplodeFraction = .1;
            pie.SliceLabelDistance = 1.3;

            // color each label's text to match the slice
            slices.ForEach(x => x.LabelFontColor = x.FillColor.Darken(.5));

            // styling can be customized for individual slices
            slice2.LabelStyle.FontSize = 18;
            slice2.LabelStyle.Bold = true;
            slice2.LabelStyle.Italic = true;

            // hide unnecessary plot components
            myPlot.Axes.Frameless();
            myPlot.HideGrid();
        }
    }

    public class PieSliceLabelsPercent : RecipeBase
    {
        public override string Name => "Pie with Percent Labels";
        public override string Description => "Slice labels may be adapted to display any text " +
            "(including numerical values) centered over each slice.";

        [Test]
        public override void Execute()
        {
            // create a pie chart
            double[] values = [6, 8, 10];
            var pie = myPlot.Add.Pie(values);
            pie.ExplodeFraction = .1;
            pie.SliceLabelDistance = 0.5;

            // determine percentages for each slice
            double total = pie.Slices.Select(x => x.Value).Sum();
            double[] percentages = pie.Slices.Select(x => x.Value / total * 100).ToArray();

            // set each slice label to its percentage
            for (int i = 0; i < pie.Slices.Count; i++)
            {
                pie.Slices[i].Label = $"{percentages[i]:0.0}%";
                pie.Slices[i].LabelFontSize = 20;
                pie.Slices[i].LabelBold = true;
                pie.Slices[i].LabelFontColor = Colors.Black.WithAlpha(.5);
            }

            // hide unnecessary plot components
            myPlot.Axes.Frameless();
            myPlot.HideGrid();
        }
    }

    public class PieSliceLabelsAndLegend : RecipeBase
    {
        public override string Name => "Pie with Different Labels";
        public override string Description => "Pie slices may have labels independent from those displayed in the legend.";

        [Test]
        public override void Execute()
        {
            // create a pie chart
            double[] values = [6, 8, 10];
            var pie = myPlot.Add.Pie(values);
            pie.ExplodeFraction = 0.1;
            pie.SliceLabelDistance = 0.5;

            // set different labels for slices and legend
            double total = pie.Slices.Select(x => x.Value).Sum();
            for (int i = 0; i < pie.Slices.Count; i++)
            {
                pie.Slices[i].LabelFontSize = 20;
                pie.Slices[i].Label = $"{pie.Slices[i].Value}";
                pie.Slices[i].LegendText = $"{pie.Slices[i].Value} " +
                    $"({pie.Slices[i].Value / total:p1})";
            }

            // hide unnecessary plot components
            myPlot.Axes.Frameless();
            myPlot.HideGrid();
        }
    }
}
