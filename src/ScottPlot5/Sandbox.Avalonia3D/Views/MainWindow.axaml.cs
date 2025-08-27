using Avalonia.Controls;
using ScottPlot.ScottPlot3D.Plottables3D;
using ScottPlot.ScottPlot3D.Primitives3D;

namespace Sandbox.Avalonia3D.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
            
        Scatter3D scatter = new();
        AvaPlot.Plot3D.Plottables.Add(scatter);

        Point3D location = new(.25, .25, 0);
        Size3D size = new(.2, .2, .2);
        Rectangle3D rect = new(location, size);
        AvaPlot.Plot3D.Plottables.Add(rect);

        AvaPlot.Plot3D.Scene.Position = new(-1, 1, -1);
    }
}
