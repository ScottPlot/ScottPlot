using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    public partial class RightClickMenu : Form
    {
        public RightClickMenu()
        {
            InitializeComponent();

            formsPlot1.RightClicked -= formsPlot1.DefaultRightClickEvent;
            formsPlot1.RightClicked += CustomRightClickEvent;

            formsPlot1.Plot.AddSignal(DataGen.Sin(51));
            formsPlot1.Plot.AddSignal(DataGen.Cos(51));
            formsPlot1.Refresh();
        }

        private void CustomRightClickEvent(object sender, EventArgs e)
        {
            var customMenu = new ContextMenu();
            customMenu.Items.Add(new ButtonMenuItem(AddSine) { Text = "Add Sine Wave" });
            customMenu.Items.Add(new ButtonMenuItem(ClearPlot) { Text = "Clear Plot" });
            customMenu.Show(Mouse.Position);
        }

        private void AddSine(object sender, EventArgs e)
        {
            Random rand = new Random();
            double[] data = DataGen.Sin(51, phase: rand.NextDouble() * 1000);
            formsPlot1.Plot.AddSignal(data);
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Refresh();
        }

        private void ClearPlot(object sender, EventArgs e)
        {
            formsPlot1.Plot.Clear();
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Refresh();
        }
    }
}
