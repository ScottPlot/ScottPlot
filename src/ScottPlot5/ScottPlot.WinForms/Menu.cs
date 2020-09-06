using ScottPlot.WinForms.Examples;
using System;
using System.Windows.Forms;

namespace ScottPlot.WinForms
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            Demos.Items.Add(new Example(typeof(QuickStart), "QuickStart", "Simple, interactive scatter plots"));
            Demos.Items.Add(new Example(typeof(MultipleAxes1), "Multiple Axes #1", "Example plot displaying sample data using multiple Y axes"));
            Demos.Items.Add(new Example(typeof(MultipleAxes2), "Multiple Axes #2", "Interact with plots that have an arbitrary numbers of axes"));
            Demos.Items.Add(new Example(typeof(LinkedPlots), "Linked Plots", "Two controls which share an axis"));

            Demos.SelectedIndex = 0;
        }

        private void Demos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Example demo = (Example)Demos.SelectedItem;
            DemoTitle.Text = demo.Title;
            DemoDescription.Text = demo.Description;
        }

        private void LaunchDemo_Click(object sender, EventArgs e)
        {
            Example demo = (Example)Demos.SelectedItem;
            demo.Launch();
        }
    }
}
