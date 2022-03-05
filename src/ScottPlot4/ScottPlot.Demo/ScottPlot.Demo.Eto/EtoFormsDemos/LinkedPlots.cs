using System;
using Eto.Forms;
using ScottPlot.Eto;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    public partial class LinkedPlots : Form
    {
        readonly PlotView[] FormsPlots;

        public LinkedPlots()
        {
            InitializeComponent();
            formsPlot1.Plot.AddSignal(DataGen.Sin(51));
            formsPlot2.Plot.AddSignal(DataGen.Cos(51));

            // create a list of plot controls we can easily iterate through later
            FormsPlots = new PlotView[] { formsPlot1, formsPlot2 };
            foreach (var fp in FormsPlots)
                fp.AxesChanged += OnAxesChanged;
        }
        private void OnAxesChanged(object sender, EventArgs e)
        {
            if (cbLinked.Checked == false)
                return;

            PlotView changedPlot = (PlotView)sender;
            var newAxisLimits = changedPlot.Plot.GetAxisLimits();

            SuspendLayout();

            foreach (var fp in FormsPlots)
            {
                if (fp == changedPlot)
                    continue;

                // disable events briefly to avoid an infinite loop
                fp.Configuration.AxesChangedEventEnabled = false;
                fp.Plot.SetAxisLimits(newAxisLimits);
                fp.Refresh();
                fp.Configuration.AxesChangedEventEnabled = true;
            }

            ResumeLayout();
        }
    }
}
