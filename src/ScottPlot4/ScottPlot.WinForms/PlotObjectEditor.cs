using System;
using System.Windows.Forms;

namespace ScottPlot
{
    public partial class PlotObjectEditor : Form
    {
        readonly FormsPlot FormsPlot;

        public PlotObjectEditor(FormsPlot formsPlot)
        {
            InitializeComponent();
            FormsPlot = formsPlot;

            listBox1.Items.Clear();
            foreach (Plottable.IPlottable plottable in formsPlot.Plot.GetPlottables())
                listBox1.Items.Add(plottable);

            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is not null)
                propertyGrid1.SelectedObject = listBox1.SelectedItem;
        }

        private void PlotObjectEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormsPlot.Refresh();
        }
    }
}
