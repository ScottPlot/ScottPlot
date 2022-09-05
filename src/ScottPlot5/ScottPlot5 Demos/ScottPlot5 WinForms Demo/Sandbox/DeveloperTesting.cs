using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot5_WinForms_Demo.Sandbox
{
    public partial class DeveloperTesting : Form, IDemoForm
    {
        public string Title => "Basic Sandbox";

        public string Description => "This demo demonstrates how to create a simple demo";

        public DeveloperTesting()
        {
            InitializeComponent();
            formsPlot1.Plot.Plottables.AddSignal(Generate.Sin(51));
            formsPlot1.Plot.Plottables.AddSignal(Generate.Cos(51));
        }
    }
}
