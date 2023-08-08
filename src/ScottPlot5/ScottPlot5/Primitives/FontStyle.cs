using System.Runtime.CompilerServices;

namespace ScottPlot;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class FontStyle
{
    /* Typefaces are cached to improve performance.
     * https://github.com/ScottPlot/ScottPlot/issues/2833
     * https://github.com/ScottPlot/ScottPlot/pull/2848
     */

    private SKTypeface? _typeface = null;

    public SKTypeface Typeface => _typeface ??= GetTypeFace();

    private string _name = Fonts.Default;
    public string Name
    {
        get => _name;
        set
        {
            bool fieldChanged = string.Compare(_name, value, StringComparison.InvariantCultureIgnoreCase) != 0;

            if (fieldChanged)
                ClearCachedTypeface();

            _name = value;
        }
    }

    private bool _bold = false;
    public bool Bold
    {
        get => _bold;
        set
        {
            bool fieldChanged = _bold != value;

            if (fieldChanged)
                ClearCachedTypeface();

            _bold = value;
        }
    }

    private bool _italic = false;
    public bool Italic
    {
        get => _italic;
        set
        {
            bool fieldChanged = (_italic != value);

            if (fieldChanged)
                ClearCachedTypeface();

            _italic = value;
        }
    }

    public Color Color { get; set; } = Colors.Black;
    public float Size { get; set; } = 12;
    public bool AntiAlias { get; set; } = true;

    private void ClearCachedTypeface()
    {
        _typeface = null;
    }

    private SKTypeface GetTypeFace()
    {
        SKFontStyleWeight weight = Bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal;
        SKFontStyleSlant slant = Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;
        SKFontStyleWidth width = SKFontStyleWidth.Normal;
        SKFontStyle skfs = new(weight, width, slant);
        return SKTypeface.FromFamilyName(Name, skfs);
    }
}
