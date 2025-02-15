using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class ReportViewer : Form, IDemoWindow
{
    public string Title => "Report Viewer";
    public string Description => "A simple strategy for displaying multiple plots paired with descriptions";

    readonly ScottPlot.Reporting.PlotCollection Report = GetSampleReport();

    public ReportViewer()
    {
        InitializeComponent();

        foreach (string title in Report.Figures.Select(x => x.Title))
        {
            listBox1.Items.Add(title);
        }

        listBox1.SelectedIndexChanged += (s, e) =>
        {
            if (listBox1.SelectedIndex < 0)
                return;

            ScottPlot.Reporting.PlotFigure figure = Report.Figures[listBox1.SelectedIndex];
            richTextBox1.Text = figure.Description;
            formsPlot1.Reset(figure.Plot, disposeOldPlot: false);
            formsPlot1.Plot.RenderInMemory();
            formsPlot1.Refresh();
        };
    }

    private static ScottPlot.Reporting.PlotCollection GetSampleReport()
    {
        ScottPlot.Reporting.PlotCollection report = new();

        Plot plot1 = new();
        plot1.Add.Signal(Generate.Sin());
        report.Add(plot1, "Sine", "A sine wave starts at zero");

        Plot plot2 = new();
        plot2.Add.Signal(Generate.Cos());
        report.Add(plot2, "Cosine", "A cosine wave starts at one");

        return report;
    }
}
