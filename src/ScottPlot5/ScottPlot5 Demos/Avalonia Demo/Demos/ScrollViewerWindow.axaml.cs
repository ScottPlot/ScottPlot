using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia_Demo.ViewModels.Demos;
using ScottPlot;
using ScottPlot.Avalonia;
using System;
using System.ComponentModel;

namespace Avalonia_Demo.Demos;

public class ScrollViewerDemo : IDemo
{
    public string Title => "Plot in a Scroll Viewer";

    public string Description => "How to switch between using the mouse wheel to " +
        "scroll up/down vs. zoom in/out";

    public Window GetWindow()
    {
        return new ScrollViewerWindow();
    }
}

public partial class ScrollViewerWindow : Window
{
    private ScrollViewerViewModel TypedDataContext => (DataContext as ScrollViewerViewModel) ?? throw new ArgumentException(nameof(DataContext));

    private readonly AvaPlot[] plots;
    public ScrollViewerWindow()
    {
        InitializeComponent();
        
        DataContext = new ScrollViewerViewModel();
        TypedDataContext.PropertyChanged += HandleDataContextChanged;

        plots = [AvaPlot1, AvaPlot2, AvaPlot3];

        foreach (var avaPlot in plots)
        {
            avaPlot.Plot.Add.Signal(Generate.RandomWalk(100));
            avaPlot.PointerWheelChanged += HandleMouseWheel;
        }
    }

    private void HandleMouseWheel(object? sender, PointerWheelEventArgs e)
    {
        // Don't let the scroll viewer scroll if we want plot zooming instead.
        e.Handled = TypedDataContext.ZoomAllowed;
    }

    private void HandleDataContextChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(TypedDataContext.ZoomAllowed))
        {
            UpdateMouseWheelAction();
        }
    }

    private void UpdateMouseWheelAction()
    {
        foreach (var avaPlot in plots)
        {
            if (!TypedDataContext.ZoomAllowed)

            {
                avaPlot.UserInputProcessor.RemoveAll<ScottPlot.Interactivity.UserActionResponses.MouseWheelZoom>();
            }
            else
            {
                avaPlot.UserInputProcessor.Reset();
            }
        }
    }
}
