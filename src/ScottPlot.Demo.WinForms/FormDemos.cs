using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms
{
    public partial class FormDemos : Form
    {
        public FormDemos()
        {
            InitializeComponent();
        }

        private void MouseTrackerButton_Click(object sender, EventArgs e)
        {
            new WinFormsDemos.MouseTracker().ShowDialog();
        }
    }
}
