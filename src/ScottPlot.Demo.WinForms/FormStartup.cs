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
    public partial class FormStartup : Form
    {
        public FormStartup()
        {
            InitializeComponent();
        }

        private void FormStartup_Load(object sender, EventArgs e)
        {
            versionLabel.Text = Tools.GetVersionString();
        }

        private void cookbookButton_Click(object sender, EventArgs e)
        {
            new FormCookbook().ShowDialog();
        }

        private void winFormsDemosButtom_Click(object sender, EventArgs e)
        {
            new FormDemos().ShowDialog();
        }
    }
}
