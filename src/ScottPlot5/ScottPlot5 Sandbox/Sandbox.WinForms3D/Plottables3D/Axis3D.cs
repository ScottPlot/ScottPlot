using Sandbox.WinForms3D.Primitives3D;
using ScottPlot;

namespace Sandbox.WinForms3D.Plottables3D;

public class Axis3D : IPlottable3D
{
    readonly ScottPlot.Label LabelStyle = new()
    {
        FontSize = 15,
        Bold = true,
        Alignment = Alignment.MiddleCenter
    };

    public void Render(RenderPack3D rp)
    {
        RenderGrid(rp);
        RenderSpines(rp);
        RenderAxisLabels(rp);
    }

    private void RenderGrid(RenderPack3D rp)
    {
        int divisions = 10;

        List<Line3D> lines = [];

        for (int xIndex = 0; xIndex <= divisions; xIndex++)
        {
            double x = xIndex * 1.0 / divisions;
            Point3D xStart = new(x, 0, 0);
            Point3D yEnd = new(x, 1, 0);
            Point3D zEnd = new(x, 0, 1);
            lines.Add(new Line3D(xStart, yEnd));
            lines.Add(new Line3D(xStart, zEnd));
        }

        for (int yIndex = 0; yIndex <= divisions; yIndex++)
        {
            double y = yIndex * 1.0 / divisions;
            Point3D yStart = new(0, y, 0);
            Point3D xEnd = new(1, y, 0);
            Point3D zEnd = new(0, y, 1);
            lines.Add(new Line3D(yStart, xEnd));
            lines.Add(new Line3D(yStart, zEnd));
        }

        for (int zIndex = 0; zIndex <= divisions; zIndex++)
        {
            double z = zIndex * 1.0 / divisions;
            Point3D zStart = new(0, 0, z);
            Point3D xEnd = new(1, 0, z);
            Point3D yEnd = new(0, 1, z);
            lines.Add(new Line3D(zStart, xEnd));
            lines.Add(new Line3D(zStart, yEnd));
        }

        foreach (Line3D line in lines)
        {
            line.LineStyle.Color = Colors.Black.WithAlpha(.2);
            line.Render(rp);
        }
    }

    private void RenderSpines(RenderPack3D rp)
    {
        Point3D origin = new(0, 0, 0);
        Point3D xUnit = new(1, 0, 0);
        Point3D yUnit = new(0, 1, 0);
        Point3D zUnit = new(0, 0, 1);

        List<Line3D> lines2 =
        [
            new(origin, xUnit, 2, Colors.Red),
            new(origin, yUnit, 2, Colors.Green),
            new(origin, zUnit, 2, Colors.Blue),
        ];

        foreach (Line3D line in lines2)
        {
            line.Render(rp);
        }
    }

    private void RenderAxisLabels(RenderPack3D rp)
    {
        double padding = .1;
        Point3D ptX = new(1 + padding, 0, 0);
        Point3D ptY = new(0, 1 + padding, 0);
        Point3D ptZ = new(0, 0, 1 + padding);

        LabelStyle.Text = "X";
        LabelStyle.ForeColor = Colors.Red;
        LabelStyle.Render(rp.Canvas, rp.GetPixel(ptX), rp.Paint);

        LabelStyle.Text = "Y";
        LabelStyle.ForeColor = Colors.Green;
        LabelStyle.Render(rp.Canvas, rp.GetPixel(ptY), rp.Paint);

        LabelStyle.Text = "Z";
        LabelStyle.ForeColor = Colors.Blue;
        LabelStyle.Render(rp.Canvas, rp.GetPixel(ptZ), rp.Paint);
    }
}
