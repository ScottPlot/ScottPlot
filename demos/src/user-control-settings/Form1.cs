using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace user_control_settings
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Random rand = new Random(0);
            formsPlot1.plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 10_000));
            formsPlot1.plt.XLabel("horizontal units");
            formsPlot1.plt.YLabel("vertical units");
            formsPlot1.Render();
        }

        private void CbMenu_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configure(enableRightClickMenu: cbMenu.Checked);
        }

        private void CbPan_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configure(enablePanning: cbPan.Checked);
        }

        private void CbZoom_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Configure(enableZooming: cbZoom.Checked);
        }

        private void FormsPlot1_MouseClicked_1(object sender, MouseEventArgs e)
        {
            Console.WriteLine($"Mouse clicked: {e.Button}");
            if ((e.Button == MouseButtons.Right) && (cbMenu.Checked == false))
            {
                var menu = new ContextMenuStrip();
                menu.Items.Add("custom menu");
                menu.ItemClicked += new ToolStripItemClickedEventHandler(CustomMenuItemClicked);
                menu.Show(this, PointToClient(Cursor.Position));
            }
        }
        private void CustomMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            System.Console.WriteLine("CLICKED: " + e.ClickedItem);
        }
    }
}
