using System;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private readonly ScottPlot.FormsPlot formsPlot1;

        public Form1()
        {
            InitializeComponent();

            // add this manually because the .NET winforms designer is still in preview mode
            // and the FormsPlot user control typically does not appear in the toolbox
            formsPlot1 = new ScottPlot.FormsPlot() { Dock = DockStyle.Fill };
            Controls.Add(formsPlot1);

            int pointCount = 10;
            var rand = new Random(0);
            double[] xs = ScottPlot.DataGen.Random(rand, pointCount);
            double[] ys = ScottPlot.DataGen.Random(rand, pointCount);
            formsPlot1.Plot.AddScatter(xs, ys);
        }
    }
}
