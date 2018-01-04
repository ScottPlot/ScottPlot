using System;
using System.Windows.Forms;

namespace _03_interactive_sandbox
{
    public partial class form_interactive : Form
    {
        public form_interactive()
        {
            InitializeComponent();
            //NewData();
        }

        public void NewData()
        {
            // generate some data to plot
            int nPoints = 1000;
            Random rand = new Random();
            double scale = 1 + 10 * rand.NextDouble();
            double scaleX = (1 + 10 * rand.NextDouble())/nPoints;
            double offset = 100.0 * (rand.NextDouble() - .5);
            double[] Xs = ScottPlot.Generate.Sequence(nPoints, scaleX);
            double[] Ys = ScottPlot.Generate.Sine(nPoints, scale, scale, offset);

            // load the data into the user control
            ucInteractive1.Xs = Xs;
            ucInteractive1.Ys = Ys;
            ucInteractive1.SP.AxisAuto(Xs, Ys);

            // update the plot since we have new data to display
            Render();
        }

        public void Render()
        {
            ucInteractive1.GraphResize(); // also clears
            ucInteractive1.GraphRender();
        }

        private void btnNewData_Click(object sender, EventArgs e)
        {
            NewData();
        }
    }
}
