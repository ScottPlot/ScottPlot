using ScottPlot.WinForms;

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

        btnDefault.Click += (s, e) =>
        {
            richTextBox1.Text = "left-click-drag pan, right-click-drag zoom, middle-click autoscale, " +
                "middle-click-drag zoom rectangle, alt+left-click-drag zoom rectangle, right-click menu, " +
                "double-click benchmark, scroll wheel zoom, arrow keys pan, " +
                "shift or alt with arrow keys pans more or less, ctrl+arrow keys zoom";

            formsPlot1.UserInputProcessor.IsEnabled = true;
            formsPlot1.UserInputProcessor.Reset();
        };

        btnDisable.Click += (s, e) =>
        {
            richTextBox1.Text = "Mouse and keyboard events are disabled";
            formsPlot1.UserInputProcessor.IsEnabled = false;
        };

        btnCustom.Click += (s, e) =>
        {
            richTextBox1.Text = "middle-click-drag pan, right-click-drag zoom rectangle, " +
                "right-click autoscale, left-click menu, Q key autoscale, WASD keys pan";

            formsPlot1.UserInputProcessor.IsEnabled = true;

            // remove all existing responses so we can create and add our own
            formsPlot1.UserInputProcessor.UserActionResponses.Clear();

            // middle-click-drag pan
            var panButton = ScottPlot.Interactivity.StandardMouseButtons.Middle;
            var panResponse = new ScottPlot.Interactivity.UserActionResponses.MouseDragPan(panButton);
            formsPlot1.UserInputProcessor.UserActionResponses.Add(panResponse);

            // right-click-drag zoom rectangle
            var zoomRectangleButton = ScottPlot.Interactivity.StandardMouseButtons.Right;
            var zoomRectangleResponse = new ScottPlot.Interactivity.UserActionResponses.MouseDragZoomRectangle(zoomRectangleButton);
            formsPlot1.UserInputProcessor.UserActionResponses.Add(zoomRectangleResponse);

            // right-click autoscale
            var autoscaleButton = ScottPlot.Interactivity.StandardMouseButtons.Right;
            var autoscaleResponse = new ScottPlot.Interactivity.UserActionResponses.SingleClickAutoscale(autoscaleButton);
            formsPlot1.UserInputProcessor.UserActionResponses.Add(autoscaleResponse);

            // left-click menu
            var menuButton = ScottPlot.Interactivity.StandardMouseButtons.Left;
            var menuResponse = new ScottPlot.Interactivity.UserActionResponses.SingleClickContextMenu(menuButton);
            formsPlot1.UserInputProcessor.UserActionResponses.Add(menuResponse);

            // Q key autoscale too
            var autoscaleKey = new ScottPlot.Interactivity.Key("Q");
            Action<ScottPlot.IPlotControl, ScottPlot.Pixel> autoscaleAction = (plotControl, pixel) => plotControl.Plot.Axes.AutoScale();
            var autoscaleKeyResponse = new ScottPlot.Interactivity.UserActionResponses.KeyPressResponse(autoscaleKey, autoscaleAction);
            formsPlot1.UserInputProcessor.UserActionResponses.Add(autoscaleKeyResponse);

            // WASD keys pan
            var keyPanResponse = new ScottPlot.Interactivity.UserActionResponses.KeyboardPanAndZoom()
            {
                PanUpKey = new ScottPlot.Interactivity.Key("W"),
                PanLeftKey = new ScottPlot.Interactivity.Key("A"),
                PanDownKey = new ScottPlot.Interactivity.Key("S"),
                PanRightKey = new ScottPlot.Interactivity.Key("D"),
            };
            formsPlot1.UserInputProcessor.UserActionResponses.Add(keyPanResponse);
        };

        Load += (s, e) => btnDefault.PerformClick();
    }

    /// <summary>
    /// Required because arrow key presses do not invoke KeyDown
    /// </summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        ScottPlot.Interactivity.Key key = keyData.GetKey();
        if (ScottPlot.Interactivity.StandardKeys.IsArrowKey(key))
        {
            var keyDownAction = new ScottPlot.Interactivity.UserActions.KeyDown(key);
            formsPlot1.UserInputProcessor.Process(keyDownAction);
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }
}
