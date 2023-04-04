using System.Windows.Forms;

namespace ScottPlot
{
    public partial class FormHelp : Form
    {
        public FormHelp()
        {
            InitializeComponent();
            Text = $"ScottPlot {Plot.Version} Help";
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = true;
            lblVersion.Text = $"ScottPlot.WinForms {Plot.Version}";
            lblMessage.Text = Control.ControlBackEnd.GetHelpMessage();
        }
    }
}
