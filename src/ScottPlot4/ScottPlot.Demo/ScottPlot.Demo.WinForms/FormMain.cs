using System;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            versionLabel.Text = Plot.Version;
            demoListControl1.Height = (int)(demoListControl1.Height * 1.4);
        }

        private void WebsiteLink_Click(object sender, EventArgs e) => Tools.LaunchBrowser("https://ScottPlot.NET/demo");

        private void CookbookButton_Click(object sender, EventArgs e) => new FormCookbook().ShowDialog();
    }
}
