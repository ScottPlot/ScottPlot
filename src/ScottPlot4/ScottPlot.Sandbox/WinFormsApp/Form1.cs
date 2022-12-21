using System;
using ScottPlot;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var func1 = new Func<double, double?>((x) => 5 * Math.Sin(x) * Math.Sin(x / 2));
            var func2 = new Func<double, double?>((x) => 4 * Math.Sin(x) * Math.Sin(x / 3) - 1);

            var fp1 = formsPlot1.Plot.AddFunction(func1);
            fp1.FillType = FillType.FillBelow;

            var fp2 = formsPlot1.Plot.AddFunction(func2);
            fp2.FillType = FillType.FillBelow;

            formsPlot1.Refresh();
        }
    }
}
