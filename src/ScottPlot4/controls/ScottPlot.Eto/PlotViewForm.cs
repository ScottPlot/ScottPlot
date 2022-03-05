using Eto.Forms;

namespace ScottPlot.Eto
{
    public partial class PlotViewForm : Form
    {
        public PlotViewForm(Plot plot, int windowWidth = 600, int windowHeight = 400, string windowTitle = "ScottPlot Viewer")
        {
            InitializeComponent();

            Width = windowWidth;
            Height = windowHeight;

            Title = windowTitle;

            PlotView = new PlotView();
            PlotView.Reset(plot);
            PlotView.Refresh();

            Content = PlotView;
        }

        public PlotView? PlotView { get; set; }
    }
}
