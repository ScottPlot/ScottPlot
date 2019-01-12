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
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f = new FormScatter();
            f.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var f = new FormSignal();
            f.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var f = new FormMisc();
            f.ShowDialog();
        }
    }
}
