using Avalonia.Controls;
using Avalonia.Input;
using ScottPlot;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.Demos;

public class CustomFontsDemo : IDemo
{
    public string Title => "Custom Fonts";

    public string Description => "Demonstrates how to create plots that " +
        "render text using fonts defined in an external TTF file.";

    public Window GetWindow()
    {
        return new CustomFontsWindow();
    }

}

public class CustomFontsWindow : SimpleDemoWindow
{
    public CustomFontsWindow() : base("Custom Fonts")
    {

    }

    protected override void StartDemo()
    {
        // load custom font files, giving unique names to fonts with special styling (beyond bold and italic)
        Fonts.AddFontFile("Alumni Sans", "Assets/Fonts/AlumniSans/AlumniSans-Regular.ttf", bold: false, italic: false);
        Fonts.AddFontFile("Alumni Sans", "Assets/Fonts/AlumniSans/AlumniSans-Bold.ttf", bold: true, italic: false);
        Fonts.AddFontFile("Alumni Sans", "Assets/Fonts/AlumniSans/AlumniSans-BoldItalic.ttf", bold: true, italic: true);
        Fonts.AddFontFile("Alumni Sans", "Assets/Fonts/AlumniSans/AlumniSans-Italic.ttf", bold: false, italic: true);
        Fonts.AddFontFile("Noto Serif Display", "Assets/Fonts/NotoSerifDisplay/NotoSerifDisplay-Regular.ttf", bold: true, italic: false);
        Fonts.AddFontFile("Noto Serif Display", "Assets/Fonts/NotoSerifDisplay/NotoSerifDisplay-Bold.ttf", bold: true, italic: false);
        Fonts.AddFontFile("Noto Serif Display", "Assets/Fonts/NotoSerifDisplay/NotoSerifDisplay-BoldItalic.ttf", bold: true, italic: false);
        Fonts.AddFontFile("Noto Serif Display", "Assets/Fonts/NotoSerifDisplay/NotoSerifDisplay-BoldItalic.ttf", bold: true, italic: false);
        Fonts.AddFontFile("Noto Serif Display", "Assets/Fonts/NotoSerifDisplay/NotoSerifDisplay-Italic.ttf", bold: true, italic: false);
        Fonts.AddFontFile("Noto Serif Display Light", "Assets/Fonts/NotoSerifDisplay/NotoSerifDisplay-Light.ttf", bold: false, italic: false);

        // plot sample data
        AvaPlot.Plot.Add.Signal(Generate.Sin());
        AvaPlot.Plot.Add.Signal(Generate.Cos());

        // Demonstrate how to customize label fonts
        AvaPlot.Plot.Axes.Title.IsVisible = true;
        AvaPlot.Plot.Axes.Title.Label.Text = "Alumni Sans (Bold Italic)";
        AvaPlot.Plot.Axes.Title.Label.FontName = "Alumni Sans";
        AvaPlot.Plot.Axes.Title.Label.Bold = true;
        AvaPlot.Plot.Axes.Title.Label.Italic = true;
        AvaPlot.Plot.Axes.Title.Label.FontSize = 36;

        AvaPlot.Plot.Axes.Bottom.Label.Text = "Noto Serif Display (Light)";
        AvaPlot.Plot.Axes.Bottom.Label.FontName = "Noto Serif Display Light";
        AvaPlot.Plot.Axes.Bottom.Label.Bold = false;
        AvaPlot.Plot.Axes.Bottom.Label.Italic = false;
        AvaPlot.Plot.Axes.Bottom.Label.FontSize = 24;
        AvaPlot.Plot.Axes.Bottom.TickLabelStyle.FontSize = 18;

        AvaPlot.Plot.Axes.Left.Label.Text = "Alumni Sans (Regular)";
        AvaPlot.Plot.Axes.Left.Label.FontName = "Alumni Sans";
        AvaPlot.Plot.Axes.Left.Label.Bold = false;
        AvaPlot.Plot.Axes.Left.Label.Italic = false;
        AvaPlot.Plot.Axes.Left.Label.FontSize = 24;
        AvaPlot.Plot.Axes.Left.TickLabelStyle.FontSize = 18;
    }
}
