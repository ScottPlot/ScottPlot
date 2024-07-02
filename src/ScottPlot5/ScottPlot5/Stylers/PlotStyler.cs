using ScottPlot.AxisPanels;

namespace ScottPlot.Stylers;

/// <summary>
/// Helper methods for applying common styling options to plots
/// </summary>
public class PlotStyler(Plot plot)
{
    private readonly Plot Plot = plot;
    private bool _darkMode = false;
    public bool IsDarkMode
    {
        get => _darkMode;
        set
        {
            if (_darkMode == false && value == true)
            {
                ChangeToDarkMode();
            }
            else if (_darkMode == true && value == false)
            {
                ChangeToLightMode();
            }
            _darkMode = value;
        }
    }

    [Obsolete("This method is deprecated. Assign Plot.FigureBackground.Color instead.", true)]
    public void Background(Color figure, Color data) { }

    [Obsolete("This method is deprecated. Call Plot.Axes.Color() instead.", true)]
    public void ColorAxes(Color color) { }

    [Obsolete("Reference Plot.Legend properties directly.", true)]
    public void ColorLegend(Color background, Color foreground, Color border) { }

    [Obsolete("This method is deprecated. Call Plot.Axes.Frame() methods instead.", true)]
    public void AxisFrame(float left, float right, float bottom, float top) { }

    [Obsolete("This method is deprecated. Call Plot.Font.Set() instead.", true)]
    public void SetFont(string fontName) { }

    [Obsolete("This method is deprecated. Call Plot.Font.Automatic() instead.", true)]
    public void SetBestFonts() { }

    private PlotColorsSettings _darkColors = new PlotColorsSettings()
    {
        Palette = new Palettes.Penumbra(),
        Axes = Color.FromHex("#d7d7d7"),
        GridMajorLine = Color.FromHex("#404040"),
        FigureBackground = Color.FromHex("#181818"),
        DataBackGround = Color.FromHex("#1f1f1f"),
        LegendBackground = Color.FromHex("#404040"),
        LegendFont = Color.FromHex("#d7d7d7"),
        LegendOutline = Color.FromHex("#d7d7d7"),
    };

    private PlotColorsSettings _lightColors = new PlotColorsSettings()
    {
        Palette = new Palettes.Category10(),
        Axes = Colors.Black,
        GridMajorLine = Colors.Black.WithOpacity(.1),
        FigureBackground = Colors.White,
        DataBackGround = Colors.Transparent,
        LegendBackground = Colors.White,
        LegendFont = null,
        LegendOutline = Colors.Black,
    };

    public void GrabLightSettings()
    {
        _lightColors = new PlotColorsSettings()
        {
            Palette = Plot.Add.Palette,
            Axes = Plot.Axes.Left.TickLabelStyle.ForeColor,
            GridMajorLine = Plot.Grid.MajorLineColor,
            FigureBackground = Plot.FigureBackground.Color,
            DataBackGround = Plot.DataBackground.Color,
            LegendBackground = Plot.Legend.BackgroundColor,
            LegendFont = Plot.Legend.FontColor,
            LegendOutline = Plot.Legend.OutlineColor,
        };
    }

    private void ChangeColorModeFromTo(PlotColorsSettings from, PlotColorsSettings to)
    {
        if (Plot.Add.Palette.Name == from.Palette.Name)
            Plot.Add.Palette = to.Palette;

        foreach (AxisBase axis in Plot.Axes.GetAxes().OfType<AxisBase>())
        {
            if (axis.LabelStyle.ForeColor == from.Axes)
                axis.LabelStyle.ForeColor = to.Axes;
            if (axis.TickLabelStyle.ForeColor == from.Axes)
                axis.TickLabelStyle.ForeColor = to.Axes;
            if (axis.MajorTickStyle.Color == from.Axes)
                axis.MajorTickStyle.Color = to.Axes;
            if (axis.MinorTickStyle.Color == from.Axes)
                axis.MinorTickStyle.Color = to.Axes;
            if (axis.FrameLineStyle.Color == from.Axes)
                axis.FrameLineStyle.Color = to.Axes;
        }

        if (Plot.Axes.Title.Label.ForeColor == from.Axes)
            Plot.Axes.Title.Label.ForeColor = to.Axes;

        if (Plot.Grid.MajorLineColor == from.GridMajorLine)
            Plot.Grid.MajorLineColor = to.GridMajorLine;

        if (Plot.FigureBackground.Color == from.FigureBackground)
            Plot.FigureBackground.Color = to.FigureBackground;

        if (Plot.DataBackground.Color == from.DataBackGround)
            Plot.DataBackground.Color = to.DataBackGround;

        if (Plot.Legend.BackgroundColor == from.LegendBackground)
            Plot.Legend.BackgroundColor = to.LegendBackground;

        if (Plot.Legend.FontColor == from.LegendFont)
            Plot.Legend.FontColor = to.LegendFont;

        if (Plot.Legend.OutlineColor == from.LegendOutline)
            Plot.Legend.OutlineColor = to.LegendOutline;
    }

    private void ChangeToDarkMode()
    {
        ChangeColorModeFromTo(_lightColors, _darkColors);
    }

    private void ChangeToLightMode()
    {
        ChangeColorModeFromTo(_darkColors, _lightColors);
    }

    /// <summary>
    /// Reset colors and palette do a dark mode style
    /// </summary>
    public void DarkMode()
    {
        Plot.Add.Palette = new Palettes.Penumbra();

        Plot.Axes.Color(Color.FromHex("#d7d7d7"));

        Plot.Grid.MajorLineColor = Color.FromHex("#404040");
        Plot.FigureBackground.Color = Color.FromHex("#181818");
        Plot.DataBackground.Color = Color.FromHex("#1f1f1f");

        Plot.Legend.BackgroundColor = Color.FromHex("#404040");
        Plot.Legend.FontColor = Color.FromHex("#d7d7d7");
        Plot.Legend.OutlineColor = Color.FromHex("#d7d7d7");
    }
}
