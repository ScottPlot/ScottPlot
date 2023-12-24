using ScottPlot;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    List<ScottPlot.Plottables.Text> TextPlots = new();

    public Form1()
    {
        InitializeComponent();

        for (int x = 0; x < 50; x += 1)
        {
            string text = ((char)('A' + (x % 26))).ToString();
            Coordinates location = new(x, Math.Sin(x / 20.0));
            var tp = formsPlot1.Plot.Add.Text(text, location);
            TextPlots.Add(tp);
        }

        formsPlot1.Plot.RenderManager.RenderStarting += (object? s, RenderPack rp) =>
        {
            AxisLimits limits = rp.Plot.GetAxisLimits();
            double pxPerUnitX = rp.DataRect.Width / limits.HorizontalSpan;
            double pxPerUnitY = rp.DataRect.Height / limits.VerticalSpan;
            float pxPerUnit = (float)Math.Min(pxPerUnitX, pxPerUnitY);

            foreach (var textPlot in TextPlots)
            {
                textPlot.Label.FontSize = pxPerUnit;
            }
        };
    }
}
