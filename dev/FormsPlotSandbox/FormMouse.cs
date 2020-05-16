using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsPlotSandbox
{
    public partial class FormMouse : Form
    {
        public FormMouse()
        {
            InitializeComponent();
        }

        private void FormMouse_Load(object sender, EventArgs e)
        {
            formsPlot1.plt.PlotSignal(ScottPlot.DataGen.Sin(51));
            formsPlot1.plt.PlotSignal(ScottPlot.DataGen.Cos(51));
            formsPlot1.Render();
        }

        int counter;
        private void FormMouse_MouseMove(object sender, MouseEventArgs e)
        {
            // this doesn't get triggered when the mouse is over the plot
            Debug.WriteLine($"Form MouseMove {++counter}");
        }

        private void formsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            // this never gets triggered
            Debug.WriteLine($"FormsPlot MouseMove {++counter}");
        }

        private void formsPlot1_MouseMoved(object sender, MouseEventArgs e)
        {
            // this gets triggered whenever the mouse moves while over the plot
            Debug.WriteLine($"FormsPlot MouseMoved {++counter}");
            (double x, double y) = formsPlot1.GetMouseCoordinates();
            lblStatus.Text = $"Mouse position: ({x}, {y})";
        }
    }
}
