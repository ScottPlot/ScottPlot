using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia_Demo.ViewModels.Demos;

namespace Avalonia_Demo.Demos;

public abstract partial class SimpleDemoWindow : Window
{
    public SimpleDemoWindow()
    {
        InitializeComponent();
        StartDemo();
    }

    public SimpleDemoWindow(string title) : this()
    {
        this.DataContext = new SimpleDemoViewModel(title);
    }

    protected abstract void StartDemo();
}
