namespace ScottPlotCookbook.Recipes.PlotTypes;

public class Text : ICategory
{
    public string Chapter => "Plot Types";
    public string CategoryName => "Text";
    public string CategoryDescription => "Text labels can be placed on the plot in coordinate space";

    public class TextQuickstart : RecipeBase
    {
        public override string Name => "Text Quickstart";
        public override string Description => "Text can be placed anywhere in coordinate space.";

        [Test]
        public override void Execute()
        {
            myPlot.Add.Signal(Generate.Sin());
            myPlot.Add.Signal(Generate.Cos());
            myPlot.Add.Text("Hello, World", 25, 0.5);
        }
    }

    public class Formatting : RecipeBase
    {
        public override string Name => "Text Formatting";
        public override string Description => "Text formatting can be extensively customized.";

        [Test]
        public override void Execute()
        {
            var text = myPlot.Add.Text("Hello, World", 42, 69);
            text.Label.FontSize = 26;
            text.Label.Bold = true;
            text.Label.Rotation = -45;
            text.Label.ForeColor = Colors.Yellow;
            text.Label.BackColor = Colors.Navy.WithAlpha(.5);
            text.Label.BorderColor = Colors.Magenta;
            text.Label.BorderWidth = 3;
            text.Label.Padding = 10;
            text.Label.Alignment = Alignment.MiddleCenter;
        }
    }

    public class LabelLineHeight : RecipeBase
    {
        public override string Name => "Line Height";
        public override string Description => "Multiline labels have a default line height " +
            "estimated from the typeface and font size, however line height may be manually " +
            "defined by the user.";

        [Test]
        public override void Execute()
        {
            var label1 = myPlot.Add.Text($"line\nheight", 0, 0);
            label1.LineSpacing = 0;
            label1.FontColor = Colors.Red;

            var label2 = myPlot.Add.Text($"can\nbe", 1, 0);
            label2.LineSpacing = 10;
            label2.FontColor = Colors.Orange;

            var label3 = myPlot.Add.Text($"automatic\nor", 2, 0);
            label3.LineSpacing = null;
            label3.FontColor = Colors.Green;

            var label4 = myPlot.Add.Text($"set\nmanually", 3, 0);
            label4.LineSpacing = 15;
            label4.FontColor = Colors.Blue;

            myPlot.Axes.SetLimitsX(-.5, 4);
        }
    }

    public class TextOffset : RecipeBase
    {
        public override string Name => "Text Offset";
        public override string Description => "The offset properties can be used " +
            "to fine-tune text position in pixel units";

        [Test]
        public override void Execute()
        {
            for (int i = 0; i < 25; i += 5)
            {
                // place a marker at the point
                var marker = myPlot.Add.Marker(i, 1);

                // place a styled text label at the point
                var txt = myPlot.Add.Text($"{i}", i, 1);
                txt.FontSize = 16;
                txt.BorderColor = Colors.Black;
                txt.BorderWidth = 1;
                txt.Padding = 2;
                txt.Bold = true;
                txt.BackColor = marker.Color.WithAlpha(.5);

                // offset the text label by the given number of pixels
                txt.OffsetX = i;
                txt.OffsetY = i;
            }

            myPlot.Axes.SetLimitsX(-5, 30);
        }
    }
}
