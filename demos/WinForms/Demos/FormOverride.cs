using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class FormOverride : Form
    {
        ContextMenuStrip cmRightClickMenu;

        public FormOverride()
        {
            InitializeComponent();

            cmRightClickMenu = new ContextMenuStrip();
            cmRightClickMenu.Items.Add("TOTALLY CUSTOM");
            cmRightClickMenu.Items.Add("SPEIAL MENU");
            cmRightClickMenu.Items.Add("AWESOME STUFF!");
            cmRightClickMenu.ItemClicked += new ToolStripItemClickedEventHandler(CustomMenuItemClicked);

            formsPlot1.Configure(enableRightClickMenu: false);
            formsPlot1.Configure(enableDoubleClickBenchmark: false);

            formsPlot1.plt.PlotSignal(ScottPlot.DataGen.Sin(50));
            formsPlot1.plt.PlotSignal(ScottPlot.DataGen.Cos(50));
            formsPlot1.Render();
        }

        private void FormOverride_Load(object sender, EventArgs e)
        {
        }

        private void formsPlot1_MouseClicked(object sender, MouseEventArgs e)
        {
            Console.WriteLine($"CLICKED MOUSE BUTTON: {e.Button}");
        }

        private void formsPlot1_MenuDeployed(object sender, MouseEventArgs e)
        {
            Console.WriteLine($"LAUNCHING CUSTOM MENU");
            cmRightClickMenu.Show(formsPlot1, PointToClient(Cursor.Position));
        }

        private void CustomMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Console.WriteLine($"Custom menu item clicked: {e.ClickedItem}");
        }

        private void formsPlot1_MouseDoubleClicked(object sender, MouseEventArgs e)
        {
            MessageBox.Show("you double-clicked the plot", "DOUBLE-CLICK");
        }
    }
}
