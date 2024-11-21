using ScottPlot;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace WinForms_Demo.Demos
{
    public partial class DetachedLegend : Form, IDemoWindow
    {
        public string Title => "Detachable Legend";

        public string Description => "Add an option to the right-click menu to display the legend in a pop-up window";

        public DetachedLegend()
        {
            InitializeComponent();

            int count = 20;
            for (int i = 0; i < 20; i++)
            {
                double[] ys = Generate.Sin(100, phase: i / (2.0 * count));
                var sig = formsPlot1.Plot.Add.Signal(ys);
                sig.Color = Colors.Category20[i];
                sig.LineWidth = 2;
                sig.LegendText = $"Line #{i + 1}";
            }

            formsPlot1.Menu?.Add("Detach Legend", LaunchDetachedLegend);
        }

        private void LaunchDetachedLegend(IPlotControl plotControl)
        {
            Form form = new()
            {
                Text = "Detached Legend",
                StartPosition = FormStartPosition.CenterScreen,
            };

            SKControl skControl = new()
            {
                Dock = DockStyle.Fill
            };

            skControl.PaintSurface += (s, e) =>
            {
                PixelSize size = new(skControl.Width, skControl.Height);
                PixelRect rect = new(Pixel.Zero, size);
                formsPlot1.Plot.Legend.Render(e.Surface.Canvas, rect, Alignment.MiddleCenter);
            };

            form.Controls.Add(skControl);
            form.Show();
        }
    }
}
