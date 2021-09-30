using System;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class Styles : Form
    {
        public Styles()
        {
            InitializeComponent();
            listBox1.Items.AddRange(Style.GetStyles());

            formsPlot1.Plot.AddSignal(DataGen.Sin(51));
            formsPlot1.Plot.AddSignal(DataGen.Cos(51));
            formsPlot1.Plot.XLabel("Horizontal Axis");
            formsPlot1.Plot.YLabel("Vertical Axis");
            formsPlot1.Plot.Title("Default Style");
            formsPlot1.Refresh();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is null)
                return;

            ScottPlot.Styles.IStyle style = (ScottPlot.Styles.IStyle)listBox1.SelectedItem;
            formsPlot1.Plot.Style(style);
            formsPlot1.Plot.Title(style.ToString());
            formsPlot1.Refresh();
        }
    }
}
