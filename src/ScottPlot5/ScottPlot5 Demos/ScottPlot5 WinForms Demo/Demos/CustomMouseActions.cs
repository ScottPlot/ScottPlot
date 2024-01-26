namespace WinForms_Demo.Demos;

public partial class CustomMouseActions : Form, IDemoWindow
{
    public string Title => "Custom Mouse Actions";

    public string Description => "Demonstrates how to disable the mouse or changes what the button actions are";

    public CustomMouseActions()
    {
        InitializeComponent();

        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin());
        formsPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos());

        btnDefault_Click(this, EventArgs.Empty);
    }

    private void btnDefault_Click(object sender, EventArgs e)
    {
        richTextBox1.Text = "left-click-drag pan, right-click-drag zoom, middle-click autoscale, right-click menu";

        ScottPlot.Control.Interaction interaction = new(formsPlot1)
        {
            Inputs = ScottPlot.Control.InputBindings.Standard(),
            Actions = ScottPlot.Control.PlotActions.Standard(),
        };

        formsPlot1.Interaction = interaction;
    }

    private void btnDisable_Click(object sender, EventArgs e)
    {
        richTextBox1.Text = "no mouse interactivity";

        ScottPlot.Control.InputBindings customInputBindings = new() { };

        ScottPlot.Control.Interaction interaction = new(formsPlot1)
        {
            Inputs = customInputBindings,
            Actions = ScottPlot.Control.PlotActions.NonInteractive(),
        };

        formsPlot1.Interaction = interaction;
    }

    private void btnCustom_Click(object sender, EventArgs e)
    {
        richTextBox1.Text = "middle-click-drag pan, right-click zoom rectangle, right-click autoscale, left-click menu";

        ScottPlot.Control.InputBindings customInputBindings = new()
        {
            DragPanButton = ScottPlot.Control.MouseButton.Middle,
            DragZoomRectangleButton = ScottPlot.Control.MouseButton.Right,
            DragZoomButton = ScottPlot.Control.MouseButton.Right,
            ZoomInWheelDirection = ScottPlot.Control.MouseWheelDirection.Up,
            ZoomOutWheelDirection = ScottPlot.Control.MouseWheelDirection.Down,
            ClickAutoAxisButton = ScottPlot.Control.MouseButton.Right,
            ClickContextMenuButton = ScottPlot.Control.MouseButton.Left,
        };

        ScottPlot.Control.Interaction interaction = new(formsPlot1)
        {
            Inputs = customInputBindings,
        };

        formsPlot1.Interaction = interaction;
    }
}
