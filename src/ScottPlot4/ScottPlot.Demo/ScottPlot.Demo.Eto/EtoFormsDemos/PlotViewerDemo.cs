﻿using System;
using Eto.Forms;

namespace ScottPlot.Demo.Eto.EtoFormsDemos
{
    public partial class PlotViewerDemo : Form
    {
        readonly Random rand = new Random();

        public PlotViewerDemo()
        {
            InitializeComponent();

            btnLaunchRandomSine.Click += BtnLaunchRandomSine_Click;
            btnLaunchRandomWalk.Click += BtnLaunchRandomWalk_Click;
        }

        private void BtnLaunchRandomWalk_Click(object sender, EventArgs e)
        {
            int pointCount = (int)nudWalkPoints.Value;
            double[] randomWalkData = DataGen.RandomWalk(rand, pointCount);

            var plt = new ScottPlot.Plot();
            plt.AddSignal(randomWalkData);
            plt.Title($"{pointCount} Random Walk Points");

            var plotViewer = new ScottPlot.Eto.PlotViewForm(plt, 500, 300, "Random Walk Data");
            plotViewer.PlotView.Configuration.Quality = Control.QualityMode.High; // customize as desired
            plotViewer.Owner = this; // so it closes if this window closes
            plotViewer.Show(); // or ShowDialog() for a blocking window
        }

        private void BtnLaunchRandomSine_Click(object sender, EventArgs e)
        {
            int sinCount = (int)nudSineCount.Value;
            var plt = new ScottPlot.Plot();
            for (int i = 0; i < sinCount; i++)
            {
                double[] randomSinValues = DataGen.Sin(50, rand.NextDouble() * 10, rand.NextDouble(), rand.NextDouble(), rand.NextDouble() * 100);
                plt.AddSignal(randomSinValues);
            }
            plt.Title($"{sinCount} Random Sine Waves");

            var plotViewer = new ScottPlot.Eto.PlotViewForm(plt, 500, 300, "Random Walk Data");
            plotViewer.PlotView.Configuration.Quality = Control.QualityMode.High; // customize as desired
            plotViewer.Owner = this; // so it closes if this window closes
            plotViewer.Show(); // or ShowDialog() for a blocking window
        }
    }
}
