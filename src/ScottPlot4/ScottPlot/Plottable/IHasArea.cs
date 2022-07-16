using System.Drawing;

namespace ScottPlot.Plottable
{
    public interface IHasArea
    {
        System.Drawing.Color BorderColor { get; set; }
        float BorderLineWidth { get; set; }
        LineStyle BorderLineStyle { get; set; }
        System.Drawing.Color HatchColor { get; set; }
        Drawing.HatchStyle HatchStyle { get; set; }
    }
}
