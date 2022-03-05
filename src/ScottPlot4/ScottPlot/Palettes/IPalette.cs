/* 
 * A color set is a collection of colors, like a color palette.
 * 
 * System.Drawing.Color is intentionally avoided here to simplify 
 * porting to other rendering systems down the road.
 * 
 **/

namespace ScottPlot.Drawing
{
    public interface IPalette
    {
        (byte r, byte g, byte b) GetRGB(int index);
        int Count();
    }
}
