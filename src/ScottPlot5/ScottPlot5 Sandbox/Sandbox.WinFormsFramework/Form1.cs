using ScottPlot;
using ScottPlot.WinForms;
using System.Windows.Forms;

namespace Sandbox.WinFormsFramework;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        FormsPlot formsPlot1 = new() { Dock = DockStyle.Fill };
        Controls.Add(formsPlot1);

        formsPlot1.Plot.Add.Signal(Generate.Sin());
        formsPlot1.Plot.Add.Signal(Generate.Cos());

        var an = formsPlot1.Plot.Add.Annotation("test", Alignment.UpperCenter);

        formsPlot1.Plot.RenderManager.RenderStarting += (object sender, RenderPack rp) =>
        {
            AxisLimits thisRenderLimits = rp.Plot.Axes.GetLimits();
            AxisLimits lastRenderLimits = rp.Plot.LastRender.AxisLimits;

            if (thisRenderLimits == lastRenderLimits)
            {
                // test this by resizing the window
                an.Text = "limits unchanged";
            }
            else
            {
                an.Text = thisRenderLimits.ToString(2);
            }
        };
    }
}
