using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot
{
    public partial class FormHelp : Form
    {
        public FormHelp()
        {
            InitializeComponent();
            Text = $"ScottPlot {Plot.Version} Help";
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = true;
            lblVersion.Text = $"ScottPlot.WinForms {Plot.Version}";
            lblMessage.Text = Control.ControlBackEnd.GetHelpMessage();
        }
    }
}
