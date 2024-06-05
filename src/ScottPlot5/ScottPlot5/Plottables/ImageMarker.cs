
namespace ScottPlot.Plottables
{
    /// <summary>
    /// It is recommended to use images that are correctly dimensioned during creation.
    /// A scaling property is available for conveniance, but produces undesirable results on scaling up.
    /// This object retains the intial reference image used and is restored when reseting the scale if
    /// the displayed marker becomes distorted.
    /// </summary>
    public class ImageMarker : IPlottable
    {
        public ImageMarker(Coordinates location, Image referenceImage, float scale)
        {
            _location = location;

            _markerScale = scale;

            _referenceImage = referenceImage;
            _displayImage = referenceImage;

            _UpdateDisplayImage();
        }

        private ScottPlot.Image _referenceImage;
        private ScottPlot.Image _displayImage;
        public void SetReferenceImage(ScottPlot.Image image)
        {
            _referenceImage = image;
            _UpdateDisplayImage();
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
                    _UpdateDisplayImage();
                }
            }
        }

        private Coordinates _location;
        public Coordinates Location
        {
            get { return _location; }
            set 
            { 
                _location = value;
                _UpdateDisplayImage();
            }
        }

        public void ResetScale()
        {
            MarkerScale = 1.0f;
        }

        private void _UpdateDisplayImage()
        {
            if(MarkerScale != 1.0f)
            {
                int scaledWidth = (int)(_referenceImage.Width * MarkerScale) == 0 ? 1 : (int)(_referenceImage.Width * MarkerScale);
                int scaledHeight = (int)(_referenceImage.Height * MarkerScale) == 0 ? 1 : (int)(_referenceImage.Height * MarkerScale);

                using (SKBitmap scaledBitmap = new SKBitmap(scaledWidth, scaledHeight))
                {
                    using (SKCanvas canvas = new SKCanvas(scaledBitmap))
                    {
                        canvas.Clear(SKColors.Transparent);

                        SKRect sourceRect = new SKRect(0, 0, _referenceImage.Width, _referenceImage.Height);
                        SKRect destRect = new SKRect(0, 0, scaledWidth, scaledHeight);

                        canvas.DrawImage(_referenceImage.SKImageInternal, sourceRect, destRect);

                        _displayImage = new Image(scaledBitmap);
                    }
                }
            }
            else
                _displayImage = _referenceImage;
        }

        public void Render(RenderPack rp)
        {
            if (!_isVisible)
                return;

            //TODO fix this assumption
            //Align to center of draw location. Assumes a square image.
            PixelRect drawRect = new PixelRect(Axes.GetPixel(_location), _displayImage.Width / 2.0f); 
            
            using SKPaint paint = new();

            Drawing.DrawImage(rp.Canvas, _displayImage, drawRect, paint);
        }

        //TODO not implemented
        public IEnumerable<LegendItem> LegendItems => LegendItem.Single(new LegendItem());

        public AxisLimits GetAxisLimits() => new AxisLimits(_location);

        private IAxes _axes = new Axes();
        public IAxes Axes { get { return _axes; } set { _axes = value; } }

        private bool _isVisible = true;
        public bool IsVisible { get { return _isVisible; } set { _isVisible = value; } }
    }
}
