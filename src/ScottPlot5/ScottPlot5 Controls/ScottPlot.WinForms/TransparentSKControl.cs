
using SkiaSharp.Views.Desktop;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

public class TransparentSKControl : SKControl
{
    public TransparentSKControl() : base()
    {
        // Enable transparent background
        this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
    }
}


