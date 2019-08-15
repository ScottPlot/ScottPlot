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
        Plot plt;

        // let's pass in a PLT (not a user control) so it's Winforms/WPF-agnostic
        public FormSettings(Plot plt)
        {
            this.plt = plt;
            InitializeComponent();
            UpdateTextBoxesFromAxes();
            lblVersion.Text = $"Version {Tools.GetVersionString()} ({Tools.GetFrameworkVersionString()})";
            UpdatePlotObjectList();
            UpdateQualityChecks();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {

        }

        private void UpdateQualityChecks()
        {
            rbQualityLow.Checked = !plt.GetSettings().antiAliasData;
            rbQualityHigh.Checked = plt.GetSettings().antiAliasData;
            cbQualityLowWhileDragging.Checked = plt.mouseTracker.lowQualityWhileInteracting;
        }

        private void UpdateTextBoxesFromAxes()
        {
            tbX1.Text = Math.Round(plt.Axis()[0], 4).ToString();
            tbX2.Text = Math.Round(plt.Axis()[1], 4).ToString();
            tbY1.Text = Math.Round(plt.Axis()[2], 4).ToString();
            tbY2.Text = Math.Round(plt.Axis()[3], 4).ToString();
        }

        private void UpdatePlotObjectList()
        {
            lbPlotObjects.Items.Clear();
            foreach (var plotObject in plt.GetPlottables())
                lbPlotObjects.Items.Add(plotObject);
        }

        private void BtnFitData_Click(object sender, EventArgs e)
        {
            plt.AxisAuto();
            UpdateTextBoxesFromAxes();
        }

        private void BtnApplyAxes_Click(object sender, EventArgs e)
        {
            double x1, x2, y1, y2;
            double.TryParse(tbX1.Text, out x1);
            double.TryParse(tbX2.Text, out x2);
            double.TryParse(tbY1.Text, out y1);
            double.TryParse(tbY2.Text, out y2);
            plt.Axis(x1, x2, y1, y2);
            this.Close();
        }

        private void LblGitHub_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/swharden/ScottPlot");
        }

        private void LblGitHub_MouseEnter(object sender, EventArgs e)
        {
            lblGitHub.Font = new Font(label1.Font.Name, label1.Font.SizeInPoints, FontStyle.Underline);
        }

        private void LblGitHub_MouseLeave(object sender, EventArgs e)
        {
            lblGitHub.Font = new Font(label1.Font.Name, label1.Font.SizeInPoints, FontStyle.Regular);
        }

        private void BtnExportCSV_Click(object sender, EventArgs e)
        {
            int plotObjectIndex = lbPlotObjects.SelectedIndex;
            var plottable = plt.GetPlottables()[plotObjectIndex];

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = $"Export CSV data for {plottable}";
            savefile.FileName = "data.csv";
            savefile.Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*";
            if (savefile.ShowDialog() == DialogResult.OK)
                plottable.SaveCSV(savefile.FileName);
        }

        private void RbQualityLow_CheckedChanged(object sender, EventArgs e)
        {
            plt.AntiAlias(rbQualityHigh.Checked, rbQualityHigh.Checked);
        }

        private void RbQualityHigh_CheckedChanged(object sender, EventArgs e)
        {
            plt.AntiAlias(rbQualityHigh.Checked, rbQualityHigh.Checked);
        }

        private void CbQualityLowWhileDragging_CheckedChanged(object sender, EventArgs e)
        {
            plt.mouseTracker.lowQualityWhileInteracting = cbQualityLowWhileDragging.Checked;
        }
    }
}
