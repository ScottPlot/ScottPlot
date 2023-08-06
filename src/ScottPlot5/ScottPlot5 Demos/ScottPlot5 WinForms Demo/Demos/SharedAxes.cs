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
            CoordinateRange newX = formsPlot1.Plot.GetAxisLimits().XRange;
            CoordinateRange currentX = formsPlot2.Plot.GetAxisLimits().XRange;
            CoordinateRange currentY = formsPlot2.Plot.GetAxisLimits().YRange;

            if (!newX.Equals(currentX))
            {
                formsPlot2.Plot.SetAxisLimits(newX, currentY);
                formsPlot1.RefreshQueue(formsPlot2); // update plot 2 next time plot 1 draws
            }
        };

        formsPlot2.Plot.RenderManager.RenderFinished += (object? sender, ScottPlot.Rendering.RenderDetails plot2Info) =>
        {
            CoordinateRange newX = formsPlot2.Plot.GetAxisLimits().XRange;
            CoordinateRange currentX = formsPlot1.Plot.GetAxisLimits().XRange;
            CoordinateRange currentY = formsPlot1.Plot.GetAxisLimits().YRange;

            if (!newX.Equals(currentX))
            {
                formsPlot1.Plot.SetAxisLimits(newX, currentY);
                formsPlot2.RefreshQueue(formsPlot1); // update plot 1 next time plot 2 draws
            }
        };
    }

    public string Title => "Shared Axes";

    public string Description => "Two plot controls that share axis limits so one when changes the other updates automatically";
}
