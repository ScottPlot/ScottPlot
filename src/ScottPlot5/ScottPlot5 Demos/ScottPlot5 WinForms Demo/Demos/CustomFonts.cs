using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class CustomFonts : Form, IDemoWindow
{
    public string Title => "Custom Fonts";

    public string Description => "Demonstrates how to create plots that " +
        "render text using fonts defined in an external TTF file.";

    public CustomFonts()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());

        //string fontPath = "Fonts/ShadowsIntoLight/ShadowsIntoLight-Regular.ttf";

        formsPlot1.Plot.Title("Plot Using Custom Fonts");
        // formsPlot1.Plot.Axes.Title.Label.FontFile = fontPath;
        formsPlot1.Plot.Axes.Title.Label.FontSize = 36;

        formsPlot1.Plot.XLabel("Horizontal Axis");
        // formsPlot1.Plot.Axes.Bottom.Label.FontFile = fontPath;
        formsPlot1.Plot.Axes.Bottom.Label.FontSize = 24;
        // formsPlot1.Plot.Axes.Bottom.TickLabelStyle.FontFile = fontPath;
        formsPlot1.Plot.Axes.Bottom.TickLabelStyle.FontSize = 18;

        formsPlot1.Plot.YLabel("Vertical Axis");
        // formsPlot1.Plot.Axes.Left.Label.FontFile = fontPath;
        formsPlot1.Plot.Axes.Left.Label.FontSize = 24;
        // formsPlot1.Plot.Axes.Left.TickLabelStyle.FontFile = fontPath;
        formsPlot1.Plot.Axes.Left.TickLabelStyle.FontSize = 18;
    }
}
