using Eto.Forms;

namespace ScottPlot.Eto
{
    public partial class FormHelp : Form
    {
        public FormHelp()
        {
            InitializeComponent();
            lblVersion.Text = $"ScottPlot.Eto {Plot.Version}";
            lblMessage.Text = Control.ControlBackEnd.GetHelpMessage();
        }
    }
}
