using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class ReportViewer : Form, IDemoWindow
{
    public string Title => "Report Viewer";
    public string Description => "A simple strategy for displaying multiple plots paired with descriptions";

    readonly ScottPlot.Reporting.Report Report = GetSampleReport();

    ScottPlot.Reporting.Components.PlotComponent[] PlotComponents => Report.ComponentList.OfType<ScottPlot.Reporting.Components.PlotComponent>().ToArray();

    public ReportViewer()
    {
        InitializeComponent();

        foreach (string title in PlotComponents.Select(x => x.Title))
        {
            listBox1.Items.Add(title);
        }

        listBox1.SelectedIndexChanged += (s, e) =>
        {
            if (listBox1.SelectedIndex < 0)
                return;

            var plotComponent = PlotComponents[listBox1.SelectedIndex];
            richTextBox1.Text = plotComponent.Description;
            formsPlot1.Reset(plotComponent.Plot, disposeOldPlot: false);
            formsPlot1.Plot.RenderInMemory();
            formsPlot1.Refresh();
        };
    }

    private static ScottPlot.Reporting.Report GetSampleReport()
    {
        ScottPlot.Reporting.Report report = new();

        Plot plot1 = new();
        plot1.Add.Signal(Generate.Sin());
        report.AddPlot(plot1, "Sine", "A sine wave starts at zero");

        Plot plot2 = new();
        plot2.Add.Signal(Generate.Cos());
        report.AddPlot(plot2, "Cosine", "A cosine wave starts at one");

        return report;
    }
}
