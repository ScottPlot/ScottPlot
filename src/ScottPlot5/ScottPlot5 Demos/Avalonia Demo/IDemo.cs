using Avalonia.Controls;
using ScottPlot.Avalonia;

namespace Avalonia_Demo;

public interface IDemo
{
    string Title { get; }
    string Description { get; }
    public Window GetWindow();
}
