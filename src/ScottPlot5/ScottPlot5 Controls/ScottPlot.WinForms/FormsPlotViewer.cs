using System.Windows.Forms;

namespace ScottPlot.WinForms;

public static class FormsPlotViewer
{
    public static Form CreateForm(Plot plot, string title = "", int width = 600, int height = 400)
    {
        FormsPlot formsPlot = new() { Dock = DockStyle.Fill };
        formsPlot.Reset(plot);

        Form form = new()
        {
            StartPosition = FormStartPosition.CenterScreen,
            Width = width,
            Height = height,
            Text = title,
        };

        form.Controls.Add(formsPlot);

        return form;
    }

    public static void Launch(Plot plot, string title = "", int width = 600, int height = 400, bool blocking = true)
    {
        Form form = CreateForm(plot, title, width, height);

        if (blocking)
            form.ShowDialog();
        else
            form.Show();
    }
}
