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

            formsPlot1.LeftClickedPlottable += FormsPlot1_LeftClickedPlottable;

            formsPlot1.Plot.AddSignal(DataGen.Sin(51));
            formsPlot1.Plot.AddSignal(DataGen.Cos(51));

            var vl = formsPlot1.Plot.AddVerticalLine(25);
            vl.DragEnabled = true;

            formsPlot1.Plot.AddTooltip("clickable", 30, .6);

            formsPlot1.Refresh();
        }

        private void FormsPlot1_LeftClickedPlottable(object sender, EventArgs e)
        {
            if (sender is ScottPlot.Plottable.Tooltip tt)
            {
                Random rand = new();
                Color randomColor = Color.FromArgb(255, rand.Next(256), rand.Next(256), rand.Next(256));
                tt.Color = randomColor;
                formsPlot1.Refresh();
            }
        }
    }
}
