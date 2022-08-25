using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class RadarQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Radar();
        public string ID => "radar_quickstart";
        public string Title => "Radar";
        public string Description =>
            "A radar chart concisely displays multiple values. " +
            "Radar plots are also called a spider charts or star charts.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] values = {
                { 78,  83, 84, 76, 43 },
                { 100, 50, 70, 60, 90 }
            };

            plt.AddRadar(values);

            // improve plot styling
            plt.Frameless();
            plt.Grid(enable: false);
        }
    }

    public class RadarPolygon : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Radar();
        public string ID => "radar_straightLines";
        public string Title => "Straight Axis Lines";
        public string Description =>
            "Change the axis type to polygon to display radar charts with straight lines.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] values = {
                    { 78, 83, 84, 76, 43 },
                    { 100, 50, 70, 60, 90 }
                };

            var radarPlot = plt.AddRadar(values);
            radarPlot.AxisType = RadarAxis.Polygon;
        }
    }

    public class RadarNoAxis : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Radar();
        public string ID => "radar_noLines";
        public string Title => "No Axis Lines";
        public string Description => "A radar chart can have no drawn axis as well.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] values = {
                    { 78, 83, 84, 76, 43 },
                    { 100, 50, 70, 60, 90 }
                };

            var radarPlot = plt.AddRadar(values);
            radarPlot.AxisType = RadarAxis.None;
            radarPlot.ShowAxisValues = false;
        }
    }

    public class RadarSmooth : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Radar();
        public string ID => "radar_smooth";
        public string Title => "Smooth Radar";
        public string Description =>
            "The Smooth field controls whether radar areas are drawn with smooth or straight lines.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] values = {
                    { 78, 83, 84, 76, 43 },
                    { 100, 50, 70, 60, 90 }
                };

            var radarPlot = plt.AddRadar(values);
            radarPlot.Smooth = true;
        }
    }

    public class RadarUnlabeledAxes : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Radar();
        public string ID => "radar_labelCategory";
        public string Title => "Labeled Categories";
        public string Description => "Category labels can be displayed on the radar chart.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] values = {
                { 5, 3, 10, 15, 3, 2, 256 },
                { 5, 2, 10, 10, 1, 4, 252 },
            };

            var radar = plt.AddRadar(values, independentAxes: true);
            radar.CategoryLabels = new string[] { "Wins", "Poles", "Podiums", "Points Finishes", "DNFs", "Fastest Laps", "Points" };
            radar.GroupLabels = new[] { "Sebastian Vettel", "Fernando Alonso" };
            radar.ShowAxisValues = false;

            // customize the plot
            plt.Title("2010 Formula One World Championship");
            plt.Legend();

            /* Data represents the 2010 Formula One World Championship
             * https://en.wikipedia.org/wiki/2010_Formula_One_World_Championship
             * Note: Alonso did not finish (DNF) in the Malaysian GP, but was included 
             * here because he completed >90% of the race distance.
             */
        }
    }

    public class RadarWithLabels : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Radar();
        public string ID => "radar_labelValue";
        public string Title => "Labeled Values";
        public string Description => "Labels can be displayed on the arms of the radar chart.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] values = {
                { 5, 3, 10, 15, 3, 2 },
                { 5, 2, 10, 10, 1, 4 },
            };

            var radar = plt.AddRadar(values);
            radar.CategoryLabels = new string[] { "Wins", "Poles", "Podiums", "Points Finishes", "DNFs", "Fastest Laps" };
            radar.GroupLabels = new string[] { "Sebastian Vettel", "Fernando Alonso" };

            // customize the plot
            plt.Title("2010 Formula One World Championship");
            plt.Legend();

            /* Data represents the 2010 Formula One World Championship
             * https://en.wikipedia.org/wiki/2010_Formula_One_World_Championship
             * Note: Alonso did not finish (DNF) in the Malaysian GP, but was included 
             * here because he completed >90% of the race distance.
             */
        }
    }

    public class RadarWithOutlineThickness : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Radar();
        public string ID => "radar_customizePolygonOutline";
        public string Title => "Customizable polygon outline";
        public string Description => "The thickness of each radar plot's outline can be customized.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] values = {
                { 5, 3, 10, 15, 3, 2 },
                { 5, 2, 10, 10, 1, 4 },
            };

            var radar = plt.AddRadar(values);
            radar.OutlineWidth = 3;
            radar.GroupLabels = new string[] { "Sebastian Vettel", "Fernando Alonso" };
            plt.Title("2010 Formula One World Championship");
            plt.Legend();

            /* Data represents the 2010 Formula One World Championship
             * https://en.wikipedia.org/wiki/2010_Formula_One_World_Championship
             * Note: Alonso did not finish (DNF) in the Malaysian GP, but was included 
             * here because he completed >90% of the race distance.
             */
        }
    }

    public class RadarHatch : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Radar();
        public string ID => "radar_hatch";
        public string Title => "Customizable hatching (pattern)";
        public string Description => "The hatch of each radar plot can be customized";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] values = {
                { 5, 3, 10, 15, 3, 2 },
                { 5, 2, 10, 10, 1, 4 },
            };

            var radar = plt.AddRadar(values);
            radar.HatchOptions = new HatchOptions[]
            {
                new() { Pattern = HatchStyle.StripedUpwardDiagonal, Color = Color.FromArgb(100, Color.Gray) },
                new() { Pattern = HatchStyle.StripedDownwardDiagonal, Color = Color.FromArgb(100, Color.Gray) },
            };

            radar.GroupLabels = new string[] { "Sebastian Vettel", "Fernando Alonso" };
            plt.Title("2010 Formula One World Championship");
            plt.Legend();

            /* Data represents the 2010 Formula One World Championship
             * https://en.wikipedia.org/wiki/2010_Formula_One_World_Championship
             * Note: Alonso did not finish (DNF) in the Malaysian GP, but was included 
             * here because he completed >90% of the race distance.
             */
        }
    }

    public class RadarWithIcons : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Radar();
        public string ID => "radar_iconValue";
        public string Title => "Categories with images";
        public string Description => "Images can be displayed on the arms of the radar chart. " +
            "When using images, labels will be ignored so setting them will have no effect.";

        public System.Drawing.Image[] CategoryImages = new[]
{
            System.Drawing.Image.FromFile("Images/wins.png"),
            System.Drawing.Image.FromFile("Images/points.png"),
            System.Drawing.Image.FromFile("Images/did_not_finish.png"),
            System.Drawing.Image.FromFile("Images/fastest_laps.png")
        };

        public void ExecuteRecipe(Plot plt)
        {
            double[,] values = {
                { 5, 15, 3, 2 },
                { 5, 10, 1, 4 },
            };

            var radar = plt.AddRadar(values);
            radar.CategoryImages = CategoryImages;
            radar.GroupLabels = new string[] { "Sebastian Vettel", "Fernando Alonso" };

            // customize the plot
            plt.Title("2010 Formula One World Championship");
            plt.Legend();

            /* Data represents the 2010 Formula One World Championship
             * https://en.wikipedia.org/wiki/2010_Formula_One_World_Championship
             * Note: Alonso did not finish (DNF) in the Malaysian GP, but was included 
             * here because he completed >90% of the race distance.
             */
        }
    }

    public class RadarSeveralAxes : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Radar();
        public string ID => "radar_axisScaling";
        public string Title => "Independent Axis Scaling";
        public string Description =>
            "Axis scaling can be independent, allowing values for each category to be displayed using a different scale. " +
            "When independent axis mode is enabled, axis limits are automatically adjusted to fit the range of the data.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] values = { { 5, 3, 10, 15, 3, 2, 256 }, { 5, 2, 10, 10, 1, 4, 252 }, };

            var radar = plt.AddRadar(values, independentAxes: true);
            radar.CategoryLabels = new string[] { "Wins", "Poles", "Podiums", "Points Finishes", "DNFs", "Fastest Laps", "Points" };
            radar.GroupLabels = new string[] { "Sebastian Vettel", "Fernando Alonso" };

            // customize the plot
            plt.Title("2010 Formula One World Championship");
            plt.Legend();

            /* Data represents the 2010 Formula One World Championship
             * https://en.wikipedia.org/wiki/2010_Formula_One_World_Championship
             * Note: Alonso did not finish (DNF) in the Malaysian GP, but was included 
             * here because he completed >90% of the race distance.
             */
        }
    }

    public class RadarSeveralAxesWithMax : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Radar();
        public string ID => "radar_axisLimits";
        public string Title => "Defined Axis Limits";
        public string Description =>
            "Radar charts with independent axis limits use scales fitted to the data by default, " +
            "but scaling can be controlled by defining the maximum value for each axis.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] values = {
                { 5, 3, 10, 15, 3, 2, 256 },
                { 5, 2, 10, 10, 1, 4, 252 },
            };

            double[] maxValues = { 13, 15, 17, 15, 10, 10, 413 };

            var radar = plt.AddRadar(values, independentAxes: true, maxValues: maxValues);
            radar.CategoryLabels = new string[] { "Wins", "Poles", "Podiums", "Points Finishes", "DNFs", "Fastest Laps", "Points" };
            radar.GroupLabels = new string[] { "Sebastian Vettel", "Fernando Alonso" };

            // customize the plot
            plt.Title("2010 Formula One World Championship");
            plt.Legend();

            /* Data represents the 2010 Formula One World Championship
             * https://en.wikipedia.org/wiki/2010_Formula_One_World_Championship
             * Note: Alonso did not finish (DNF) in the Malaysian GP, but was included 
             * here because he completed >90% of the race distance.
             *
             * Max values are based on https://en.wikipedia.org/wiki/List_of_Formula_One_World_Drivers%27_Champions.
             */
        }
    }

    public class RadarCustomization : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Radar();
        public string ID => "radar_customization";
        public string Title => "Customization";
        public string Description =>
            "Radar charts support customization of the line color and width.";

        public void ExecuteRecipe(Plot plt)
        {
            double[,] values = {
                { 78,  83, 84, 76, 43 },
                { 100, 50, 70, 60, 90 }
            };

            var radar = plt.AddRadar(values);
            radar.WebColor = System.Drawing.Color.Red;
            radar.LineWidth = 3;
        }
    }
}
