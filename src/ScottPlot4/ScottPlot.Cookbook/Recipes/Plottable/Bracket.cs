using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class BracketQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Bracket();
        public string ID => "bracket_quickstart";
        public string Title => "Bracket Annotations";
        public string Description => "Brackets are useful for annotating a range of data.";

        public void ExecuteRecipe(Plot plt)
        {
            Func<double, double?> xSquared = (double x) => x * x;
            var function = plt.AddFunction(xSquared);

            var x = 2;
            var dx = 0.1;

            var bracket1 = plt.AddBracket(x, xSquared(x).Value, x + dx, xSquared(x).Value);
            bracket1.Label = "dx";

            var bracket2 = plt.AddBracket(x + dx, xSquared(x).Value, x + dx, xSquared(x + dx).Value);
            bracket2.Label = "dy";

            plt.SetAxisLimits(1, 3, 1, 6);
        }
    }

    public class BracketInvert : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Bracket();
        public string ID => "bracket_invert";
        public string Title => "Inverted Brackets";
        public string Description => "Sometimes the orientation of a bracket should be flipped.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random();
            double x1 = 0;
            double y1 = 0;
            double x2 = rand.NextDouble() - 0.5;
            double y2 = rand.NextDouble() - 0.5;

            var defaultBracket = plt.AddBracket(x1, y1, x2, y2);
            defaultBracket.Label = "Default";

            var invertedBracket = plt.AddBracket(x1, y1, x2, y2);
            invertedBracket.Label = "Inverted";
            invertedBracket.LabelCounterClockwise = true;

            plt.SetAxisLimits(-2, 2, -2, 2);
        }
    }
}
