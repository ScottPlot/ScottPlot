using System;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            formsPlot1.Plot.AddSignal(ScottPlot.Generate.Sin(), label: "sin");
            formsPlot1.Plot.AddSignal(ScottPlot.Generate.Cos(), label: "cos");
            formsPlot1.Plot.Legend();
            formsPlot1.Render();
        }
    }
}
