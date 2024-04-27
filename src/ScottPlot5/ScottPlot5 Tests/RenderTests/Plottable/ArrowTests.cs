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

    [Test]
    public void Test_Arrow_Directions()
    {
        Plot myPlot = new();

        {
            // up
            Coordinates p1 = new(0, 1);
            Coordinates p2 = new(0, 2);
            CoordinateLine line = new(p1, p2);
            myPlot.Add.Arrow(line);
            myPlot.Add.Line(line);
            myPlot.Add.Marker(p2);
            Console.WriteLine($"N angle {line.SlopeDegrees}");
        }

        {
            // upper right
            Coordinates p1 = new(1, 1);
            Coordinates p2 = new(2, 2);
            CoordinateLine line = new(p1, p2);
            myPlot.Add.Arrow(line);
            myPlot.Add.Line(line);
            myPlot.Add.Marker(p2);
            Console.WriteLine($"NE angle {line.SlopeDegrees}");
        }

        {
            // right
            Coordinates p1 = new(1, 0);
            Coordinates p2 = new(2, 0);
            CoordinateLine line = new(p1, p2);
            myPlot.Add.Arrow(line);
            myPlot.Add.Line(line);
            myPlot.Add.Marker(p2);
            Console.WriteLine($"E angle {line.SlopeDegrees}");
        }

        {
            // lower right
            Coordinates p1 = new(1, -1);
            Coordinates p2 = new(2, -2);
            CoordinateLine line = new(p1, p2);
            myPlot.Add.Arrow(line);
            myPlot.Add.Line(line);
            myPlot.Add.Marker(p2);
            Console.WriteLine($"SE angle {line.SlopeDegrees}");
        }

        {
            // lower
            Coordinates p1 = new(0, -1);
            Coordinates p2 = new(0, -2);
            CoordinateLine line = new(p1, p2);
            myPlot.Add.Arrow(line);
            myPlot.Add.Line(line);
            myPlot.Add.Marker(p2);
            Console.WriteLine($"S angle {line.SlopeDegrees}");
        }

        {
            // lower left
            Coordinates p1 = new(-1, -1);
            Coordinates p2 = new(-2, -2);
            CoordinateLine line = new(p1, p2);
            myPlot.Add.Arrow(line);
            myPlot.Add.Line(line);
            myPlot.Add.Marker(p2);
            Console.WriteLine($"SW angle {line.SlopeDegrees}");
        }

        {
            // left
            Coordinates p1 = new(-1, 0);
            Coordinates p2 = new(-2, 0);
            CoordinateLine line = new(p1, p2);
            myPlot.Add.Arrow(line);
            myPlot.Add.Line(line);
            myPlot.Add.Marker(p2);
            Console.WriteLine($"W angle {line.SlopeDegrees}");
        }

        {
            // upper left
            Coordinates p1 = new(-1, 1);
            Coordinates p2 = new(-2, 2);
            CoordinateLine line = new(p1, p2);
            myPlot.Add.Arrow(line);
            myPlot.Add.Line(line);
            myPlot.Add.Marker(p2);
            Console.WriteLine($"NW angle {line.SlopeDegrees}");
        }

        myPlot.Axes.SetLimits(-5, 5, -5, 5);
        myPlot.SaveTestImage();
    }
}
