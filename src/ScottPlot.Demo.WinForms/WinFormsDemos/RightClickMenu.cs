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

            var customMenu = new ContextMenuStrip();
            customMenu.Items.Add("Add Sine Wave");
            customMenu.Items.Add("Clear Plot");
            customMenu.ItemClicked += new ToolStripItemClickedEventHandler(MenuItemClicked);

            // replace the default use control right-click menu with your own
            formsPlot1.rightClickMenu = customMenu;
        }

        private void MenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            formsPlot1.rightClickMenu.Hide();
            if (e.ClickedItem.Text == "Add Sine Wave")
                formsPlot1.plt.PlotSignal(DataGen.Sin(51, phase: rand.NextDouble() * 1000));
            else if (e.ClickedItem.Text == "Clear Plot")
                formsPlot1.plt.Clear();
            formsPlot1.plt.AxisAuto();
            formsPlot1.Render();
        }
    }
}
