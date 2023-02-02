using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        // begin with an array of DateTime values
        DateTime[] dates = Generate.ConsecutiveDateTimes(
            start: new DateTime(1985, 09, 24),
            step: TimeSpan.FromDays(1),
            count: 100);

        // convert DateTime to OLE Automation (OADate) format
        double[] xs = dates.Select(x => x.ToOADate()).ToArray();
        double[] ys = Generate.RandomWalk(xs.Length);
        formsPlot1.Plot.Add.Scatter(xs, ys);

        // tell the plot to display dates on the bottom axis
        formsPlot1.Plot.Axes.DateTimeTicks(Edge.Bottom);

        formsPlot1.Refresh();
    }
}
