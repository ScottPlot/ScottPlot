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

        // load custom font files, giving unique names to fonts with special styling (beyond bold and italic)
        Fonts.AddFontFile("Alumni Sans", "Fonts/AlumniSans/AlumniSans-Regular.ttf", bold: false, italic: false);
        Fonts.AddFontFile("Alumni Sans", "Fonts/AlumniSans/AlumniSans-Bold.ttf", bold: true, italic: false);
        Fonts.AddFontFile("Alumni Sans", "Fonts/AlumniSans/AlumniSans-BoldItalic.ttf", bold: true, italic: true);
        Fonts.AddFontFile("Alumni Sans", "Fonts/AlumniSans/AlumniSans-Italic.ttf", bold: false, italic: true);
        Fonts.AddFontFile("Noto Serif Display", "Fonts/NotoSerifDisplay/NotoSerifDisplay-Regular.ttf", bold: true, italic: false);
        Fonts.AddFontFile("Noto Serif Display", "Fonts/NotoSerifDisplay/NotoSerifDisplay-Bold.ttf", bold: true, italic: false);
        Fonts.AddFontFile("Noto Serif Display", "Fonts/NotoSerifDisplay/NotoSerifDisplay-BoldItalic.ttf", bold: true, italic: false);
        Fonts.AddFontFile("Noto Serif Display", "Fonts/NotoSerifDisplay/NotoSerifDisplay-BoldItalic.ttf", bold: true, italic: false);
        Fonts.AddFontFile("Noto Serif Display", "Fonts/NotoSerifDisplay/NotoSerifDisplay-Italic.ttf", bold: true, italic: false);
        Fonts.AddFontFile("Noto Serif Display Light", "Fonts/NotoSerifDisplay/NotoSerifDisplay-Light.ttf", bold: false, italic: false);

        // plot sample data
        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());

        // Demonstrate how to customize label fonts
        formsPlot1.Plot.Axes.Title.IsVisible = true;
        formsPlot1.Plot.Axes.Title.Label.Text = "Alumni Sans (Bold Italic)";
        formsPlot1.Plot.Axes.Title.Label.FontName = "Alumni Sans";
        formsPlot1.Plot.Axes.Title.Label.Bold = true;
        formsPlot1.Plot.Axes.Title.Label.Italic = true;
        formsPlot1.Plot.Axes.Title.Label.FontSize = 36;

        formsPlot1.Plot.Axes.Bottom.Label.Text = "Noto Serif Display (Light)";
        formsPlot1.Plot.Axes.Bottom.Label.FontName = "Noto Serif Display Light";
        formsPlot1.Plot.Axes.Bottom.Label.Bold = false;
        formsPlot1.Plot.Axes.Bottom.Label.Italic = false;
        formsPlot1.Plot.Axes.Bottom.Label.FontSize = 24;
        formsPlot1.Plot.Axes.Bottom.TickLabelStyle.FontSize = 18;

        formsPlot1.Plot.Axes.Left.Label.Text = "Alumni Sans (Regular)";
        formsPlot1.Plot.Axes.Left.Label.FontName = "Alumni Sans";
        formsPlot1.Plot.Axes.Left.Label.Bold = false;
        formsPlot1.Plot.Axes.Left.Label.Italic = false;
        formsPlot1.Plot.Axes.Left.Label.FontSize = 24;
        formsPlot1.Plot.Axes.Left.TickLabelStyle.FontSize = 18;
    }
}
