namespace ScottPlot.SharedPalettes;

internal class DemoSharedPalette : ISharedPalette
{
    public SharedColor[] Colors { get; } =
    {
        SharedColor.FromHex("#003366"),
        new SharedColor(0,255,0),
        SharedColor.FromHex("#336699AA"),
        new SharedColor(255,0,255),
    };

    public string Title => "Demo Shared Palette";

    public string Description => string.Empty;
}