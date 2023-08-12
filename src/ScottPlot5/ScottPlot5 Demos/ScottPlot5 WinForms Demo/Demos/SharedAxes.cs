using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class SharedAxesForm : Form, IDemoWindow
{
    SharedAxisManager SharedAxes = new(shareX: true, shareY: false);
    SharedLayoutManager SharedLayout = new(shareX: true, shareY: false);

    public SharedAxesForm()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin(mult: 100_000));
        formsPlot2.Plot.Add.Signal(ScottPlot.Generate.Cos());

        SharedAxes.Add(formsPlot1);
        SharedAxes.Add(formsPlot2);

        SharedLayout.Add(formsPlot1);
        SharedLayout.Add(formsPlot2);
    }

    public string Title => "Shared Axes";

    public string Description => "Two plot controls that share axis limits so one when changes the other updates automatically";
}
