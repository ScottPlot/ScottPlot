using ScottPlot.Plottable;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsFrameworkApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            formsPlot1.Plot.AddSignal(ScottPlot.DataGen.Sin(51, mult: 10));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Disabled")
                formsPlot1.Plot.AxisScaleLock(enable: false);
            else if (comboBox1.Text == "Preserve X")
                formsPlot1.Plot.AxisScaleLock(enable: true, scaleMode: ScottPlot.EqualScaleMode.PreserveX);
            else if (comboBox1.Text == "Preserve Y")
                formsPlot1.Plot.AxisScaleLock(enable: true, scaleMode: ScottPlot.EqualScaleMode.PreserveY);
            else if (comboBox1.Text == "Zoom Out")
                formsPlot1.Plot.AxisScaleLock(enable: true, scaleMode: ScottPlot.EqualScaleMode.ZoomOut);
            else
                throw new InvalidOperationException();

            formsPlot1.Render();
        }
    }
}
