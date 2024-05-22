using Sandbox.WinForms3D.Primitives3D;

namespace Sandbox.WinForms3D;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        Plottables3D.Scatter3D scatter = new();
        formsPlot3d1.Plot3D.Plottables.Add(scatter);

        Point3D location = new(.25, .25, 0);
        Size3D size = new(.2, .2, .2);
        Plottables3D.Rectangle3D rect = new(location, size);
        formsPlot3d1.Plot3D.Plottables.Add(rect);
    }
}
