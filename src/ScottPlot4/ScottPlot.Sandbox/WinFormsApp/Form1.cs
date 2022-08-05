using ScottPlot;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        ScottPlot.Plottable.VLine VLine1;
        ScottPlot.Plottable.VLine VLine2;
        ScottPlot.Plottable.HLine HLine1;
        ScottPlot.Plottable.HLine HLine2;

        public Form1()
        {
            InitializeComponent();

            formsPlot1.Plot.AddSignal(DataGen.Sin(51), 1, Color.Black);
            formsPlot1.Plot.AddSignal(DataGen.Cos(51), 1, Color.Gray);

            VLine1 = formsPlot1.Plot.AddVerticalLine(23, Color.Blue);
            VLine1.LineWidth = 2;
            VLine1.PositionLabel = true;
            VLine1.DragEnabled = true;
            VLine1.PositionLabelBackground = VLine1.Color;
            VLine1.PositionLabelFont.Size = 16;

            VLine2 = formsPlot1.Plot.AddVerticalLine(24, Color.Red);
            VLine2.LineWidth = 2;
            VLine2.PositionLabel = true;
            VLine2.DragEnabled = true;
            VLine2.PositionLabelBackground = VLine2.Color;
            VLine2.PositionLabelFont.Size = 16;

            HLine1 = formsPlot1.Plot.AddHorizontalLine(.2, Color.Green);
            HLine1.DragEnabled = true;
            HLine1.PositionLabel = true;
            HLine1.PositionLabelBackground = HLine1.Color;

            HLine2 = formsPlot1.Plot.AddHorizontalLine(.4, Color.Orange);
            HLine2.DragEnabled = true;
            HLine2.PositionLabel = true;
            HLine2.PositionLabelBackground = HLine2.Color;

            formsPlot1.PlottableDragged += FormsPlot1_PlottableDragged;

            formsPlot1.Plot.Layout(right: 40);
            FormsPlot1_PlottableDragged(null, null);
            formsPlot1.Refresh();
        }

        private void FormsPlot1_PlottableDragged(object sender, EventArgs e)
        {
            if (VLine1.X > VLine2.X)
            {
                VLine1.PositionLabelAlignmentX = ScottPlot.HorizontalAlignment.Left;
                VLine2.PositionLabelAlignmentX = ScottPlot.HorizontalAlignment.Right;
            }
            else
            {
                VLine1.PositionLabelAlignmentX = ScottPlot.HorizontalAlignment.Right;
                VLine2.PositionLabelAlignmentX = ScottPlot.HorizontalAlignment.Left;
            }

            if (HLine1.Y > HLine2.Y)
            {
                HLine1.PositionLabelAlignmentY = ScottPlot.VerticalAlignment.Lower;
                HLine2.PositionLabelAlignmentY = ScottPlot.VerticalAlignment.Upper;
            }
            else
            {
                HLine1.PositionLabelAlignmentY = ScottPlot.VerticalAlignment.Upper;
                HLine2.PositionLabelAlignmentY = ScottPlot.VerticalAlignment.Lower;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            VLine1.PositionLabelOppositeAxis = checkBox1.Checked;
            VLine2.PositionLabelOppositeAxis = checkBox1.Checked;
            HLine1.PositionLabelOppositeAxis = checkBox1.Checked;
            HLine2.PositionLabelOppositeAxis = checkBox1.Checked;
            formsPlot1.Refresh();
        }
    }
}
