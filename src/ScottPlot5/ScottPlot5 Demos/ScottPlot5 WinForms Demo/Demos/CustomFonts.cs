using ScottPlot;
using SkiaSharp;

namespace WinForms_Demo.Demos;

public partial class CustomFonts : Form, IDemoWindow
{
    public string Title => "Custom Fonts";

    public string Description => "Demonstrates how to create plots that " +
        "render text using fonts defined in an external TTF file.";

    public CustomFonts()
    {
        InitializeComponent();

        Fonts.FontResolver = new ScottPlot.FontResolvers.FileFontResolver("Alumni Sans", "Fonts/AlumniSans/AlumniSans");

        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());

        formsPlot1.Plot.Font.Set("Alumni Sans");

        formsPlot1.Plot.Title("Plot Using Custom Fonts [Bold Italic]");
        formsPlot1.Plot.Axes.Title.Label.Bold = true;
        formsPlot1.Plot.Axes.Title.Label.Italic = true;
        formsPlot1.Plot.Axes.Title.Label.FontSize = 36;

        formsPlot1.Plot.XLabel("Horizontal Axis [Italic]");
        formsPlot1.Plot.Axes.Bottom.Label.Bold = false;
        formsPlot1.Plot.Axes.Bottom.Label.Italic = true;
        formsPlot1.Plot.Axes.Bottom.Label.FontSize = 24;
        formsPlot1.Plot.Axes.Bottom.TickLabelStyle.FontSize = 18;

        formsPlot1.Plot.YLabel("Vertical Axis [Bold]");
        formsPlot1.Plot.Axes.Left.Label.Bold = true;
        formsPlot1.Plot.Axes.Left.Label.Italic = false;
        formsPlot1.Plot.Axes.Left.Label.FontSize = 24;
        formsPlot1.Plot.Axes.Left.TickLabelStyle.FontSize = 18;
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        formsPlot1.Plot.Font.Set(Fonts.Sans);
        base.OnFormClosed(e);
    }
}
