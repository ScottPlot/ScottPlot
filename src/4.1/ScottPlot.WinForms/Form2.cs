using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ScottPlot.WinForms
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            interactivePlot1.plt.PlotScatter(Generate.Consecutive(51), Generate.Sin(51));
            interactivePlot1.plt.PlotScatter(Generate.Consecutive(51), Generate.Cos(51));
        }
    }
}
