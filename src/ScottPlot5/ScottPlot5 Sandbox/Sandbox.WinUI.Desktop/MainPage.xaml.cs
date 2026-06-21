using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ScottPlot;
using System;
using System.Diagnostics;

namespace Sandbox.WinUI;

/// <summary>
/// Main page demonstrating ScottPlot WinUI control
/// </summary>
public sealed partial class MainPage : Page
{
    private double _currentDpiScale = 1.0;

    public MainPage()
    {
        InitializeComponent();
        WinUIPlot.AppWindow = App.MainWindow;

        WinUIPlot.UserInputProcessor.IsEnabled = true;

        WinUIPlot.Plot.Add.Signal(Generate.Sin());
        WinUIPlot.Plot.Add.Signal(Generate.Cos());

        this.Loaded += OnPageLoaded;
    }

    /// <summary>
    /// Initialize scale controls when page loads
    /// </summary>
    private void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        if (XamlRoot != null)
        {
            XamlRoot.Changed += OnXamlRootChanged;
        }

        if (WinUIPlot.XamlRoot != null)
        {
            var dpiScale = WinUIPlot.XamlRoot.RasterizationScale;

            // Initialize NumberBox with current user scale ratio as percentage
            var currentUserScaleRatio = WinUIPlot.Plot.ScaleFactor / dpiScale;
            ScaleValueBox.Value = currentUserScaleRatio * 100.0;

            Debug.WriteLine($"=== Initial DPI ===");
            Debug.WriteLine($"RasterizationScale: {dpiScale}");
            Debug.WriteLine($"ScaleFactor: {WinUIPlot.Plot.ScaleFactor}");
            Debug.WriteLine($"User Scale: {currentUserScaleRatio:P0}");
        }
    }

    /// <summary>
    /// Handle DPI changes when moving between monitors
    /// </summary>
    private void OnXamlRootChanged(XamlRoot sender, XamlRootChangedEventArgs args)
    {
        var newDpi = sender.RasterizationScale;
        Debug.WriteLine($"=== DPI Change Detected ===");
        Debug.WriteLine($"New RasterizationScale: {newDpi}");
        Debug.WriteLine($"New ScaleFactor: {WinUIPlot.Plot.ScaleFactor}");
    }

    /// <summary>
    /// Handle user scale adjustment via NumberBox (percentage input)
    /// </summary>
    private void ScaleValueBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        if (WinUIPlot is null || XamlRoot is null)
            return;

        // Convert percentage to ratio (100% = 1.0, 200% = 2.0)
        var percentageValue = args.NewValue;
        var userScaleRatio = percentageValue / 100.0;

        // Clamp between 80% and 200%
        userScaleRatio = Math.Max(0.8, Math.Min(2.0, userScaleRatio));

        var dpi = XamlRoot.RasterizationScale;
        WinUIPlot.Plot.ScaleFactor = dpi * userScaleRatio;
        WinUIPlot.Refresh();

        Debug.WriteLine($"DPI: {dpi}, User Scale: {userScaleRatio:P0}, New ScaleFactor: {WinUIPlot.Plot.ScaleFactor}");
    }
}
