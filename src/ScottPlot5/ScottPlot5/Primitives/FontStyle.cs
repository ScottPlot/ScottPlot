using System.Runtime.CompilerServices;

namespace ScottPlot;

/// <summary>
/// This configuration object (reference type) permanently lives inside objects which require styling.
/// It is recommended to use this object as an init-only property.
/// </summary>
public class FontStyle
{
    protected bool SetField<T>(ref T fieldValue, T value)
    {
        if (Equals(fieldValue, value))
            return false;
        fieldValue = value;
        _typeface = null;
        return true;
    }
    protected SKTypeface GetTypeFace()
    {
        SKFontStyleWeight weight = Bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal;
        SKFontStyleSlant slant = Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright;
        SKFontStyleWidth width = SKFontStyleWidth.Normal;
        SKFontStyle skfs = new(weight, width, slant);
        return SKTypeface.FromFamilyName(Name, skfs);
    }

    public SKTypeface Typeface => _typeface ??= GetTypeFace();
    protected SKTypeface? _typeface = null;
    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }
    private string _name = Fonts.Default;
    public Color Color { get; set; } = Colors.Black;
    public bool Bold
    {
        get => _bold;
        set => SetField(ref _bold, value);
    }
    private bool _bold = false;
    public bool Italic
    {
        get => _italic;
        set => SetField(ref _italic, value);
    }
    private bool _italic = false;

    public float Size { get; set; } = 12;
    public bool AntiAlias { get; set; } = true;
}
