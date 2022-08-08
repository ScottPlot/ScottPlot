using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class RadialGaugeQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_quickstart";
        public string Title => "Radial Gauge";
        public string Description =>
            "A radial gauge chart displays scalar data as circular gauges. ";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 100, 80, 65, 45, 20 };
            plt.AddRadialGauge(values);
        }
    }

    public class RadialGaugeColormap : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_colors";
        public string Title => "Gauge Colors";
        public string Description =>
            "Gauge colors can be customized by changing the default palette. ";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 100, 80, 65, 45, 20 };
            plt.Palette = ScottPlot.Palette.Nord;
            plt.AddRadialGauge(values);
        }
    }

    public class RadialGaugeNegative : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_negative";
        public string Title => "Negative Values";
        public string Description =>
            "Radial gauge plots support positive and negative values.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, -65, 45, -20 };
            plt.AddRadialGauge(values);
        }
    }

    public class RadialGaugeSequential : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_mode";
        public string Title => "Sequential Gauge Mode";
        public string Description =>
            "Sequential gauge mode indicates that the base of each gauge starts " +
            "at the tip of the previous gauge.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 50 };

            var gauges = plt.AddRadialGauge(values);
            gauges.GaugeMode = ScottPlot.RadialGaugeMode.Sequential;
        }
    }

    public class RadialGaugeReverse : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_reverse";
        public string Title => "Reverse Order";
        public string Description =>
            "Gauges are displayed from the center outward by default but the order can be customized.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 50 };

            var gauges = plt.AddRadialGauge(values);
            gauges.GaugeMode = ScottPlot.RadialGaugeMode.Sequential;
            gauges.OrderInsideOut = false;
        }
    }

    public class RadialGaugeModeSingle : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_single";
        public string Title => "Single Gauge Mode";
        public string Description =>
            "The SingleGauge mode draws all gauges stacked together as a single gauge. " +
            "This is useful for showing a progress gauges composed of many individual smaller gauges.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45 };

            var gauges = plt.AddRadialGauge(values);
            gauges.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            gauges.MaximumAngle = 180;
            gauges.StartingAngle = 180;
        }
    }

    public class RadialGaugeDirection : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_direction";
        public string Title => "Gauge Direction";
        public string Description =>
            "The direction of gauges can be customized. Clockwise is used by default.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var gauges = plt.AddRadialGauge(values);
            gauges.Clockwise = false;
        }
    }

    public class RadialGaugeSize : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_size";
        public string Title => "Gauge Size";
        public string Description =>
            "The empty space between gauges can be adjusted as a fraction of their width. ";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };
            var gauges = plt.AddRadialGauge(values);
            gauges.SpaceFraction = .05;
        }
    }

    public class RadialGaugeCaps : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_caps";
        public string Title => "Gauge Caps";
        public string Description =>
            "Caps can be customized for the starting and end of the gauges. ";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var gauges = plt.AddRadialGauge(values);
            gauges.CircularBackground = false;
            gauges.StartCap = System.Drawing.Drawing2D.LineCap.Flat;
            gauges.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;
        }
    }

    public class RadialGaugeStart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_start";
        public string Title => "Gauge Starting Angle";
        public string Description =>
            "The starting angle for gauges can be customized. " +
            "270 for North (default value), 0 for East, 90 for South, 180 for West, etc.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var gauges = plt.AddRadialGauge(values);
            gauges.StartingAngle = 180;
        }
    }

    public class RadialGaugeRange : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_range";
        public string Title => "Gauge Angular Range";
        public string Description =>
            "By default gauges are full circles (360 degrees) but smaller gauges can be created " +
            "by customizing the gauge size.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var gauges = plt.AddRadialGauge(values);
            gauges.MaximumAngle = 180;
        }
    }

    public class RadialGaugeLabels : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_levels";
        public string Title => "Show Levels";
        public string Description =>
            "The value of each gauge is displayed as text by default but this behavior can be overridden. " +
            "Note that this is different than the labels fiels which is what appears in the legened.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var gauges = plt.AddRadialGauge(values);
            gauges.ShowLevels = false;
        }
    }

    public class RadialGaugeLabelPos : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_labelpos";
        public string Title => "Gauge Label Position";
        public string Description =>
            "Gauge level text is positioned at the tip of each gauge by default, " +
            "but this position can be adjusted by the user.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var gauges = plt.AddRadialGauge(values);
            gauges.LabelPositionFraction = 0;
        }
    }

    public class RadialGaugeLabelFontPct : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_labelfontsize";
        public string Title => "Gauge Label Font Percentage";
        public string Description =>
            "Size of the gauge level text as a fraction of the gauge width.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var gauges = plt.AddRadialGauge(values);
            gauges.FontSizeFraction = .4;
        }
    }

    public class RadialGaugeLabelColor : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_labelcolor";
        public string Title => "Gauge Label Color";
        public string Description =>
            "Level text fonts may be customized.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var gauges = plt.AddRadialGauge(values);
            gauges.Font.Color = Color.Black;
        }
    }

    public class RadialGaugeLegend : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_legend";
        public string Title => "Gauge Labels in Legend";
        public string Description =>
            "Radial gauge labels will appear in the legend if they are assigned. ";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var gauges = plt.AddRadialGauge(values);
            gauges.Labels = new string[] { "alpha", "beta", "gamma", "delta", "epsilon" };
            plt.Legend(true);
        }
    }

    public class RadialGaugeBackDim : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_backdim";
        public string Title => "Background Gauges Dim";
        public string Description =>
            "By default the full range of each gauge is drawn as a semitransparent ring. " +
            "The amount of transparency can be adjusted as desired.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var gauges = plt.AddRadialGauge(values);
            gauges.BackgroundTransparencyFraction = .5;
        }
    }

    public class RadialGaugeBackNorm : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.RadialGauge();
        public string ID => "radialgauge_backnorm";
        public string Title => "Background Gauges Normalization";
        public string Description =>
            "Gauge backgrounds are drawn as full circles by default. " +
            "This behavior can be disabled to draw partial backgrounds for non-circular gauges.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var gauges = plt.AddRadialGauge(values);
            gauges.CircularBackground = false;
            gauges.MaximumAngle = 180;
            gauges.StartingAngle = 180;
        }
    }
}
