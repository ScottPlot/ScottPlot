using System.Windows.Forms;

namespace ScottPlot.WinForms;

public static class LauncherExtensions
{
    /// <summary>
    /// Launch the plot in an interactive WinForms control
    /// </summary>
    public static void FormsPlot(this Launcher launcher, int width = 600, int height = 400, string title = "Interactive Plot")
    {
        FormsPlot formsPlot1 = new() { Dock = DockStyle.Fill };
        formsPlot1.Reset(launcher.Plot);

        Form form = new()
        {
            Width = width,
            Height = height,
            StartPosition = FormStartPosition.CenterScreen,
            Text = title,
        };

        form.Controls.Add(formsPlot1);
        form.Load += (s, e) => formsPlot1.Refresh();

        form.ShowDialog();
    }
}
