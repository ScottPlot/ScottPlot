namespace ScottPlot.Plottables;

/// <summary>
/// It is recommended to use images that are correctly dimensioned during creation.
/// A scaling property is available for convenience, but produces undesirable results on scaling up.
/// This object retains the initial reference image used and is restored when resetting the scale if
/// the displayed marker becomes distorted.
/// </summary>
public class ImageMarker : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    private ScottPlot.Image _referenceImage;

    private ScottPlot.Image _displayImage;

    private Coordinates _location;
    public Coordinates Location
    {
        get { return _location; }
        set
        {
            _location = value;
            _displayImage = GetDisplayImage();
        }
    }

    private float _markerScale = 0.0f;
    public float MarkerScale
    {
        get { return _markerScale; }
        set
        {
            if (value > 0.0f)
            {
                _markerScale = value;
                _displayImage = GetDisplayImage();
            }
        }
    }

    public bool AntiAlias { get; set; } = true;

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public AxisLimits GetAxisLimits() => new(_location);

    public ImageMarker(Coordinates location, Image referenceImage, float scale)
    {
        _location = location;
        _markerScale = scale;
        _referenceImage = referenceImage;
        _displayImage = GetDisplayImage();
    }

    public void SetReferenceImage(ScottPlot.Image image)
    {
        _referenceImage = image;
        _displayImage = GetDisplayImage();
    }

    private ScottPlot.Image GetDisplayImage()
    {
        if (MarkerScale == 1.0f)
            return _referenceImage;

        int scaledWidth = Math.Max(1, (int)(_referenceImage.Width * MarkerScale));
        int scaledHeight = Math.Max(1, (int)(_referenceImage.Height * MarkerScale));

        using SKBitmap scaledBitmap = new(scaledWidth, scaledHeight);
        using SKCanvas canvas = new(scaledBitmap);
        canvas.Clear(SKColors.Transparent);

        SKRect sourceRect = new(0, 0, _referenceImage.Width, _referenceImage.Height);
        SKRect destRect = new(0, 0, scaledWidth, scaledHeight);
        canvas.DrawImage(_referenceImage.SKImageInternal, sourceRect, destRect);

        return new Image(scaledBitmap);
    }

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        //TODO: Fix the assumption that images are square
        PixelRect drawRect = new(
            center: Axes.GetPixel(_location),
            radius: _displayImage.Width / 2);

        using SKPaint paint = new();

        _displayImage.Render(rp.Canvas, drawRect, paint, AntiAlias);
    }
}
