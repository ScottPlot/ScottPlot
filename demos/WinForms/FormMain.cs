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
                //btnPlotTypes_Click(null, null);
                //btnDraggableAxisLines_Click(null, null);
                //btnSignal_Click(null, null);
                //btnTimeAxis_Click(null, null);
                //btnFinancial_Click(null, null);
                //btnRegression_Click(null, null);
                //btnIncoming_Click(null, null);
                //btnTickTester_Click(null, null);
                //btnBoxAndWhisker_Click(null, null);
                btnDateTimeAxis_Click(null, null);
            }
        }

        private void lblGitHubUrl_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/swharden/ScottPlot");
        }

        private void btnPlotTypes_Click(object sender, EventArgs e)
        {
            new FormPlotTypes().ShowDialog();
        }

        private void btnQuickstart_Click(object sender, EventArgs e)
        {
            new FormQuickstart().ShowDialog();
        }

        private void btnBenchmark_Click(object sender, EventArgs e)
        {
            new FormBenchmark().ShowDialog();
        }

        private void btnCustomGrid_Click(object sender, EventArgs e)
        {
            new FormCustomGrid().ShowDialog();
        }

        private void btnDraggableAxisLines_Click(object sender, EventArgs e)
        {
            new FormDraggableAxisLines().ShowDialog();
        }

        private void btnScatterErrorbars_Click(object sender, EventArgs e)
        {
            new FormErrorbars().ShowDialog();
        }

        private void btnExtremeAxes_Click(object sender, EventArgs e)
        {
            new FormExtremeAxes().ShowDialog();
        }

        private void btnFinancial_Click(object sender, EventArgs e)
        {
            new FormFinancial().ShowDialog();
        }

        private void btnHistogram_Click(object sender, EventArgs e)
        {
            new FormHistogram().ShowDialog();
        }

        private void btnAnimated_Click(object sender, EventArgs e)
        {
            new FormAnimatedSine().ShowDialog();
        }

        private void btnGrowingArray_Click(object sender, EventArgs e)
        {
            new FormGrowingArray().ShowDialog();
        }

        private void btnGrowingCircular_Click(object sender, EventArgs e)
        {
            new FormGrowingCircular().ShowDialog();
        }

        private void btnGrowingRoll_Click(object sender, EventArgs e)
        {
            new FormGrowingRoll().ShowDialog();
        }

        private void btnBillion_Click(object sender, EventArgs e)
        {
            new FormBillion().ShowDialog();
        }

        private void btnHover_Click(object sender, EventArgs e)
        {
            new FormHoverValue().ShowDialog();
        }

        private void btnSignalConst_Click(object sender, EventArgs e)
        {
            new FormSignalConst().ShowDialog();
        }

        private void btnBarGraph_Click_1(object sender, EventArgs e)
        {
            new FormBarGraph().ShowDialog();
        }

        private void btnLegend_Click(object sender, EventArgs e)
        {
            new FormLegend().ShowDialog();
        }

        private void btnTickTester_Click(object sender, EventArgs e)
        {
            new FormTickTester().ShowDialog();
        }

        private void btnDateTimeAxis_Click(object sender, EventArgs e)
        {
            new FormDateTimeAxis().ShowDialog();
        }

        private void btnUserControlSettings_Click(object sender, EventArgs e)
        {
            new FormUserControlSettings().ShowDialog();
        }

        private void btnWavFileViewer_Click(object sender, EventArgs e)
        {
            new FormWavViewer().ShowDialog();
        }

        private void btnPadding_Click(object sender, EventArgs e)
        {
            new FormPadding().ShowDialog();
        }

        private void btnLinkedPlots_Click(object sender, EventArgs e)
        {
            new FormLinkedPlots().ShowDialog();
        }

        private void btnCustomGrid_Click_1(object sender, EventArgs e)
        {
            new FormCustomGrid().ShowDialog();
        }

        private void btnSignal_Click(object sender, EventArgs e)
        {
            new FormSignal().ShowDialog();
        }

        private void btnOverride_Click(object sender, EventArgs e)
        {
            new FormOverride().ShowDialog();
        }

        private void btnToggleVisibility_Click(object sender, EventArgs e)
        {
            new FormToggleVis().ShowDialog();
        }

        private void SignalDistribution_Click_1(object sender, EventArgs e)
        {
            new FormSignalDistribution().ShowDialog();
        }

        private void btnRegression_Click(object sender, EventArgs e)
        {
            new FormRegression().ShowDialog();
        }

        private void btnIncoming_Click(object sender, EventArgs e)
        {
            new FormIncomingData().ShowDialog();
        }
    }
}
