using Eto.Forms;
using ScottPlot.Eto;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eto
{
    public partial class MainWindow : Form
    {
        private EtoPlot etoPlot = null!;

        private void InitializeComponent()
        {
            etoPlot = new();
        }
    }
}
