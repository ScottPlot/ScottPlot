using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Benchmark
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void BitmapButton_Click(object sender, EventArgs e)
        {
            new FormBmp().ShowDialog();
        }

        private void ControlButton_Click(object sender, EventArgs e)
        {
            new FormGui().ShowDialog();
        }
    }
}
