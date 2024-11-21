using ScottPlot;

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

            formsPlot1.Plot.Legend.OutlineWidth = 0;
            formsPlot1.Plot.Legend.BackgroundColor = ScottPlot.Color.FromColor(SystemColors.Control);
            ScottPlot.Image legendImage = formsPlot1.Plot.GetLegendImage();
            byte[] legendBitmapBytes = legendImage.GetImageBytes(ImageFormat.Bmp);
            MemoryStream ms = new(legendBitmapBytes);
            Bitmap bmp = new(ms);

            // add menu items with custom actions
            formsPlot1.Menu?.Add("Detached Legend", (formsplot1) =>
            {
               // To be replaced with 
            });

        }
    }
}
