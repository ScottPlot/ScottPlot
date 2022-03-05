using System;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class LinkedPlots : Form
    {
        readonly FormsPlot[] FormsPlots;

        public LinkedPlots()
        {
            InitializeComponent();
            formsPlot1.Plot.AddSignal(DataGen.Sin(51));
            formsPlot2.Plot.AddSignal(DataGen.Cos(51));

            formsPlot1.Refresh();
            formsPlot2.Refresh();

            // create a list of plot controls we can easily iterate through later
            FormsPlots = new FormsPlot[] { formsPlot1, formsPlot2 };
            foreach (var fp in FormsPlots)
                fp.AxesChanged += OnAxesChanged;
        }

        private void OnAxesChanged(object sender, EventArgs e)
        {
            if (cbLinked.Checked == false)
                return;

            FormsPlot changedPlot = (FormsPlot)sender;
            var newAxisLimits = changedPlot.Plot.GetAxisLimits();

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
        }
    }
}
