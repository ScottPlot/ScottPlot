using System.ComponentModel;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

[ToolboxItem(false)]
public partial class FormsPlotDesignerAlternative : UserControl
{
    public FormsPlotDesignerAlternative()
    {
        InitializeComponent();

        label1.Text = ScottPlot.Version.LongString;
        label2.Text = "Visual Studio Designer View";
        richTextBox1.Text = "It appears you are working inside Visual Studio and a " +
            "DLL required for rendering failed to load, so this placeholder will be displayed " +
            "in the designer but will be replaced by an interactive plot when the program is run.";
    }
}
