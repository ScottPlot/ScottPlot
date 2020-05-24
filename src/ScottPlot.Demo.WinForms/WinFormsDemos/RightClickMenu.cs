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

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class RightClickMenu : Form
    {
        Random rand = new Random();
        public RightClickMenu()
        {
            InitializeComponent();
            formsPlot1.plt.PlotSignal(DataGen.Sin(51));
            formsPlot1.plt.PlotSignal(DataGen.Cos(51));
            formsPlot1.Render();

            ContextMenuStrip customMenu = new ContextMenuStrip();
            customMenu.Items.Add(new ToolStripMenuItem("Add Sine Wave", null, new EventHandler(AddSine)));
            customMenu.Items.Add(new ToolStripMenuItem("Clear Plot", null, new EventHandler(ClearPlot)));
            formsPlot1.ContextMenuStrip = customMenu;
        }

        private void AddSine(object sender, EventArgs e)
        {
            formsPlot1.plt.PlotSignal(DataGen.Sin(51, phase: rand.NextDouble() * 1000));
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }

        private void ClearPlot(object sender, EventArgs e)
        {
            formsPlot1.plt.Clear();
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }
    }
}
