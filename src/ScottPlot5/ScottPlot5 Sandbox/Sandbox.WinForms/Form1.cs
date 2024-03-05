using ScottPlot;
using System.Diagnostics;

namespace Sandbox.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        var c1 = formsPlot1.Plot.Add.Circle(5, 5, 9);
        var c2 = formsPlot1.Plot.Add.Circle(10, 0, 9);
        var c3 = formsPlot1.Plot.Add.Circle(15, 10, 9);
        c1.FillStyle.Color = Colors.Red;
        c2.FillStyle.Color = c1.FillStyle.Color.Darken(.5f);
        c3.FillStyle.Color = c1.FillStyle.Color.Lighten(.5f);

        var c = Colors.Red;
        var rgb = c.ToStringRGB();
        (float h, float s, float l) = c.ToHSL();
        Debug.WriteLine($"RGB: {rgb} Hue: {h}, Saturation: {s}, Liminance: {l}");

        var cc = ScottPlot.Color.FromHSL(h, s, l);
        rgb = cc.ToStringRGB();
        (h, s, l) = cc.ToHSL();
        Debug.WriteLine($"RGB: {rgb} Hue: {h}, Saturation: {s}, Liminance: {l}");

        rgb = c1.FillStyle.Color.ToStringRGB();
        (h, s, l) = c1.FillStyle.Color.ToHSL();
        Debug.WriteLine($"RGB: {rgb} Hue: {h}, Saturation: {s}, Liminance: {l}");
        rgb = c2.FillStyle.Color.ToStringRGB();
        (h, s, l) = c2.FillStyle.Color.ToHSL();
        Debug.WriteLine($"RGB: {rgb} Hue: {h}, Saturation: {s}, Liminance: {l}");
        rgb = c3.FillStyle.Color.ToStringRGB();
        (h, s, l) = c3.FillStyle.Color.ToHSL();
        Debug.WriteLine($"RGB: {rgb} Hue: {h}, Saturation: {s}, Liminance: {l}");
        formsPlot1.Plot.SavePng("DarkenLighten.png", 240, 240);
    }
}
