using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using ScottPlot;
using System.Drawing;
using System.IO;

namespace Avalonia_Demo.Demos;

public class LegendOutsideBitmapDemo : IDemo
{
    public string Title => "Legend Outside the Plot (Bitmap)";

    public string Description => "Demonstrates how to display the " +
        "legend outside the plot by obtaining it as a Bitmap and " +
        "displaying it outside the plot control anywhere in your window.";

    public Window GetWindow()
    {
        return new LegendOutsideBitmapWindow();
    }
}

public partial class LegendOutsideBitmapWindow : Window
{
    public LegendOutsideBitmapWindow()
    {
        InitializeComponent();


        int count = 20;
        for (int i = 0; i < 20; i++)
        {
            double[] ys = Generate.Sin(100, phase: i / (2.0 * count));
            var sig = AvaPlot.Plot.Add.Signal(ys);
            sig.Color = Colors.Category20[i];
            sig.LineWidth = 2;
            sig.LegendText = $"Line #{i + 1}";
        }

        AvaPlot.Plot.Legend.OutlineWidth = 0;
        AvaPlot.Plot.Legend.BackgroundColor = ScottPlot.Color.FromColor(SystemColors.Control);
        ScottPlot.Image legendImage = AvaPlot.Plot.GetLegendImage();
        byte[] legendBitmapBytes = legendImage.GetImageBytes(ImageFormat.Bmp);
        MemoryStream ms = new(legendBitmapBytes);
        Bitmap bmp = new(ms);

        LegendImage.Source = bmp;
        LegendImage.Stretch = Avalonia.Media.Stretch.None;
    }
}
