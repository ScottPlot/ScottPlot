namespace ScottPlot5_WinForms_Demo;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        Text = ScottPlot.Version.VersionString;

        ScottPlot.WinForms.FormsPlot formsPlot1 = new()
        {
            Dock = DockStyle.Fill
        };

        Controls.Add(formsPlot1);

        formsPlot1.Plot.Plottables.AddSignal(ScottPlot.Generate.Sin(51));
        formsPlot1.Plot.Plottables.AddSignal(ScottPlot.Generate.Cos(51));
        formsPlot1.Refresh();
    }
}
