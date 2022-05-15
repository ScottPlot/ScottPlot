using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            formsPlot1.Configuration.EnablePlotObjectEditor = true;

            formsPlot1.Plot.AddScatter(DataGen.Consecutive(51), DataGen.Sin(51));
            formsPlot1.Plot.AddSignal(DataGen.Cos(51));
            formsPlot1.Plot.AddVerticalLine(25);
            formsPlot1.Plot.AddText("test 123", 20, .5);
            formsPlot1.Plot.AddMarker(15, -.5, MarkerShape.openCircle, 20);
            formsPlot1.Refresh();
        }
    }
}
