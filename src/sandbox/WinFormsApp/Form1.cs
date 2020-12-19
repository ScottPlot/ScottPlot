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
        private readonly ScottPlot.FormsPlot formsPlot1;

        public Form1()
        {
            InitializeComponent();

            // add this manually because the .NET winforms designer is still in preview mode
            // and the FormsPlot user control typically does not appear in the toolbox
            formsPlot1 = new ScottPlot.FormsPlot() { Dock = DockStyle.Fill };
            Controls.Add(formsPlot1);

            var xs = new double[] { 1, 2, 3, 4, 5 };
            var ys = new double[] { 1, 4, 9, 16, 25 };
            formsPlot1.plt.AddScatter(xs, ys);

            var vline = formsPlot1.plt.AddVerticalLine(3.5);
            vline.LineWidth = 3;
            vline.DragEnabled = true;
            vline.Dragged += new EventHandler(OnDragLine);

            var vspan = formsPlot1.plt.AddVerticalSpan(10, 20);
            vspan.DragEnabled = true;
            vspan.Dragged += new EventHandler(OnDragSpan);

            formsPlot1.Render();
        }

        private void OnDragLine(object sender, EventArgs e)
        {
            var line = sender as ScottPlot.Plottable.VLine;
            Text = $"Line X={line.X}";
        }

        private void OnDragSpan(object sender, EventArgs e)
        {
            var span = sender as ScottPlot.Plottable.VSpan;
            Text = $"Span Y={span.Y1} Y2={span.Y2}";
        }
    }
}
