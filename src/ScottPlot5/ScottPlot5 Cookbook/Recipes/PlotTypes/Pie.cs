﻿namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Pie : ICategory
{
    public string Chapter => "Plot Types";
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
        }
    }

    public class PieSlices : RecipeBase
    {
        public override string Name => "Pie Chart from Slices";
        public override string Description => "A pie chart can be created from a collection of slices.";

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
            pie.ExplodeFraction = .1;

            myPlot.ShowLegend();
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
            pie.ShowSliceLabels = true;
            pie.SliceLabelDistance = 1.3;

            // color each label's text to match the slice
            slices.ForEach(x => x.LabelFontColor = x.FillColor);

            // styling can be customized for individual slices
            slice5.LabelStyle.FontSize = 22;
            slice5.LabelStyle.Bold = true;
            slice5.LabelStyle.Italic = true;
        }
    }

    public class PieSlicePercentLabels : RecipeBase
    {
        public override string Name => "Pie Slice Percent Labels";
        public override string Description => "Slice percent labels can be displayed " +
            "centered with the slice at a specific format " +
            "and a customizable distance from the center of the pie.";

        [Test]
        public override void Execute()
        {
            PieSlice slice1 = new() { Value = 2, FillColor = Colors.Red, Label = "alpha" };
            PieSlice slice2 = new() { Value = 4, FillColor = Colors.Orange, Label = "beta" };
            PieSlice slice3 = new() { Value = 6, FillColor = Colors.Gold, Label = "gamma" };
            PieSlice slice4 = new() { Value = 8, FillColor = Colors.Green, Label = "delta" };
            PieSlice slice5 = new() { Value = 10, FillColor = Colors.Blue, Label = "epsilon" };

            List<PieSlice> slices = [slice1, slice2, slice3, slice4, slice5];

            // setup the pie to display slice labels
            var pie = myPlot.Add.Pie(slices);
            pie.ExplodeFraction = 0.05;
            pie.SlicePercentFormat = "P1";
            pie.SlicePercentDistance = 0.66;

            // styling can be customized for individual slices
            slice5.PercentLabelStyle.FontSize = 22;
            slice5.PercentLabelStyle.Bold = true;
            slice5.PercentLabelStyle.Italic = true;
        }
    }
}
