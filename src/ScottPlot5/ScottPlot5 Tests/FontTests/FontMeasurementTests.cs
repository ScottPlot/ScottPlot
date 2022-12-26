namespace ScottPlotTests.FontTests;

internal class FontMeasurementTests
{
    [Test]
    public void Test_String_Measurement()
    {
        string text = "Hello, World";
        Font font = FontService.GetSystemDefaultMonospaceFont(16);
        PixelSize size = FontService.MeasureString(font, text);
        Console.WriteLine(size);
    }
}
