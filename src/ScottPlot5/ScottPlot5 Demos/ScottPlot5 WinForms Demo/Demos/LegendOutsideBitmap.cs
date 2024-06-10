using ScottPlot;

namespace WinForms_Demo.Demos;

public partial class LegendOutsideBitmap : Form, IDemoWindow
{
    public string Title => "Legend Outside the Plot (Bitmap)";

    public string Description => "Demonstrates how to display the " +
        "legend outside the plot by obtaining it as a Bitmap and " +
        "displaying it outside the plot control anywhere in your window.";

    public LegendOutsideBitmap()
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
        pictureBox1.Image = bmp;
        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
        pictureBox1.BackColor = SystemColors.Control;
    }
}
