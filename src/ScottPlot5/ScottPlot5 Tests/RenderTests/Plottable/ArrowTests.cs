namespace ScottPlotTests.RenderTests.Plottable;

internal class ArrowTests
{
    [Test]
    public void Test_Arrow_Shapes()
    {
        Plot myPlot = new();

        ArrowShape[] arrowShapes = Enum.GetValues<ArrowShape>().ToArray();

        for (int i = 0; i < arrowShapes.Length; i++)
        {
            Coordinates arrowTip = new(0, -i);
            Coordinates arrowBase = arrowTip.WithDelta(1, 0);
            var arrow = myPlot.Add.Arrow(arrowBase, arrowTip);
            arrow.ArrowShape = arrowShapes[i].GetShape();

            var txt = myPlot.Add.Text(arrowShapes[i].ToString(), arrowBase.WithDelta(.1, 0));
            txt.LabelFontColor = arrow.ArrowLineColor;
            txt.LabelAlignment = Alignment.MiddleLeft;
            txt.LabelFontSize = 26;
        }

        myPlot.Axes.SetLimits(-1, 3, -arrowShapes.Length - 1, 1);
        myPlot.HideGrid();
        myPlot.SaveTestImage();
    }
}
