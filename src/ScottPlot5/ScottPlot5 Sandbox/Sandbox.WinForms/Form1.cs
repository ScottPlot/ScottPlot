namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin(51));
        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos(51));

        formsPlot1.Refresh();
    }

    private void cbGuides_CheckedChanged(object sender, EventArgs e)
    {
        formsPlot1.Plot.Developer_ShowAxisDetails(cbGuides.Checked);
        formsPlot1.Refresh();
    }

    private void btnAddX_Click(object sender, EventArgs e)
    {
        ScottPlot.Axis.StandardAxes.BottomAxis newAxis = new();
        newAxis.Label.Text = "Additional Axis";
        formsPlot1.Plot.XAxes.Add(newAxis);
        formsPlot1.Refresh();
    }

    private void btnRemoveX_Click(object sender, EventArgs e)
    {
        if (formsPlot1.Plot.XAxes.Count == 1)
            return;

        formsPlot1.Plot.XAxes.Remove(formsPlot1.Plot.XAxes.Last());
        formsPlot1.Refresh();
    }

    private void btnAddY_Click(object sender, EventArgs e)
    {

    }

    private void btnRemoveY_Click(object sender, EventArgs e)
    {

    }
}
