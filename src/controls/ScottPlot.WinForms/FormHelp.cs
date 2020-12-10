using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.UserControls
{
    public partial class FormHelp : Form
    {
        public FormHelp()
        {
            InitializeComponent();
            lblVersion.Text = Plot.Version;
            btnScottPlotGithub.Select();
        }

        private void FormHelp_Load(object sender, EventArgs e)
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine("Left-click-drag: pan");
            msg.AppendLine("Right-click-drag: zoom");
            msg.AppendLine("Middle-click-drag: zoom region");
            msg.AppendLine("ALT+Left-click-drag: zoom region");
            msg.AppendLine("Scroll wheel: zoom to cursor");
            msg.AppendLine("");
            msg.AppendLine("Right-click: show menu");
            msg.AppendLine("Middle-click: auto-axis");
            msg.AppendLine("Double-click: show benchmark");
            msg.AppendLine("");
            msg.AppendLine("CTRL+Left-click-drag to pan horizontally");
            msg.AppendLine("SHIFT+Left-click-drag to pan vertically");
            msg.AppendLine("CTRL+Right-click-drag to zoom horizontally");
            msg.AppendLine("SHIFT+Right-click-drag to zoom vertically");
            msg.AppendLine("CTRL+SHIFT+Right-click-drag to zoom evenly");
            richTextBox1.Text = msg.ToString();
        }

        private void BtnScottPlotGithub_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://swharden.com/scottplot/");
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
