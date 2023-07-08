using System;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var cb = formsPlot1.Plot.AddColorbar();
            double[] fractions = { 0, .5, 1 };
            string[] labels = {
                "Testing long message 1",
                "Testing long message 2",
                "Testing long message 3",
            };
            cb.SetTicks(fractions, labels);
            cb.LabelIsVisible = true;
            cb.Label = "Awesome Colorbar";
            formsPlot1.Render();

            // automatic layout resizing must be called AFTER a render
            cb.ResizeLayout(formsPlot1.Plot);
            formsPlot1.Render();
        }
    }
}
