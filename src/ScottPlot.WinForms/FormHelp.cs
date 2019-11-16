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
            lblVersion.Text = Tools.GetVersionString();
            btnScottPlotGithub.Select();
        }

        private void FormHelp_Load(object sender, EventArgs e)
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine("Left-click-drag: pan");
            msg.AppendLine("Right-click-drag: zoom");
            msg.AppendLine("Middle-click-drag: zoom region");
            msg.AppendLine("");
            msg.AppendLine("Right-click: show menu");
            msg.AppendLine("Middle-click: auto-axis");
            msg.AppendLine("Double-click: show benchmark");
            msg.AppendLine("");
            msg.AppendLine("Hold CTRL to lock vertical axis");
            msg.AppendLine("Hold ALT to lock horizontal axis");
            richTextBox1.Text = msg.ToString();
        }

        private void BtnScottPlotGithub_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/swharden/ScottPlot");
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
