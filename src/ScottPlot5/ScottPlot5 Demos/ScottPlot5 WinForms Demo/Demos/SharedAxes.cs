using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class SharedAxes : Form, IDemoWindow
{
    public SharedAxes()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin(mult: 100_000));

        formsPlot2.Plot.Add.Signal(ScottPlot.Generate.Cos());

        // TODO: to ILayoutMaker add a method for getting the layout with a user defined data area

        formsPlot1.Plot.RenderManager.RenderFinished += (s, e) =>
        {
            AxisLimits originalLimits = formsPlot2.Plot.GetAxisLimits();
            formsPlot2.Plot.MatchAxisLimits(formsPlot1.Plot, x: true, y: false);
            bool limitsChanged = !formsPlot2.Plot.GetAxisLimits().Equals(originalLimits);

            if (limitsChanged)
            {
                formsPlot1.RefreshQueue(formsPlot2); // update plot 2 next time plot 1 draws
            }
        };

        formsPlot2.Plot.RenderManager.RenderFinished += (object? sender, ScottPlot.Rendering.RenderDetails plot2Info) =>
        {
            AxisLimits originalLimits = formsPlot1.Plot.GetAxisLimits();
            formsPlot1.Plot.MatchAxisLimits(formsPlot2.Plot, x: true, y: false);
            bool limitsChanged = !formsPlot1.Plot.GetAxisLimits().Equals(originalLimits);

            if (limitsChanged)
            {
                formsPlot2.RefreshQueue(formsPlot1); // update plot 2 next time plot 1 draws
            }
        };
    }

    public string Title => "Shared Axes";

    public string Description => "Two plot controls that share axis limits so one when changes the other updates automatically";
}
