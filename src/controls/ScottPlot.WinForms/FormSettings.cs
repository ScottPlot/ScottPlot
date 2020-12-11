using ScottPlot.Plottable;
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
            PopualteGuiFromPlot();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {

        }

        private void PopualteGuiFromPlot()
        {
            // vertical axis
            //tbYlabel.Text = plt.GetSettings(false).YAxis.AxTitle.Label;
            var limits = plt.GetAxisLimits();
            tbY2.Text = Math.Round(limits.YMax, 4).ToString();
            tbY1.Text = Math.Round(limits.YMin, 4).ToString();
            //cbYminor.Checked = plt.GetSettings().YAxis.TickCollection.displayYminor;
            //cbYdateTime.Checked = plt.GetSettings().ticks.y.dateFormat;

            // horizontal axis
            //tbXlabel.Text = plt.GetSettings(false).XAxis.AxTitle.Label;
            tbX2.Text = Math.Round(limits.XMax, 4).ToString();
            tbX1.Text = Math.Round(limits.XMin, 4).ToString();
            //cbXminor.Checked = plt.GetSettings().ticks.displayXminor;
            //cbXdateTime.Checked = plt.GetSettings().ticks.x.dateFormat;

            // tick display options
            //cbTicksOffset.Checked = plt.GetSettings().ticks.useOffsetNotation;
            //cbTicksMult.Checked = plt.GetSettings().ticks.useMultiplierNotation;
            //cbGrid.Checked = plt.GetSettings(false).XAxis.AxTicks.MajorGridStyle != LineStyle.None;

            // legend
            cbLegend.Checked = plt.GetSettings(false).CornerLegend.IsVisible;

            // image quality
            //rbQualityLow.Checked = !plt.GetSettings().misc.antiAliasData;
            //rbQualityHigh.Checked = plt.GetSettings().misc.antiAliasData;
            //cbQualityLowWhileDragging.Checked = plt.mouseTracker.lowQualityWhileInteracting;

            // list of plottables
            lbPlotObjects.Items.Clear();
            foreach (var plotObject in plt.GetPlottables())
                lbPlotObjects.Items.Add(plotObject);

            // list of color styles
            cbStyle.Items.AddRange(Enum.GetNames(typeof(Style)));
        }

        private void BtnFitDataY_Click(object sender, EventArgs e)
        {
            plt.AxisAutoY();
            PopualteGuiFromPlot();
        }

        private void BtnFitDataX_Click(object sender, EventArgs e)
        {
            plt.AxisAutoX();
            PopualteGuiFromPlot();
        }

        private void btnCopyCSV_Click(object sender, EventArgs e)
        {
            int plotObjectIndex = lbPlotObjects.SelectedIndex;
            IExportable plottable = (IExportable)plt.GetPlottables()[plotObjectIndex];
            Clipboard.SetText(plottable.GetCSV());
        }

        private void BtnExportCSV_Click(object sender, EventArgs e)
        {
            int plotObjectIndex = lbPlotObjects.SelectedIndex;
            IExportable plottable = (IExportable)plt.GetPlottables()[plotObjectIndex];

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = $"Export CSV data for {plottable}";
            savefile.FileName = "data.csv";
            savefile.Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*";
            if (savefile.ShowDialog() == DialogResult.OK)
                plottable.SaveCSV(savefile.FileName);
        }

        private void LbPlotObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbPlotObjects.Items.Count > 0 && lbPlotObjects.SelectedItem != null)
            {
                int plotObjectIndex = lbPlotObjects.SelectedIndex;
                var plottable = plt.GetPlottables()[plotObjectIndex];

                btnExportCSV.Enabled = plottable is IExportable;
                btnCopyCSV.Enabled = plottable is IExportable;

                // don't allow labels to be changed (after new legend system)
                //tbLabel.Enabled = true;
                //tbLabel.Text = plottable.ToString();
                tbLabel.Text = "editing disabled";
            }
            else
            {
                btnExportCSV.Enabled = false;
                btnCopyCSV.Enabled = false;
                tbLabel.Enabled = false;
            }
        }

        private void TbLabel_TextChanged(object sender, EventArgs e)
        {
            // don't allow labels to be changed (after new legend system)
            //int plotObjectIndex = lbPlotObjects.SelectedIndex;
            //var plottable = plt.GetPlottables()[plotObjectIndex];
            //plottable.label = tbLabel.ToString;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var settings = plt.GetSettings();

            // vertical axis
            plt.YLabel(tbYlabel.Text);
            plt.YAxis.MinorLogScale(cbYminor.Checked);
            plt.YAxis.DateTimeFormat(cbYdateTime.Checked);
            double y1, y2;
            double.TryParse(tbY1.Text, out y1);
            double.TryParse(tbY2.Text, out y2);
            plt.SetAxisLimits(yMin: y1, yMax: y2);

            // horizontal axis
            plt.XLabel(tbXlabel.Text);
            plt.YAxis.MinorLogScale(cbXminor.Checked);
            plt.YAxis.DateTimeFormat(cbXdateTime.Checked);
            double x1, x2;
            double.TryParse(tbX1.Text, out x1);
            double.TryParse(tbX2.Text, out x2);
            plt.SetAxisLimits(xMin: x1, xMax: x2);

            // tick display options
            plt.XAxis.TickLabelNotation(offset: cbTicksOffset.Checked, multiplier: cbTicksMult.Checked);
            plt.YAxis.TickLabelNotation(offset: cbTicksOffset.Checked, multiplier: cbTicksMult.Checked);

            // image quality
            //plt.AntiAlias(figure: rbQualityHigh.Checked, data: rbQualityHigh.Checked);
            //plt.mouseTracker.lowQualityWhileInteracting = cbQualityLowWhileDragging.Checked;

            // misc
            plt.Grid(enable: cbGrid.Checked);
            plt.Legend(enable: cbLegend.Checked);
            if (cbStyle.Text != "")
            {
                Style newStyle = (Style)Enum.Parse(typeof(Style), cbStyle.Text);
                plt.Style(newStyle);
            }

            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnTighten_Click(object sender, EventArgs e)
        {

        }
    }
}
