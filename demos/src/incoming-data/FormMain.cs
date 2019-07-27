using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemoIncomingData
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void BtnGrowingArray_Click(object sender, EventArgs e)
        {
            FormGrowingArray frm = new FormGrowingArray();
            frm.ShowDialog();
        }

        private void BtnCircularBuffer_Click(object sender, EventArgs e)
        {
            FormCircularBuffer frm = new FormCircularBuffer();
            frm.ShowDialog();
        }

        private void BtnRollingBuffer_Click(object sender, EventArgs e)
        {
            FormRollingBuffer frm = new FormRollingBuffer();
            frm.ShowDialog();
        }
    }
}
