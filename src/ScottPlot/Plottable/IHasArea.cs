using System.Drawing;

namespace ScottPlot.Plottable
{
    public interface IHasArea
    {
        Color BorderColor { get; set; }
        float BorderLineWidth { get; set; }
        LineStyle BorderLineStyle { get; set; }
        Color HatchColor { get; set; }
        Drawing.HatchStyle HatchStyle { get; set; }
    }
}
