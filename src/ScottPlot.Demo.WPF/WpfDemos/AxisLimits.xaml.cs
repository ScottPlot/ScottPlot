﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    /// <summary>
    /// Interaction logic for AxisLimits.xaml
    /// </summary>
    public partial class AxisLimits : Window
    {
        public AxisLimits()
        {
            InitializeComponent();
            wpfPlot1.plt.PlotSignal(DataGen.Sin(51));
            wpfPlot1.plt.PlotSignal(DataGen.Cos(51));
            wpfPlot1.plt.AxisAuto();
            wpfPlot1.plt.AxisBounds(0, 50, -1, 1);
            wpfPlot1.Render();
        }
    }
}
