using ScottPlot.WinForms;

namespace WinForms_Demo.Demos;
public partial class TransparentBackground : Form, IDemoWindow
{
    public string Title => "Transparent Background";

    public string Description => "Plot controls may be made transparent so the Form beneath it can shine through";

    public TransparentBackground()
    {
        InitializeComponent();

        // give the Form a complex background image
        BackgroundImage = ScottPlot.SampleImages.MonaLisa().Scaled(3).GetBitmap();

        // add sample data to the plot
        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin());
        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos());

        // use a dark background color theme
        formsPlot1.Plot.SetStyle(new ScottPlot.PlotStyles.Dark());

        // tell the plot control to be transparent
        formsPlot1.BackColor = Color.Transparent;

        // configure a fully transparent figure background
        formsPlot1.Plot.FigureBackground.Color = ScottPlot.Colors.Transparent;

        // configure a semi-transparent data background
        formsPlot1.Plot.DataBackground.Color = formsPlot1.Plot.FigureBackground.Color.WithAlpha(0.5);
    }
}
