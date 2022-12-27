using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    readonly RandomDataGenerator RDG = new();

    public Form1()
    {
        InitializeComponent();

        double[] ys = RDG.RandomSin(1000);
        double sampleRate = RDG.RandomNumber(1, 10);
        formsPlot1.Plot.Add.Signal(ys, sampleRate);

        formsPlot1.Plot.XAxis.Label.Color = formsPlot1.Plot.Palette.GetColor(0);
        formsPlot1.Plot.YAxis.Label.Color = formsPlot1.Plot.Palette.GetColor(0);
        formsPlot1.Plot.Developer_ShowAxisDetails(cbGuides.Checked);

        formsPlot1.Refresh();
    }

    private void cbGuides_CheckedChanged(object sender, EventArgs e)
    {
        formsPlot1.Plot.Developer_ShowAxisDetails(cbGuides.Checked);
        formsPlot1.Refresh();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        ScottPlot.Axis.StandardAxes.BottomAxis newBottomAxis = new();
        newBottomAxis.Label.Text = "Additional Bottom Axis";
        newBottomAxis.Label.Color = formsPlot1.Plot.Palette.GetColor(formsPlot1.Plot.XAxes.Count);
        newBottomAxis.ShowDebugInformation = cbGuides.Checked;
        formsPlot1.Plot.XAxes.Add(newBottomAxis);

        ScottPlot.Axis.StandardAxes.LeftAxis newLeftAxis = new();
        newLeftAxis.Label.Text = "Additional Left Axis";
        newLeftAxis.Label.Color = formsPlot1.Plot.Palette.GetColor(formsPlot1.Plot.YAxes.Count);
        newLeftAxis.ShowDebugInformation = cbGuides.Checked;
        formsPlot1.Plot.YAxes.Add(newLeftAxis);

        double[] ys = RDG.RandomSin(1000);
        double sampleRate = RDG.RandomNumber(1, 10);
        var sig = formsPlot1.Plot.Add.Signal(ys, sampleRate);
        sig.Axes.XAxis = newBottomAxis;
        sig.Axes.YAxis = newLeftAxis;

        formsPlot1.Refresh();
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
        // remove the plottable
        IPlottable lastPlottable = formsPlot1.Plot.Plottables.Last();
        formsPlot1.Plot.Plottables.Remove(lastPlottable);

        // remove both axes
        formsPlot1.Plot.XAxes.Remove(lastPlottable.Axes.XAxis);
        formsPlot1.Plot.YAxes.Remove(lastPlottable.Axes.YAxis);

        formsPlot1.Refresh();

        if (formsPlot1.Plot.Plottables.Count == 1)
        {
            btnRemove.Enabled = false;
            return;
        }
    }
}
