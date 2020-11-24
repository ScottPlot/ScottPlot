﻿using System;
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
    public partial class AxisLimits : Form
    {
        public AxisLimits()
        {
            InitializeComponent();
            formsPlot1.plt.PlotSignal(DataGen.Sin(51));
            formsPlot1.plt.PlotSignal(DataGen.Cos(51));
            formsPlot1.plt.AxisAuto();
            formsPlot1.plt.AxisBounds(0, 50, -1, 1);
            formsPlot1.Render();
        }
    }
}
