using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia_Demo.Demos;
using Avalonia_Demo.ViewModels.Demos;
using ScottPlot;
using System;
using System.ComponentModel;

namespace Avalonia_Demo.Demos;

public class AxisRulesDemo : IDemo
{
    public string Title => "Axis Rules";
    public string Description => "Configure rules that limit how far the user " +
        "can zoom in or out or enforce equal axis scaling";

    public Window GetWindow()
    {
        return new AxisRulesWindow();
    }
}

public partial class AxisRulesWindow : Window
{
    private AxisRulesViewModel TypedDataContext => (DataContext as AxisRulesViewModel) ?? throw new ArgumentException(nameof(DataContext));
    public AxisRulesWindow()
    {
        InitializeComponent();

        DataContext = new AxisRulesViewModel();

        TypedDataContext.PropertyChanged += HandleDataContextChanged;

        PlotRandomData();
        AvaPlot.Plot.Title("No axis rules are in effect");
    }

    private void HandleDataContextChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(TypedDataContext.Selected))
        {
            OnAxisRuleSelected();
        }
        else if (args.PropertyName == nameof(TypedDataContext.InvertX) || args.PropertyName == nameof(TypedDataContext.InvertY))
        {
            AutoScaleAxes();
        }
    }

    private void OnAxisRuleSelected()
    {
        if (!TypedDataContext.Selected.HasValue)
        {
            AvaPlot.Plot.Axes.Rules.Clear();
            AvaPlot.Plot.Title("No axis rules are in effect");
            AutoScaleAxes();
            AvaPlot.Refresh();
            return;
        }

        (string category, string option) = TypedDataContext.Selected.Value;

        (IAxisRule rule, string plotTitle) = GetAxisRule(category, option);

        AvaPlot.Plot.Axes.Rules.Clear();
        AvaPlot.Plot.Axes.Rules.Add(rule);
        AvaPlot.Plot.Title(plotTitle);
        AvaPlot.Refresh();
    }

    private void AutoScaleAxes()
    {
        AvaPlot.Plot.Axes.AutoScale(invertX: TypedDataContext.InvertX, invertY: TypedDataContext.InvertY);
        AvaPlot.Refresh();
    }

    private (IAxisRule rule, string plotTitle) GetAxisRule(string category, string option)
    {
        if (category == "Boundary")
        {
            if (option == "Minimum")
            {
                ScottPlot.AxisRules.MinimumBoundary rule = new(
                    xAxis: AvaPlot.Plot.Axes.Bottom,
                    yAxis: AvaPlot.Plot.Axes.Left,
                    limits: new AxisLimits(0, 1, 0, 1));

                return (rule, "Area inside the boundary is always in view");
            }
            else if (option == "Maximum")
            {
                ScottPlot.AxisRules.MaximumBoundary rule = new(
                    xAxis: AvaPlot.Plot.Axes.Bottom,
                    yAxis: AvaPlot.Plot.Axes.Left,
                    limits: new AxisLimits(0, 1, 0, 1));

                return (rule, "Cannot view area outside the boundary");
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(option));
            }
        }
        else if (category == "Square Scaling")
        {
            if (option == "Preserve X")
            {
                ScottPlot.AxisRules.SquarePreserveX rule = new(
                    xAxis: AvaPlot.Plot.Axes.Bottom,
                    yAxis: AvaPlot.Plot.Axes.Left);

                return (rule, "Automatically adjust Y so coodinates are square");
            }
            else if (option == "Preserve Y")
            {
                ScottPlot.AxisRules.SquarePreserveY rule = new(
                    xAxis: AvaPlot.Plot.Axes.Bottom,
                    yAxis: AvaPlot.Plot.Axes.Left);

                return (rule, "Automatically adjust X so coodinates are square");
            }
            else if (option == "Zoom Out")
            {
                ScottPlot.AxisRules.SquareZoomOut rule = new(
                    xAxis: AvaPlot.Plot.Axes.Bottom,
                    yAxis: AvaPlot.Plot.Axes.Left);

                return (rule, "Automatically adjust the most zoomed-in axis so coordinates are square");
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(option));
            }
        }
        else if (category == "Span Limit")
        {
            if (option == "Minimum")
            {
                ScottPlot.AxisRules.MinimumSpan rule = new(
                    xAxis: AvaPlot.Plot.Axes.Bottom,
                    yAxis: AvaPlot.Plot.Axes.Left,
                    xSpan: 1,
                    ySpan: 1);

                return (rule, "Cannot zoom in beyond an axis span of 1");
            }
            else if (option == "Maximum")
            {
                ScottPlot.AxisRules.MaximumSpan rule = new(
                    xAxis: AvaPlot.Plot.Axes.Bottom,
                    yAxis: AvaPlot.Plot.Axes.Left,
                    xSpan: 1,
                    ySpan: 1);

                return (rule, "Cannot zoom out beyond an axis span of 1");
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(option));
            }
        }
        else if (category == "Axis Lock")
        {
            if (option == "Horizontal")
            {
                AxisLimits limits = AvaPlot.Plot.Axes.GetLimits();
                ScottPlot.AxisRules.LockedHorizontal rule = new(AvaPlot.Plot.Axes.Bottom, limits.Left, limits.Right);

                return (rule, "Horizontal axis is locked");
            }
            else if (option == "Vertical")
            {
                AxisLimits limits = AvaPlot.Plot.Axes.GetLimits();
                ScottPlot.AxisRules.LockedVertical rule = new(AvaPlot.Plot.Axes.Left, limits.Left, limits.Right);


                return (rule, "Vertical axis is locked");
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(option));
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(category));
        }
    }

    private void PlotRandomData()
    {
        AvaPlot.Plot.Clear();

        // generate data that fits between (0, 0) and (1, 1)
        int pointCount = 500;
        double[] xs = Generate.Consecutive(pointCount, delta: 1.0 / pointCount);
        double[] ys = Generate.Sin(pointCount, oscillations: 0.37);
        Generate.AddNoiseInPlace(ys, 0.1);

        var sp = AvaPlot.Plot.Add.Scatter(xs, ys);
        sp.LineWidth = 0;
        sp.MarkerStyle.Size = 5;
        sp.Color = Colors.Magenta;

        var rect = AvaPlot.Plot.Add.Rectangle(0, 1, 0, 1);
        rect.FillStyle.Color = Colors.Transparent;
        rect.LineStyle.Color = Colors.Green;
        rect.LineStyle.Width = 3;
        rect.LineStyle.IsVisible = true;
    }
}
