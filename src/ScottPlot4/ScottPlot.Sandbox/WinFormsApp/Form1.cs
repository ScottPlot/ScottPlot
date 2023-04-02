using System;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            formsPlot1.Plot.SetAxisLimits(-10, 10, -10, 10);

            formsPlot1.MouseMove += FormsPlot1_MouseMove;
        }

        private void FormsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            Text = $"{formsPlot1.Plot.GetCoordinate(e.X, e.Y)}";
        }
    }
}
