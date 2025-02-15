using ScottPlot;
using ScottPlot.WinForms;

namespace WinForms_Demo.Demos;
public partial class ScrollViewerDemo : Form, IDemoWindow
{
    public string Title => "Plot is a Scroll Viewer";

    public string Description => "How to switch between using the mouse wheel to " +
        "scroll up/down vs. zoom in/out";

    readonly FormsPlot[] FormsPlots;

    public ScrollViewerDemo()
    {
        InitializeComponent();
        FormsPlots = [formsPlot1, formsPlot2, formsPlot3];

        foreach (var formsPlot in FormsPlots)
        {
            formsPlot.Plot.Add.Signal(Generate.RandomWalk(100));
        }

        radioScrollUpDown.CheckedChanged += (s, e) => UpdateMouseWheelAction();
        radioZoomInOut.CheckedChanged += (s, e) => UpdateMouseWheelAction();

        // intercept mouse wheel events so we can control whether the viewer is scrolled
        MouseWheel += FormsPlot_MouseWheel;

        // tell each control to forward its mouse wheel event here
        foreach (var formsPlot in FormsPlots)
        {
            formsPlot.MouseWheel += FormsPlot_MouseWheel;
        }
    }

    private void FormsPlot_MouseWheel(object? sender, MouseEventArgs e)
    {
        // indicate the event was handled if we do not want it to scroll the viewer
        ((HandledMouseEventArgs)e).Handled = radioZoomInOut.Checked;
    }

    private void UpdateMouseWheelAction()
    {
        foreach (var formsPlot in FormsPlots)
        {
            if (radioScrollUpDown.Checked)
            {
                formsPlot.UserInputProcessor.RemoveAll<ScottPlot.Interactivity.UserActionResponses.MouseWheelZoom>();
            }
            else
            {
                formsPlot.UserInputProcessor.Reset();
            }
        }
    }
}
