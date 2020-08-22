using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsPlotSandbox
{
    public partial class FormSlowPan : Form
    {
        public FormSlowPan()
        {
            InitializeComponent();
            formsPlot1.plt.Grid(color: Color.Gray);
            checkBox2.Checked = formsPlot1.plt.GetSettings(false).HorizontalGridLines.SnapToNearestPixel;
            checkBox3.Checked = formsPlot1.plt.GetSettings(false).ticks.snapToNearestPixel;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            formsPlot1.plt.AxisPan(.001, .001);
            formsPlot1.Render();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.Grid(snapToNearestPixel: checkBox2.Checked);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.Ticks(snapToNearestPixel: checkBox3.Checked);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.Ticks(rulerModeX: checkBox4.Checked);
            formsPlot1.plt.Ticks(rulerModeY: checkBox4.Checked);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.plt.AntiAlias(
                figure: checkBox5.Checked,
                data: checkBox5.Checked);
        }
    }
}
