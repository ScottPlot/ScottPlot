﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class PieQuickstart : IRecipe
    {
        public string Category => "Plottable: Pie";
        public string ID => "pie_quickstart";
        public string Title => "Pie Chart";
        public string Description =>
            "A pie chart illustrates numerical proportions as slices of a circle.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 778, 283, 184, 76, 43 };
            plt.AddPie(values);
        }
    }

    public class PieExploded : IRecipe
    {
        public string Category => "Plottable: Pie";
        public string ID => "pie_exploded";
        public string Title => "Exploded Pie Chart";
        public string Description =>
            "Exploded pie charts have a bit of space between their slices.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 778, 283, 184, 76, 43 };
            var pie = plt.AddPie(values);
            pie.Explode = true;
        }
    }

    public class PieDonut : IRecipe
    {
        public string Category => "Plottable: Pie";
        public string ID => "pie_donut";
        public string Title => "Donut Chart";
        public string Description =>
            "Donut plots are pie charts with a hollow center.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 778, 283, 184, 76, 43 };
            var pie = plt.AddPie(values);
            pie.Explode = true;
            pie.DonutSize = .6;
        }
    }

    public class PieDonutText : IRecipe
    {
        public string Category => "Plottable: Pie";
        public string ID => "pie_donutText";
        public string Title => "Donut with Text";
        public string Description =>
            "Custom text can be displayed in the center of a donut chart. " +
            "Notice too how the colors of each slice are customized in this example.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 779, 586 };
            string centerText = $"{values[0] / values.Sum() * 100:00.0}%";
            Color color1 = Color.FromArgb(255, 0, 150, 200);
            Color color2 = Color.FromArgb(100, 0, 150, 200);

            var pie = plt.AddPie(values);
            pie.DonutSize = .6;
            pie.DonutLabel = centerText;
            pie.CenterFont.Color = color1;
            pie.OutlineSize = 2;
            pie.SliceFillColors = new Color[] { color1, color2 };
        }
    }

    public class PieShowValues : IRecipe
    {
        public string Category => "Plottable: Pie";
        public string ID => "pie_showValues";
        public string Title => "Slice Values";
        public string Description =>
            "The value of each slice can be displayed at its center.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 778, 43, 283, 76, 184 };
            var pie = plt.AddPie(values);
            pie.ShowValues = true;
        }
    }

    public class PieShowPercentage : IRecipe
    {
        public string Category => "Plottable: Pie";
        public string ID => "pie_showPercentage";
        public string Title => "Slice Percentages";
        public string Description =>
            "The percentage of each slice can be displayed at its center.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 778, 43, 283, 76, 184 };
            var pie = plt.AddPie(values);
            pie.ShowPercentages = true;
        }
    }

    public class PieCustomColors : IRecipe
    {
        public string Category => "Plottable: Pie";
        public string ID => "pie_customColors";
        public string Title => "Customize Pie Colors";
        public string Description =>
            "Colors for pie slices and labels can be customized.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 778, 43, 283, 76, 184 };
            string[] labels = { "C#", "JAVA", "Python", "F#", "PHP" };

            // Language colors from https://github.com/ozh/github-colors
            Color[] sliceColors =
            {
                ColorTranslator.FromHtml("#178600"),
                ColorTranslator.FromHtml("#B07219"),
                ColorTranslator.FromHtml("#3572A5"),
                ColorTranslator.FromHtml("#B845FC"),
                ColorTranslator.FromHtml("#4F5D95"),
            };

            // Show labels using different transparencies
            Color[] labelColors =
                new Color[] {
                Color.FromArgb(255, Color.White),
                Color.FromArgb(100, Color.White),
                Color.FromArgb(250, Color.White),
                Color.FromArgb(150, Color.White),
                Color.FromArgb(200, Color.White),
            };

            var pie = plt.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowLabels = true;
            pie.SliceFillColors = sliceColors;
            pie.SliceLabelColors = labelColors;
        }
    }

    public class PieLegend : IRecipe
    {
        public string Category => "Plottable: Pie";
        public string ID => "pie_legend";
        public string Title => "Slices in Legend";
        public string Description =>
            "Slices can be labeled in the legend.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 778, 43, 283, 76, 184 };
            string[] labels = { "C#", "JAVA", "Python", "F#", "PHP" };
            var pie = plt.AddPie(values);
            pie.SliceLabels = labels;
            plt.Legend();
        }
    }

    public class PieShowEverything : IRecipe
    {
        public string Category => "Plottable: Pie";
        public string ID => "pie_showEverything";
        public string Title => "Label Everything";
        public string Description =>
            "Slices can labeled with values, percentages, and lables, with a legend.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 778, 43, 283, 76, 184 };
            string[] labels = { "C#", "JAVA", "Python", "F#", "PHP" };
            var pie = plt.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowPercentages = true;
            pie.ShowValues = true;
            pie.ShowLabels = true;
            plt.Legend();
        }
    }

    public class PieCustom : IRecipe
    {
        public string Category => "Plottable: Pie";
        public string ID => "pie_customLabels";
        public string Title => "Custom Slice Labels";
        public string Description =>
            "Custom slice labels can be used to display values using custom formats";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 778, 43, 283, 76, 184 };
            string[] labels = { "C#", "JAVA", "Python", "F#", "PHP" };

            // modify labels to include a custom formatted value
            labels = Enumerable.Range(0, values.Length)
                               .Select(i => $"{labels[i]}\n({values[i]})")
                               .ToArray();

            var pie = plt.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowLabels = true;
        }
    }
}
