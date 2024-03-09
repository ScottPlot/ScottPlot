using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class ImageBackgrounds : Form, IDemoWindow
{
    public string Title => "Background Images";

    public string Description => "Use a bitmap image for the background of the figure or data area";

    readonly ImagePosition[] Positions = Enum.GetValues<ImagePosition>();

    public ImageBackgrounds()
    {
        InitializeComponent();

        foreach (ImagePosition mode in Positions)
        {
            cbMode.Items.Add(mode);
        }
        cbMode.SelectedIndex = 2;

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
        formsPlot1.Plot.YLabel("Vertical Axis");
        formsPlot1.Plot.XLabel("Horizontal Axis");
        formsPlot1.Plot.Title("Plot with Image Background");

        // assign the bitmap image
        formsPlot1.Plot.FigureBackground.Image = cbFigure.Checked ? SampleImages.ScottPlotLogo() : null;
        formsPlot1.Plot.DataBackground.Image = cbData.Checked ? SampleImages.MonaLisa() : null;

        // set the scaling mode
        formsPlot1.Plot.FigureBackground.ImagePosition = Positions[cbMode.SelectedIndex];
        formsPlot1.Plot.DataBackground.ImagePosition = Positions[cbMode.SelectedIndex];

        // force a redraw
        formsPlot1.Refresh();
    }
}
