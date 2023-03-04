using System;
using System.Drawing;

namespace ScottPlot.Cookbook.Recipes.Plottable;

public class Ellipse
{
    public class EllipseQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Ellipse();
        public string ID => "ellipse_quickstart";
        public string Title => "Ellipse Quickstart";
        public string Description => "Ellipses can be added to plots";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new(0);
            for (int i = 0; i < 5; i++)
            {
                plt.AddEllipse(
                    x: rand.Next(-10, 10),
                    y: rand.Next(-10, 10),
                    xRadius: rand.Next(1, 7),
                    yRadius: rand.Next(1, 7),
                    lineWidth: rand.Next(1, 10));
            }
        }
    }

    public class CircleQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Ellipse();
        public string ID => "circle_quickstart";
        public string Title => "Circle Quickstart";
        public string Description => "Circles can be added to plots. " +
            "Circles are really Ellipses with the same X and Y radius. " +
            "Note that circles appear as ellipses unless the plot has a square coordinate system.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new(0);
            for (int i = 0; i < 5; i++)
            {
                plt.AddCircle(
                    x: rand.Next(-10, 10),
                    y: rand.Next(-10, 10),
                    radius: rand.Next(1, 7),
                    lineWidth: rand.Next(1, 10));
            }
        }
    }

    public class CircleSquarePixels : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Ellipse();
        public string ID => "circle_square_pixel";
        public string Title => "Circle with Locked Scale";
        public string Description => "For circles to always appear circular, " +
            "the coordinate system must be forced to always display square-shaped pixels. " +
            "This can be achieved by enabling the axis scale lock.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new(0);
            for (int i = 0; i < 5; i++)
            {
                plt.AddCircle(
                    x: rand.Next(-10, 10),
                    y: rand.Next(-10, 10),
                    radius: rand.Next(1, 7),
                    lineWidth: rand.Next(1, 10));
            }

            plt.AxisScaleLock(true); // this forces pixels to have 1:1 scale ratio
        }
    }

    public class EllipseStyling : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Ellipse();
        public string ID => "ellipse_styling";
        public string Title => "Ellipse Styling";
        public string Description => "Ellipses styles can be extensively customized";

        public void ExecuteRecipe(Plot plt)
        {
            var el = plt.AddCircle(0, 0, 5);
            el.BorderLineWidth = 5;
            el.BorderLineStyle = LineStyle.Dash;
            el.BorderColor = Color.Green;
            el.Color = Color.Navy;
            el.HatchColor = Color.Red;
            el.HatchStyle = Drawing.HatchStyle.StripedUpwardDiagonal;

            plt.SetAxisLimits(-10, 10, -10, 10);
        }
    }
}
