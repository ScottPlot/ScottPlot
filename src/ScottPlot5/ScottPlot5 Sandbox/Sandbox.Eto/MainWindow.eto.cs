using Eto.Forms;
using ScottPlot.Eto;

namespace Sandbox.Eto;

public partial class MainWindow : Form
{
    private readonly EtoPlot EtoPlot1 = new();

    private void InitializeComponent()
    {
        Content = EtoPlot1;
        Width = 800;
        Height = 600;
    }
}
