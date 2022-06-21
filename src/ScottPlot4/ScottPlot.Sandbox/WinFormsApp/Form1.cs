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
        readonly ScottPlot.Plottable.BarSeries BarSeries;
        ScottPlot.Plottable.Bar BarUnderMouse = null;

        public Form1()
        {
            InitializeComponent();

            formsPlot1.MouseMove += FormsPlot1_MouseMove;

            Random rand = new(0);
            List<ScottPlot.Plottable.Bar> bars = new();
            for (int i = 0; i < 10; i++)
            {
                ScottPlot.Plottable.Bar bar = new()
                {
                    Value = rand.Next(25, 100),
                    Position = i,
                    FillColor = ScottPlot.Palette.Category10.GetColor(i),
                };
                bar.Font.Size = 18;
                bar.Font.Bold = true;
                bars.Add(bar);
            };

            BarSeries = formsPlot1.Plot.AddBarSeries(bars);
            formsPlot1.Plot.SetAxisLimitsY(0, 120);
            formsPlot1.Refresh();
        }

        private void FormsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            (double x, double y) = formsPlot1.Plot.GetCoordinate(e.X, e.Y);
            Coordinate coordinate = new(x, y);
            ScottPlot.Plottable.Bar bar = BarSeries.GetBar(coordinate);

            if (bar is null)
            {
                Text = $"X={x:N2} Y={y:N2} Mouse over nothing";

                if (BarUnderMouse is not null)
                {
                    // clear old bar
                    BarUnderMouse.LineWidth = 0;
                    BarUnderMouse.Label = null;
                    BarUnderMouse = null;
                    formsPlot1.Refresh();
                }
            }
            else
            {
                int barIndex = BarSeries.Bars.IndexOf(bar);
                Text = $"X={x:N2} Y={y:N2} Mouse over bar index {barIndex} (Value={bar.Value})";

                if (BarUnderMouse != bar)
                {
                    // bar changed
                    BarUnderMouse = bar;
                    BarUnderMouse.LineWidth = 2;
                    BarUnderMouse.Label = BarUnderMouse.Value.ToString();
                    formsPlot1.Refresh();
                }
            }
        }
    }
}
