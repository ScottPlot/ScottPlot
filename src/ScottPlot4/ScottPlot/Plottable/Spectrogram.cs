using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// signal-Spectrogram
    /// </summary>
    public class Spectrogram : IPlottable, IHasColormap, IDisposable
    {
        /// <summary>
        /// Pre-rendered heatmap image
        /// </summary>
        private Bitmap _bitmap = null;

        private double _offsetX = 0d;
        /// <summary>
        /// Horizontal location of the lower-left cell
        /// </summary>
        public double OffsetX
        {
            get { return _offsetX; }
            set { _offsetX = value; }
        }

        private double _offsetY = 0d;
        /// <summary>
        /// Vertical location of the lower-left cell
        /// </summary>
        public double OffsetY
        {
            get { return _offsetY; }
            set { _offsetY = value; }
        }

        private double _cellWidth = 1d;
        /// <summary>
        /// Width of each cell composing the heatmap
        /// </summary>
        public double CellWidth
        {
            get { return _cellWidth; }
            set { _cellWidth = value; }
        }

        private double _cellHeight = 1d;
        /// <summary>
        /// Width of each cell composing the heatmap
        /// </summary>
        public double CellHeight
        {
            get { return _cellHeight; }
            set { _cellHeight = value; }
        }

        /// <summary>
        /// Position of the left edge of the heatmap
        /// </summary>
        public double XMin
        {
            get { return this._offsetX; }
            set { this._offsetX = value; }
        }

        /// <summary>
        /// Position of the right edge of the heatmap
        /// </summary>
        public double XMax
        {
            get
            {
                return this._offsetX + this._data.DataWidth * this._cellWidth;
            }
            set
            {
                this._cellWidth = (value - this._offsetX) / this._data.DataWidth;
            }
        }

        public double YMin
        {
            get { return this._offsetY; }
            set { this._offsetY = value; }
        }

        public double YMax
        {
            get
            {
                return this._offsetY + this._data.DataHeight * this._cellHeight;
            }
            set
            {
                this._cellHeight = (value - this._offsetY) / this._data.DataHeight;
            }
        }

        /// <summary>
        /// Indicates whether the heatmap's size or location has been modified by the user
        /// </summary>
        public bool IsDefaultSizeAndLocation
        {
            get
            {
                return this._offsetX == 0 &&
                    this._offsetY == 0 &&
                    this._cellHeight == 1 &&
                    this._cellWidth == 1;
            }
        }

        /// <summary>
        /// Text to appear in the legend
        /// </summary>
        public string Label { get; set; }

        private Colormap _colormap = null;
        /// <summary>
        /// Colormap used to translate heatmap values to colors
        /// </summary>
        public Colormap Colormap
        {
            get { return _colormap; }
        }

        private double? _scaleMin = null;
        /// <summary>
        /// If defined, colors will be "clipped" to this value such that lower values (lower colors) will not be shown
        /// </summary>
        public double? ScaleMin
        {
            get { return _scaleMin; }
            set { _scaleMin = value; }
        }

        private double? _scaleMax = null;
        /// <summary>
        /// If defined, colors will be "clipped" to this value such that greater values (higher colors) will not be shown
        /// </summary>
        public double? ScaleMax
        {
            get { return _scaleMax; }
            set { _scaleMax = value; }
        }

        private double? _transparencyThreshold = null;
        /// <summary>
        /// Heatmap values below this number (if defined) will be made transparent
        /// </summary>
        public double? TransparencyThreshold
        {
            get { return _transparencyThreshold; }
            set { _transparencyThreshold = value; }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        private int _XAxisIndex = 0;
        public int XAxisIndex
        {
            get { return _XAxisIndex; }
            set { _XAxisIndex = value; }
        }

        private int _YAxisIndex = 0;
        public int YAxisIndex
        {
            get { return _YAxisIndex; }
            set { _YAxisIndex = value; }
        }

        /// <summary>
        /// Value of the the lower edge of the colormap
        /// </summary>
        public double ColormapMin
        {
            get
            {
                if (!this._scaleMin.HasValue || this._data.MinValue < this._scaleMin.Value)
                {
                    return this._data.MinValue;
                }
                else
                {
                    return this._scaleMin.Value;
                }
            }
        }

        /// <summary>
        /// Value of the the upper edge of the colormap
        /// </summary>
        public double ColormapMax
        {
            get
            {
                if (!this._scaleMax.HasValue || this._data.MaxValue > this._scaleMax.Value)
                {
                    return this._data.MaxValue;
                }
                else
                {
                    return this._scaleMax.Value;
                }
            }
        }

        private bool _colormapMinIsClipped = false;
        /// <summary>
        /// Indicates whether values extend beyond the lower edge of the colormap
        /// </summary>
        public bool ColormapMinIsClipped
        {
            get { return _colormapMinIsClipped; }
        }

        private bool _colormapMaxIsClipped = false;
        /// <summary>
        /// Indicates whether values extend beyond the upper edge of the colormap
        /// </summary>
        public bool ColormapMaxIsClipped
        {
            get { return _colormapMaxIsClipped; }
        }

        /// <summary>
        /// If true, heatmap squares will be smoothed using high quality bicubic interpolation.
        /// If false, heatmap squares will look like sharp rectangles (nearest neighbor interpolation).
        /// </summary>
        public bool Smooth
        {
            get
            {
                return this._interpolation != InterpolationMode.NearestNeighbor;
            }
            set
            {
                this._interpolation = value ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
            }
        }

        private InterpolationMode _interpolation = InterpolationMode.NearestNeighbor;
        /// <summary>
        /// Controls which interpolation mode is used when zooming into the heatmap.
        /// </summary>
        public InterpolationMode Interpolation
        {
            get { return _interpolation; }
            set { _interpolation = value; }
        }

        private bool _flipVertically = false;
        /// <summary>
        /// If true the Heatmap will be drawn from the bottom left corner of the plot. Otherwise it will be drawn from the top left corner. Defaults to false.
        /// </summary>
        public bool FlipVertically
        {
            get { return _flipVertically; }
            set { _flipVertically = value; }
        }

        public LegendItem[] GetLegendItems()
        {
            return Array.Empty<LegendItem>();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Update()
        {
            this._colormapMinIsClipped = this._scaleMin.HasValue && this._scaleMin > this._data.MinValue;
            this._colormapMinIsClipped = this._scaleMax.HasValue && this._scaleMax < this._data.MaxValue;

            double minimumIntensity = this._scaleMin ?? this._data.MinValue;
            double maximumIntensity = this._scaleMax ?? this._data.MaxValue;

            if (this._transparencyThreshold.HasValue)
            {
                this._transparencyThreshold = this.Normalize(new double[] { this._transparencyThreshold.Value }, this._scaleMin.Value, this._data.MaxValue, this._scaleMin, this._scaleMax)[0];
                minimumIntensity = this._transparencyThreshold.Value;
            }

            double[] intensitiesFlattened = this._data.GetIntensitiesFlattened();
            double[] normalizedIntensities = this.Normalize(intensitiesFlattened, minimumIntensity, maximumIntensity, this._scaleMin, this._scaleMax);

            int[] flatARGB;
            if (this._transparencyThreshold.HasValue)
            {
                flatARGB = Colormap.GetRGBAs(normalizedIntensities, this._colormap, minimumIntensity);
            }
            else
            {
                flatARGB = Colormap.GetRGBAs(normalizedIntensities, this._colormap, double.NegativeInfinity);
            }

            if (this._bitmap != null &&
                (this._bitmap.Width != this._data.DataWidth || this._bitmap.Height != this._data.DataHeight))
            {
                this._bitmap.Dispose();
                this._bitmap = null;
            }

            if (this._bitmap == null)
            {
                this._bitmap = new Bitmap(this._data.DataWidth, this._data.DataHeight, PixelFormat.Format32bppArgb);
            }

            Rectangle rect = new Rectangle(0, 0, this._bitmap.Width, this._bitmap.Height);
            BitmapData bmpData = this._bitmap.LockBits(rect, ImageLockMode.ReadWrite, this._bitmap.PixelFormat);
            Marshal.Copy(flatARGB, 0, bmpData.Scan0, flatARGB.Length);
            this._bitmap.UnlockBits(bmpData);
        }

        private double[] Normalize(double[] input, double min, double max, double? scaleMin = null, double? scaleMax = null)
        {
            double NormalizePreserveNull(double i)
            {
                return (i - min) / (max - min);
            }

            min = (scaleMin.HasValue && scaleMin.Value < min) ? scaleMin.Value : min;
            max = (scaleMax.HasValue && scaleMax.Value > max) ? scaleMax.Value : max;

            double[] normalized = input.AsParallel().AsOrdered().Select(i => NormalizePreserveNull(i)).ToArray();

            if (scaleMin.HasValue)
            {
                double threshold = (scaleMin.Value - min) / (max - min);
                normalized = normalized.AsParallel().AsOrdered().Select(i => i < threshold ? threshold : i).ToArray();
            }

            if (scaleMax.HasValue)
            {
                double threshold = (scaleMax.Value - min) / (max - min);
                normalized = normalized.AsParallel().AsOrdered().Select(i => i > threshold ? threshold : i).ToArray();
            }

            return normalized;
        }

        public virtual AxisLimits GetAxisLimits()
        {
            if (this._bitmap is null)
                return AxisLimits.NoLimits;

            return new AxisLimits(
                xMin: this._offsetX,
                xMax: this._offsetX + this._data.DataWidth * this._cellWidth,
                yMin: this._offsetY,
                yMax: this._offsetY + this._data.DataHeight * this._cellHeight);
        }

        /// <summary>
        /// Return the position in the 2D array corresponding to the given coordinate.
        /// Returns null if the coordinate is not over the heatmap.
        /// </summary>
        public (int? xIndex, int? yIndex) GetCellIndexes(double x, double y)
        {
            int? xIndex = (int)((x - this._offsetX) / this._cellWidth);
            int? yIndex = (int)((y - this._offsetY) / this._cellHeight);

            if (xIndex < 0 || xIndex >= this._data.DataWidth)
                xIndex = null;

            if (yIndex < 0 || yIndex >= this._data.DataHeight)
                yIndex = null;

            return (xIndex, yIndex);
        }

        public void ValidateData(bool deepValidation = false)
        {
            if (this._bitmap is null)
                throw new InvalidOperationException("Update() was not called prior to rendering");

            if (deepValidation)
            {
                if (this._data.DataWidth > 1e6 || this._data.DataHeight > 1e6)
                    throw new ArgumentException("Heatmaps may be unreliable for arrays with edges larger than 1 million values");
                if (this._data.DataWidth * this._data.DataHeight > 1e7)
                    throw new ArgumentException("Heatmaps may be unreliable for arrays with more than 10 million values");
            }
        }
        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            {
                gfx.InterpolationMode = this._interpolation;
                gfx.PixelOffsetMode = PixelOffsetMode.Half;

                int fromX = (int)Math.Round(dims.GetPixelX(this._offsetX));
                int fromY = (int)Math.Round(dims.GetPixelY(this._offsetY + this._data.DataHeight * this._cellHeight));
                int width = (int)Math.Round(dims.GetPixelX(this._offsetX + this._data.DataWidth * this._cellWidth) - fromX);
                int height = (int)Math.Round(dims.GetPixelY(this._offsetY) - fromY);


                ImageAttributes attr = new ImageAttributes();
                attr.SetWrapMode(WrapMode.TileFlipXY);

                gfx.TranslateTransform(fromX, fromY);

                if (this._flipVertically)
                {
                    gfx.ScaleTransform(1, -1);
                }

                Rectangle destRect = this._flipVertically ? new Rectangle(0, -height, width, height) : new Rectangle(0, 0, width, height);
                gfx.DrawImage(
                        image: this._bitmap,
                        destRect: destRect,
                        srcX: 0,
                        srcY: 0,
                        this._bitmap.Width,
                        this._bitmap.Height,
                        GraphicsUnit.Pixel,
                        attr);
            }
        }



        private readonly SpectrogramData _data;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        /// <param name="colormap"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Spectrogram(SpectrogramData data, Colormap colormap = null, double? min = null, double? max = null)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "data cannot be null");
            }

            this._data = data;
            if (colormap != null)
            {
                this._colormap = colormap;
            }
            else
            {
                this._colormap = Colormap.Viridis;
            }
            this._scaleMin = min;
            this._scaleMax = max;
        }

        public override string ToString()
        {
            return $"PlottableHeatmap ({this._bitmap.Size})";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            if (this._bitmap != null)
            {
                this._bitmap.Dispose();
                this._bitmap = null;
            }
        }

    }


    /// <summary>
    /// signal-Spectrogram data
    /// </summary>
    public class SpectrogramData
    {
        private const int _DEFAULT_COUNT = -1;
        private readonly double[] _datas;

        private bool _setValueRange = false;
        private bool _appendedData = false;

        /// <summary>
        /// Minimum heatmap value
        /// </summary>
        private double _minValue = double.MaxValue;
        /// <summary>
        /// 值最小值
        /// </summary>
        public double MinValue
        {
            get
            {
                if (this._appendedData)
                {
                    return this._minValue;
                }
                else
                {
                    if (this._setValueRange)
                    {
                        return this._minValue;
                    }
                    else
                    {
                        return short.MinValue;
                    }
                }
            }
        }

        /// <summary>
        /// Maximum heatmap value
        /// </summary>
        private double _maxValue = double.MinValue;
        /// <summary>
        /// 值最大值
        /// </summary>
        public double MaxValue
        {
            get
            {
                if (this._appendedData)
                {
                    return this._maxValue;
                }
                else
                {
                    if (this._setValueRange)
                    {
                        return this._maxValue;
                    }
                    else
                    {
                        return short.MaxValue;
                    }
                }
            }
        }

        /// <summary>
        /// 设置值初始范围
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetValueRange(double minValue, double maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("最小值不能大于最大值");
            }

            this._minValue = minValue;
            this._maxValue = maxValue;
            this._setValueRange = true;
        }


        private readonly int _dataWidth = _DEFAULT_COUNT;
        public int DataWidth
        {
            get { return this._dataWidth; }
        }

        private readonly int _dataHeigth = _DEFAULT_COUNT;
        public int DataHeight
        {
            get { return this._dataHeigth; }
        }

        private readonly SpecPlotMoveOrientation _moveOrientation;
        public SpecPlotMoveOrientation MoveOrientation
        {
            get { return this._moveOrientation; }
        }

        public int Count
        {
            get { return this._count; }
        }

        private readonly SpecPlotMoveMode _moveMode;
        private double _initFillValue;
        private int _index;
        private int _count = 0;
        private readonly int _moveLength;
        private readonly int _valueLength;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataHeigth">行数</param>
        /// <param name="dataWidth">列数</param>
        public SpectrogramData(int dataHeigth, int dataWidth, double initFillValue, SpecPlotMoveOrientation moveOrientation = SpecPlotMoveOrientation.Down, SpecPlotMoveMode moveMode = SpecPlotMoveMode.Always)
        {
            if (dataHeigth < 1 || dataWidth < 1)
            {
                throw new ArgumentOutOfRangeException("行数或列数不能小于1");
            }

            this._dataWidth = dataWidth;
            this._dataHeigth = dataHeigth;
            this._moveOrientation = moveOrientation;
            this._moveMode = moveMode;

            int length = dataHeigth * dataWidth;
            this._datas = new double[length];
            this._initFillValue = initFillValue;
            this.Reset();

            switch (this._moveOrientation)
            {
                case SpecPlotMoveOrientation.Right:
                    this._moveLength = dataWidth - 1;
                    this._valueLength = dataHeigth;
                    break;
                case SpecPlotMoveOrientation.Left:
                    this._moveLength = dataWidth - 1;
                    this._valueLength = dataHeigth;
                    break;
                case SpecPlotMoveOrientation.Down:
                    this._moveLength = length - dataWidth;
                    this._valueLength = dataWidth;
                    break;
                case SpecPlotMoveOrientation.Up:
                    this._moveLength = length - dataWidth;
                    this._valueLength = dataWidth;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void Reset()
        {
            Parallel.For(0, this._dataHeigth, (rowIndex) =>
            {
                int index = rowIndex * this._dataWidth;
                for (int i = 0; i < this._dataWidth; i++)
                {
                    this._datas[index + i] = this._initFillValue;
                }
            });

            //Array.Fill(this._datas, this._initFillValue);

            switch (this._moveMode)
            {
                case SpecPlotMoveMode.Full:
                    switch (this._moveOrientation)
                    {
                        case SpecPlotMoveOrientation.Right:
                            this._index = this._dataWidth - 1;
                            break;
                        case SpecPlotMoveOrientation.Left:
                            this._index = 0;
                            break;
                        case SpecPlotMoveOrientation.Down:
                            this._index = this._datas.Length - this._dataWidth;
                            break;
                        case SpecPlotMoveOrientation.Up:
                            this._index = 0;
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    break;
                case SpecPlotMoveMode.Always:
                    switch (this._moveOrientation)
                    {
                        case SpecPlotMoveOrientation.Right:
                            this._index = 0;
                            break;
                        case SpecPlotMoveOrientation.Left:
                            this._index = this._dataWidth - 1;
                            break;
                        case SpecPlotMoveOrientation.Down:
                            this._index = 0;
                            break;
                        case SpecPlotMoveOrientation.Up:
                            this._index = this._datas.Length - this._dataWidth;
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    break;
                default:
                    throw new NotSupportedException(this._moveMode.ToString());
            }
        }


        public void Clear()
        {
            this.Reset();
            this._count = 0;
        }


        public double[] GetIntensitiesFlattened()
        {
            return this._datas;
        }

        public void Append(double[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Length != this._valueLength)
            {
                throw new ArgumentException($"目标数据长度\"{values.Length}\"与期望数据长度\"{this._valueLength}\"不一致", nameof(values));
            }

            double minValue = double.MaxValue;
            double maxValue = double.MinValue;
            foreach (var value in values)
            {
                if (double.IsNaN(value))
                {
                    throw new InvalidOperationException("SpecPlot do not support intensities of double.NaN");
                }

                if (value < minValue)
                {
                    minValue = value;
                }

                if (value > maxValue)
                {
                    maxValue = value;
                }
            }

            if (this._moveMode == SpecPlotMoveMode.Full)
            {
                switch (this._moveOrientation)
                {
                    case SpecPlotMoveOrientation.Right:
                        if (this._count >= this._dataWidth)
                        {
                            //数据已经填充满
                            Parallel.For(0, this._dataHeigth, (rowIndex) =>
                            {
                                int index = rowIndex * this._dataWidth;
                                Array.Copy(this._datas, index, this._datas, index + 1, this._moveLength);
                                this._datas[index] = values[rowIndex];
                            });
                        }
                        else
                        {
                            //数据未填充满
                            int index;
                            for (int i = 0; i < values.Length; i++)
                            {
                                index = this._index + i * this._dataWidth;
                                this._datas[index] = values[i];
                            }
                            this._index--;
                            this._count++;
                        }
                        break;
                    case SpecPlotMoveOrientation.Left:
                        if (this._count >= this._dataWidth)
                        {
                            //数据已经填充满
                            Parallel.For(0, this._dataHeigth, (rowIndex) =>
                            {
                                int index = rowIndex * this._dataWidth;
                                Array.Copy(this._datas, index + 1, this._datas, index, this._moveLength);
                                this._datas[index + this._dataWidth - 1] = values[rowIndex];
                            });
                        }
                        else
                        {
                            //数据未填充满
                            int index;
                            for (int i = 0; i < values.Length; i++)
                            {
                                index = this._index + i * this._dataWidth;
                                this._datas[index] = values[i];
                            }
                            this._index++;
                            this._count++;
                        }
                        break;
                    case SpecPlotMoveOrientation.Down:
                        if (this._count >= this._dataHeigth)
                        {
                            //数据已经填充满
                            Array.Copy(this._datas, 0, this._datas, this._dataWidth, this._moveLength);
                            Array.Copy(values, 0, this._datas, 0, values.Length);
                        }
                        else
                        {
                            //数据未填充满
                            Array.Copy(values, 0, this._datas, this._index, values.Length);
                            this._index -= this._dataWidth;
                            this._count++;
                        }
                        break;
                    case SpecPlotMoveOrientation.Up:
                        if (this._count >= this._dataHeigth)
                        {
                            //数据已经填充满
                            Array.Copy(this._datas, this._dataWidth, this._datas, 0, this._moveLength);
                            Array.Copy(values, 0, this._datas, this._moveLength, values.Length);
                        }
                        else
                        {
                            //数据未填充满
                            Array.Copy(values, 0, this._datas, this._index, values.Length);
                            this._index += this._dataWidth;
                            this._count++;
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            else if (this._moveMode == SpecPlotMoveMode.Always)
            {
                switch (this._moveOrientation)
                {
                    case SpecPlotMoveOrientation.Right:
                        if (this._count >= this._dataWidth)
                        {
                            Parallel.For(0, this._dataHeigth, (rowIndex) =>
                            {
                                int index = rowIndex * this._dataWidth;
                                Array.Copy(this._datas, index, this._datas, index + 1, this._dataWidth - 1);
                                this._datas[index] = values[rowIndex];
                            });
                        }
                        else
                        {
                            Parallel.For(0, this._dataHeigth, (rowIndex) =>
                            {
                                int index = rowIndex * this._dataWidth;
                                Array.Copy(this._datas, index, this._datas, index + 1, this._count);
                                this._datas[index] = values[rowIndex];
                            });
                            this._count++;
                        }
                        break;
                    case SpecPlotMoveOrientation.Left:
                        if (this._count >= this._dataWidth)
                        {
                            Parallel.For(0, this._dataHeigth, (rowIndex) =>
                            {
                                int index = rowIndex * this._dataWidth;
                                Array.Copy(this._datas, index + 1, this._datas, index, this._dataWidth - 1);
                                this._datas[index + this._dataWidth - 1] = values[rowIndex];
                            });
                        }
                        else
                        {
                            Parallel.For(0, this._dataHeigth, (rowIndex) =>
                            {
                                int rowListItemIndex = rowIndex * this._dataWidth + this._dataWidth;
                                int moveIndex = rowListItemIndex - this._count;
                                Array.Copy(this._datas, moveIndex, this._datas, moveIndex - 1, this._count);
                                this._datas[rowListItemIndex - 1] = values[rowIndex];
                            });
                            this._count++;
                        }
                        break;
                    case SpecPlotMoveOrientation.Down:
                        if (this._count >= this._dataHeigth)
                        {
                            //数据已经填充满
                            Array.Copy(this._datas, 0, this._datas, this._dataWidth, this._moveLength);
                            Array.Copy(values, 0, this._datas, 0, values.Length);
                        }
                        else
                        {
                            //数据未填充满
                            int moveLength = this._count * this._dataWidth;
                            Array.Copy(this._datas, 0, this._datas, this._dataWidth, moveLength);
                            Array.Copy(values, 0, this._datas, 0, values.Length);
                            this._count++;
                        }
                        break;
                    case SpecPlotMoveOrientation.Up:
                        if (this._count >= this._dataHeigth)
                        {
                            //数据已经填充满
                            Array.Copy(this._datas, this._dataWidth, this._datas, 0, this._moveLength);
                            Array.Copy(values, 0, this._datas, this._moveLength, values.Length);
                        }
                        else
                        {
                            //数据未填充满
                            int moveLength = this._count * this._dataWidth;
                            Array.Copy(this._datas, this._datas.Length - moveLength, this._datas, this._datas.Length - moveLength - this._dataWidth, moveLength);
                            Array.Copy(values, 0, this._datas, this._datas.Length - this._dataWidth, values.Length);
                            this._count++;
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            else
            {
                throw new NotSupportedException(this._moveMode.ToString());
            }

            if (minValue < this._minValue)
            {
                this._minValue = Math.Floor(minValue / 10) * 10;
            }

            if (minValue < this._initFillValue)
            {
                this._initFillValue = minValue;
            }

            if (maxValue > this._maxValue)
            {
                this._maxValue = Math.Ceiling(maxValue / 10) * 10;
            }
            this._appendedData = true;
        }
    }

    /// <summary>
    /// 图形移动方向(Move Orientation)
    /// </summary>
    public enum SpecPlotMoveOrientation
    {
        /// <summary>
        /// 从左向向右移动(from left to right)
        /// </summary>
        Right = 0,

        /// <summary>
        /// 从右向左移动(from right to left)
        /// </summary>
        Left,

        /// <summary>
        /// 从上向下移动(from up to down)
        /// </summary>
        Down,

        /// <summary>
        /// 从下向上移动(from down to up)
        /// </summary>
        Up
    }

    /// <summary>
    /// 图形移动模式(Move Mode)
    /// </summary>
    public enum SpecPlotMoveMode
    {
        /// <summary>
        /// 数据填充后才移动(data fill full mope)
        /// </summary>
        Full,

        /// <summary>
        /// 始终移动(always mope)
        /// </summary>
        Always
    }



}
