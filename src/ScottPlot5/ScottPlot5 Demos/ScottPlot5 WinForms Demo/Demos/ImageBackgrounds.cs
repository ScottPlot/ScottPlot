using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class ImageBackgrounds : Form, IDemoWindow
{
    public string Title => "Background Images";

    public string Description => "Use a bitmap image for the background of the figure or data area";

    readonly ImagePosition[] ScaleModes = Enum.GetValues<ImagePosition>();

    public ImageBackgrounds()
    {
        InitializeComponent();

        foreach (ImagePosition mode in ScaleModes)
        {
            cbMode.Items.Add(mode);
        }
        cbMode.SelectedIndex = 0;

        cbData.CheckStateChanged += (s, e) => ResetPlot();
        cbFigure.CheckStateChanged += (s, e) => ResetPlot();
        cbMode.SelectionChangeCommitted += (s, e) => ResetPlot();
        ResetPlot();
    }

    public void ResetPlot()
    {
        formsPlot1.Reset();

        // add sample data
        var sig1 = formsPlot1.Plot.Add.Signal(Generate.Sin());
        var sig2 = formsPlot1.Plot.Add.Signal(Generate.Cos());
        sig1.LineWidth = 5;
        sig2.LineWidth = 5;

        // assign the bitmap image
        formsPlot1.Plot.DataBackground.Image = cbData.Checked ? SampleImages.ScottPlotLogo(100, 100) : null;
        formsPlot1.Plot.FigureBackground.Image = cbFigure.Checked ? SampleImages.ScottPlotLogo(100, 100) : null;

        // set the scaling mode
        formsPlot1.Plot.DataBackground.ImageScaling = ScaleModes[cbMode.SelectedIndex];
        formsPlot1.Plot.FigureBackground.ImageScaling = ScaleModes[cbMode.SelectedIndex];

        // force a redraw
        formsPlot1.Refresh();
    }
}
