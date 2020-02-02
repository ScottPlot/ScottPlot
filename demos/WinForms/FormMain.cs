using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlotDemos
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            lblVersion.Text = ScottPlot.Tools.GetVersionString();

            if (Debugger.IsAttached)
            {
                btnPlotTypes_Click(null, null);
                //btnDraggableAxisLines_Click(null, null);
                //btnSignal_Click(null, null);
                //btnTimeAxis_Click(null, null);
                //btnFinancial_Click(null, null);
                //btnRegression_Click(null, null);
                //btnIncoming_Click(null, null);
                //btnTickTester_Click(null, null);
            }
        }

        private void lblGitHubUrl_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/swharden/ScottPlot");
        }

        private void btnPlotTypes_Click(object sender, EventArgs e)
        {
            using (var frm = new FormPlotTypes())
                frm.ShowDialog();
        }

        private void btnQuickstart_Click(object sender, EventArgs e)
        {
            using (var frm = new FormQuickstart())
                frm.ShowDialog();
        }

        private void btnBenchmark_Click(object sender, EventArgs e)
        {
            using (var frm = new FormBenchmark())
                frm.ShowDialog();
        }

        private void btnCustomGrid_Click(object sender, EventArgs e)
        {
            using (var frm = new FormCustomGrid())
                frm.ShowDialog();
        }

        private void btnDraggableAxisLines_Click(object sender, EventArgs e)
        {
            using (var frm = new FormDraggableAxisLines())
                frm.ShowDialog();
        }

        private void btnScatterErrorbars_Click(object sender, EventArgs e)
        {
            using (var frm = new FormErrorbars())
                frm.ShowDialog();
        }

        private void btnExtremeAxes_Click(object sender, EventArgs e)
        {
            using (var frm = new FormExtremeAxes())
                frm.ShowDialog();
        }

        private void btnFinancial_Click(object sender, EventArgs e)
        {
            using (var frm = new FormFinancial())
                frm.ShowDialog();
        }

        private void btnHistogram_Click(object sender, EventArgs e)
        {
            using (var frm = new FormHistogram())
                frm.ShowDialog();
        }

        private void btnAnimated_Click(object sender, EventArgs e)
        {
            using (var frm = new FormAnimatedSine())
                frm.ShowDialog();
        }

        private void btnGrowingArray_Click(object sender, EventArgs e)
        {
            using (var frm = new FormGrowingArray())
                frm.ShowDialog();
        }

        private void btnGrowingCircular_Click(object sender, EventArgs e)
        {
            using (var frm = new FormGrowingCircular())
                frm.ShowDialog();
        }

        private void btnGrowingRoll_Click(object sender, EventArgs e)
        {
            using (var frm = new FormGrowingRoll())
                frm.ShowDialog();
        }

        private void btnBillion_Click(object sender, EventArgs e)
        {
            using (var frm = new FormBillion())
                frm.ShowDialog();
        }

        private void btnHover_Click(object sender, EventArgs e)
        {
            using (var frm = new FormHoverValue())
                frm.ShowDialog();
        }

        private void btnSignalConst_Click(object sender, EventArgs e)
        {
            using (var frm = new FormSignalConst())
                frm.ShowDialog();
        }

        private void btnBarGraph_Click_1(object sender, EventArgs e)
        {
            using (var frm = new FormBarGraph())
                frm.ShowDialog();
        }

        private void btnLegend_Click(object sender, EventArgs e)
        {
            using (var frm = new FormLegend())
                frm.ShowDialog();
        }

        private void btnTickTester_Click(object sender, EventArgs e)
        {
            using (var frm = new FormTickTester())
                frm.ShowDialog();
        }

        private void btnTimeAxis_Click(object sender, EventArgs e)
        {
            using (var frm = new FormTimeAxis())
                frm.ShowDialog();
        }

        private void btnUserControlSettings_Click(object sender, EventArgs e)
        {
            using (var frm = new FormUserControlSettings())
                frm.ShowDialog();
        }

        private void btnWavFileViewer_Click(object sender, EventArgs e)
        {
            using (var frm = new FormWavViewer())
                frm.ShowDialog();
        }

        private void btnPadding_Click(object sender, EventArgs e)
        {
            using (var frm = new FormPadding())
                frm.ShowDialog();
        }

        private void btnLinkedPlots_Click(object sender, EventArgs e)
        {
            using (var frm = new FormLinkedPlots())
                frm.ShowDialog();
        }

        private void btnCustomGrid_Click_1(object sender, EventArgs e)
        {
            using (var frm = new FormCustomGrid())
                frm.ShowDialog();
        }

        private void btnSignal_Click(object sender, EventArgs e)
        {
            using (var frm = new FormSignal())
                frm.ShowDialog();
        }

        private void btnOverride_Click(object sender, EventArgs e)
        {
            using (var frm = new FormOverride())
                frm.ShowDialog();
        }

        private void btnToggleVisibility_Click(object sender, EventArgs e)
        {
            using (var frm = new FormToggleVis())
                frm.ShowDialog();
        }

        private void SignalDistribution_Click_1(object sender, EventArgs e)
        {
            using (var frm = new FormSignalDistribution())
                frm.ShowDialog();
        }

        private void btnRegression_Click(object sender, EventArgs e)
        {
            using (var frm = new FormRegression())
                frm.ShowDialog();
        }

        private void btnIncoming_Click(object sender, EventArgs e)
        {
            using (var frm = new FormIncomingData())
                frm.ShowDialog();
        }
    }
}
