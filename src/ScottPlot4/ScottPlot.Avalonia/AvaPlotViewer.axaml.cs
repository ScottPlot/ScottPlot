using System;

using Avalonia.Controls;

namespace ScottPlot.Avalonia
{
    public partial class AvaPlotViewer : Window
    {
        public AvaPlot AvaPlot => avaPlot1;

        [Obsolete("The zero parameter constructor is not to be called")]
        public AvaPlotViewer()
        {
            throw new NotSupportedException("The zero parameter constructor is not to be called");
        }

        public AvaPlotViewer(ScottPlot.Plot plot, int windowWidth = 600, int windowHeight = 400, string windowTitle = "ScottPlot Viewer")
        {
            this.InitializeComponent();

            Width = windowWidth;
            Height = windowHeight;
            Title = windowTitle;

            plot.Resize(windowWidth, windowHeight);
            this.avaPlot1.Reset(plot);
            this.avaPlot1.Refresh();
        }

        public void SetWindowOwner(WindowBase owner)
        {
            this.Owner = owner;
        }
    }
}
