using Eto.Forms;
using ScottPlot.Eto;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Sandbox.Eto
{
    public partial class MainWindow : Form
    {
        private EtoPlot etoPlot = null!;

        private void InitializeComponent()
        {
            etoPlot = new();
            this.Content = etoPlot;

            this.Width = 800;
            this.Height = 600;

            etoPlot.Width = 800;
            etoPlot.Height = 600;
        }
    }
}
