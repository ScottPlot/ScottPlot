using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class RadialGaugeQuickstart : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_quickstart";
        public string Title => "Radial Gauge";
        public string Description =>
            "A radial gauge chart displays scalar data as circular gauges. ";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };
            plt.AddRadialGauge(values);
        }
    }

    public class RadialGaugeNegative : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_negative";
        public string Title => "Negative Values";
        public string Description =>
            "It works best with positive data, " +
            "but it also plots negative values.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, -20 };
            plt.AddRadialGauge(values);
        }
    }

    public class RadialGaugeSequential : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_mode";
        public string Title => "Sequential Gauge Mode";
        public string Description =>
            "The gauges can be plotted in three different modes: " +
            "stacked (default), sequential, and as a single gauge.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.GaugeMode = ScottPlot.RadialGaugeMode.Sequential;
        }
    }

    public class RadialGaugeModeSingle : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_single";
        public string Title => "Single Gauge Mode";
        public string Description =>
            "The SingleGauge mode draws ones single gauge with all values stacked together. " +
            "This is useful for showing a progress-type plot.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.GaugeMode = ScottPlot.RadialGaugeMode.SingleGauge;
            RadialGauge.AngleRange = 180;
            RadialGauge.StartingAngleGauges = 180;
        }
    }

    public class RadialGaugeDirection : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_direction";
        public string Title => "Gauge Direction";
        public string Description =>
            "The gauges can be plotted either clockwise (default) or anti-clockwise.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.GaugeDirection = ScottPlot.RadialGaugeDirection.AntiClockwise;
        }
    }

    public class RadialGaugeSize : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_size";
        public string Title => "Gauge Size";
        public string Description =>
            "The empty space between gauges can be adjusted as a percentage of the gauges' width. " +
            "A value of 100 means that both widths are equal whereas a value of 50 (default) means that space is half the width of the gauges'.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.GaugeSpacePercentage = 20;
        }
    }

    public class RadialGaugeCaps : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_caps";
        public string Title => "Gauge Caps";
        public string Description =>
            "Caps can be set for both the starting and ending points. " +
            "Accepts values from System.Drawing.Drawing2D.LineCap enum. " +
            "Default values are Round for the starting point and Triangle for the ending point.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.NormBackGauge = true;
            RadialGauge.StartCap = System.Drawing.Drawing2D.LineCap.Flat;
            RadialGauge.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;
        }
    }

    public class RadialGaugeStart : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_start";
        public string Title => "Gauge Starting Angle";
        public string Description =>
            "Angle (in degrees) at which the gauges start: 270 for North (default value), 0 for East, 90 for South, 180 for West, and so on. " +
            "Expected values in the range [0-360]";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.StartingAngleGauges = 180;
        }
    }

    public class RadialGaugeRange : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_range";
        public string Title => "Gauge Angular Range";
        public string Description =>
            "The maximum angular interval that the gauges will consist of. " +
            "It takes values in the range [0-360], default value is 360 (full circle).";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.AngleRange = 180;
        }
    }

    public class RadialGaugeLabels : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_labels";
        public string Title => "Gauge Labels";
        public string Description =>
            "By default, labels are drawn over the gauges.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.ShowGaugeValues = false;
        }
    }

    public class RadialGaugeLegend : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_legend";
        public string Title => "Gauge Labels in Legend";
        public string Description =>
            "Radial gauge labels will appear in the legend if they are assigned.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };
            var rg = plt.AddRadialGauge(values);
            rg.GaugeLabels = new string[] { "alpha", "beta", "gamma", "delta", "epsilon" };
            plt.Legend(true);
        }
    }

    public class RadialGaugeLabelPos : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_labelpos";
        public string Title => "Gauge Label Position";
        public string Description =>
            "Gauge labels are positioned at the tip of each gauge by default, " +
            "but this position can be adjusted by the user.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.GaugeLabelPosition = .5;
        }
    }

    public class RadialGaugeLabelFontPct : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_labelfontpct";
        public string Title => "Gauge Label Font Percentage";
        public string Description =>
            "Size of the gague label text as a fraction of the gauge width.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.FontSizeFraction = .5;
        }
    }

    public class RadialGaugeLabelColor : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_labelcolor";
        public string Title => "Gauge Label Color";
        public string Description =>
            "Color of the labels.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.GaugeLabelsColor = Color.Yellow;
        }
    }

    public class RadialGaugeBackDim : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_backdim";
        public string Title => "Background Gauges Dim";
        public string Description =>
            "Dimmed percentage used to draw the gauges' background. Values in the range [0-100], default value is 90 [percent]. " +
            "A value of 100 renders invisible the background gauges.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.ColorDimFraction = .5;
        }
    }

    public class RadialGaugeBackNorm : IRecipe
    {
        public string Category => "Plottable: RadialGauge";
        public string ID => "radialgauge_backnorm";
        public string Title => "Background Gauges Normalization";
        public string Description =>
            "The gauges' background are drawn as full circles by default. If normalization is set to true, background gauges are limited to the values of StartingAngle and AngleRange.";

        public void ExecuteRecipe(Plot plt)
        {
            plt.Palette = ScottPlot.Drawing.Palette.Nord;
            double[] values = { 100, 80, 65, 45, 20 };

            var RadialGauge = plt.AddRadialGauge(values);
            RadialGauge.NormBackGauge = true;
            RadialGauge.AngleRange = 180;
        }
    }
}
