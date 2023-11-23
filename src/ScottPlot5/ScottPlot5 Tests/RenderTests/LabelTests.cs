using SkiaSharp;

namespace ScottPlotTests.RenderTests;

internal class LabelTests
{
    [Test]
    public void Test_Label_Basic()
    {
        ScottPlot.LabelExperimental lbl = new()
        {
            Text = "Test",
            PointSize = 5,
            BackColor = Colors.White.WithAlpha(.1),
            ForeColor = Colors.White.WithAlpha(.3),
            BorderColor = Colors.White.WithAlpha(.5),
            PointColor = Colors.Yellow,
            FontSize = 26,
        };

        SKSurface surface = Drawing.CreateSurface(500, 1000);
        SKCanvas canvas = surface.Canvas;
        canvas.Clear(SKColors.Navy);

        float[] rotations = { 0, 90, 180, 270 };
        Alignment[] alignments = Enum.GetValues<Alignment>();

        for (int i=0; i<alignments.Length; i++)
        {
            lbl.Alignment = alignments[i];
            for (int j = 0; j < rotations.Length; j++)
            {
                lbl.Rotation = rotations[j];

                float x = 100 + 100 * j;
                float y = 100 + 100 * i;
                lbl.Render(canvas, x, y);
            }
        }

        surface.SaveTestImage();
    }
}
