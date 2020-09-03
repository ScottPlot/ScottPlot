using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
            Demos.Items.Add(new DemoForm(typeof(Examples.MultipleAxes1), "Multiple Axes #1", "Example plot displaying sample data using multiple Y axes"));
            Demos.Items.Add(new DemoForm(typeof(Examples.MultipleAxes2), "Multiple Axes #2", "Interact with plots that have an arbitrary numbers of axes"));
            
            Demos.SelectedIndex = 0;
        }

        private void Demos_SelectedIndexChanged(object sender, EventArgs e)
        {
            DemoForm demo = (DemoForm)Demos.SelectedItem;
            DemoTitle.Text = demo.Title;
            DemoDescription.Text = demo.Description;
        }

        private void LaunchDemo_Click(object sender, EventArgs e)
        {
            DemoForm demo = (DemoForm)Demos.SelectedItem;
            demo.Launch();
        }
    }
}
