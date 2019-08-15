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
    public partial class FormSettings : Form
    {
        public FormSettings(Plot plt)
        {
            InitializeComponent();

            tbX1.Text = plt.Axis()[0].ToString();
            tbX2.Text = plt.Axis()[1].ToString();
            tbY1.Text = plt.Axis()[2].ToString();
            tbY2.Text = plt.Axis()[3].ToString();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {

        }
    }
}
