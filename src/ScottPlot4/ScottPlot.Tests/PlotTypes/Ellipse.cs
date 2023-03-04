using NUnit.Framework;
using ScottPlotTests;

namespace ScottPlot.Tests.PlotTypes;

internal class Ellipse
{
    [Test]
    public void Test_Ellipse_AddCircle()
    {
        Plot plt = new(400, 300);

        plt.AddCircle(0, 0, 5);
        plt.AddCircle(2, 3, 5);
        plt.AddCircle(2, 3, 0.5);

        var filled = plt.AddCircle(0, 0, -1);
        filled.Color = System.Drawing.Color.Navy;
        filled.HatchColor = System.Drawing.Color.Yellow;
        filled.HatchStyle = Drawing.HatchStyle.DottedDiamond;

        TestTools.SaveFig(plt);
    }

    [Test]
    public void Test_Ellipse_AddElipse()
    {
        Plot plt = new(400, 300);

        plt.AddEllipse(3, 0, 2, 2);
        plt.AddEllipse(2, 3, 5, 1);
        plt.AddEllipse(0, 0, 1, 5);

        TestTools.SaveFig(plt);
    }

    [Test]
    public void Test_Ellipse_Legend()
    {
        Plot plt = new(400, 300);

        var c1 = plt.AddCircle(0, 0, 1);
        c1.Label = "outlined";
        c1.BorderLineWidth = 3;
        c1.BorderLineStyle = LineStyle.Solid;
        c1.BorderColor = System.Drawing.Color.Blue;

        var c2 = plt.AddCircle(1, 1, 1);
        c2.Label = "filled";
        c2.BorderLineStyle = LineStyle.None;
        c2.Color = System.Drawing.Color.Green;

        var c3 = plt.AddCircle(2, 2, 1);
        c3.Label = "both";
        c3.Color = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Green);
        c3.BorderColor = System.Drawing.Color.Blue;
        c3.BorderLineStyle = LineStyle.Dot;

        plt.Legend();

        TestTools.SaveFig(plt);
    }
}
