namespace ScottPlot.SharedPalettes;

internal class DemoSharedPalette : ISharedPalette
{
    public SharedColor[] Colors { get; } =
    {
        new SharedColor("#003366"),
        new SharedColor(0,255,0),
        new SharedColor("#3399AA"),
        new SharedColor(255,0,255),
    };

    public string Title => "Demo Shared Palette";

    public string Description => string.Empty;
}