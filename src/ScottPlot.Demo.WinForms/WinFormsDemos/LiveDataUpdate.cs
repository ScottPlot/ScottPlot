using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class LiveDataUpdate : Form
    {
        Random rand = new Random();
        double[] liveData = DataGen.Sin(100, oscillations: 2, mult: 20);

        public LiveDataUpdate()
        {
            InitializeComponent();

            // plot the data array only once
            formsPlot1.plt.PlotSignal(liveData);
            formsPlot1.plt.Axis(y1: -50, y2: 50);
            formsPlot1.Render();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // modify the underlying data
            for (int i = 0; i < liveData.Length; i++)
                liveData[i] += rand.NextDouble() - .5;

            // then ask for a render to redraw the new data
            formsPlot1.Render(skipIfCurrentlyRendering: true);
        }
    }
}
