using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotCookbook.Recipes.PlotTypes;

internal class RangePlot : RecipePageBase
{
    public override RecipePageDetails PageDetails => new RecipePageDetails
    {
        Chapter = Chapter.PlotTypes,
        PageName = "Range plot",
        PageDescription = "Range plot represents a range of values as a polygon, can be created from 2 scatter plots or from a custom item collection",
    };

    internal class Quickstart : RecipeTestBase
    {
        public override string Name => "Range Plot Quickstart";
        public override string Description => "Range plots use two values per X to generate the polygon, the FillStyle, LineStyle and MarkerStyle are all customizable.";

        [Test]
        public override void Recipe()
        {
            int count = 20;
            RandomDataGenerator dataGen = new RandomDataGenerator(1234567);
            double[] top = dataGen.RandomWalk(count);
            double[] bottom = dataGen.RandomWalk(count);
            var scatter1 = myPlot.Add.Scatter(top.Select((y, i) => new Coordinates((double)i, y)).ToArray());
            var scatter2 = myPlot.Add.Scatter(bottom.Select((y, i) => new Coordinates((double)i, y)).ToArray());
            myPlot.Add.RangePlot(scatter1, scatter2);
            myPlot.AutoScale();
        }
      
    }

    internal class Styling : RecipeTestBase
    {
        public override string Name => "Range Plot Styling";
        public override string Description => "The FillStyle, LineStyle and MarkerStyle can be customized to provide a wide variety of different options. Note that FillStyle is nullable, making it null will remove the filling, to remove the outline, set the LineStyle to LineStyle.NoLine, and to remove the markers set the MarkerStyle to MarkerStyle.None.";

        [Test]
        public override void Recipe()
        {
            int count = 20;
            RandomDataGenerator dataGen = new RandomDataGenerator(1234567);
            double[] top = dataGen.RandomWalk(count);
            double[] bottom = dataGen.RandomWalk(count);
            var data = Enumerable.Range(0, count).Select(x => ((double)x, top[x], bottom[x])).ToArray();
            var rangePlot = myPlot.Add.RangePlot(data);
            rangePlot.FillStyle = new FillStyle
            {
                Color = Colors.OrangeRed,
            };
            rangePlot.MarkerStyle = new MarkerStyle(MarkerShape.OpenSquare, 8);
            rangePlot.LineStyle = new LineStyle
            {
                AntiAlias = true,
                Color = Colors.DarkBlue,
                Pattern = LinePattern.Dot,
                Width = 2
            };
            myPlot.AutoScale();
        }
    }
}

