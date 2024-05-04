namespace Sandbox.WinForms3D;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        Plottables3D.Scatter3D scatter = new();

        formsPlot3d1.Plot3D.Plottables.Add(scatter);
    }
}
