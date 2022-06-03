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
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            plt.AddBracket(0, 1, 0, 0, "Bracket A");
            plt.AddBracket(25, -1, 38, -1, "Bracket B");
            plt.AddBracket(20, .55, 27, -.3, "Bracket C");

            plt.Margins(.2, .2); // zoom out slightly to make room for labels
        }
    }

    public class BracketInvert : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Bracket();
        public string ID => "bracket_invert";
        public string Title => "Inverted Brackets";
        public string Description => "By default bracket labels appear clockwise " +
            "relative to the line formed by their two points. " +
            "Users can invert the direction of brackets as needed.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddSignal(DataGen.Sin(51));
            plt.AddSignal(DataGen.Cos(51));

            var bracketA = plt.AddBracket(50, 1, 50, 0, "Bracket A");
            bracketA.LabelCounterClockwise = true;

            var backetB = plt.AddBracket(13, 1, 50, 1, "Bracket B");
            backetB.LabelCounterClockwise = true;

            var backetC = plt.AddBracket(20, .65, 27, -.20, "Bracket C");
            backetC.LabelCounterClockwise = true;

            plt.Margins(.2, .2); // zoom out slightly to make room for labels
        }
    }

    public class BracketStyle : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Bracket();
        public string ID => "bracket_style";
        public string Title => "Styling Brackets";
        public string Description => "Brackets have additional options for customizations.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.AddBracket(-1, 0, 0, 1, "Default Style");

            var bracketA = plt.AddBracket(0, 0, 1, 1, "Large Font");
            bracketA.Font.Size = 24;

            var bracketB = plt.AddBracket(1, 0, 2, 1, "Custom Color");
            bracketB.Color = Color.Magenta;

            var bracketC = plt.AddBracket(2, 0, 3, 1, "Longer Stem & Edges");
            bracketC.EdgeLength = 20;

            var bracketD = plt.AddBracket(3, 0, 4, 1, "Thicker Bracket");
            bracketD.LineWidth = 3;
            bracketD.Font.Bold = true;

            plt.AddBracket(4, 0, 5, 1, label: null);

            plt.Margins(.2, .2); // zoom out slightly to make room for labels
        }
    }
}
