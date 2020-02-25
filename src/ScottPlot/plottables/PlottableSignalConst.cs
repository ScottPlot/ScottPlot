using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    // Variation of PlottableSignal that uses a segmented tree for faster min/max range queries
    // - frequent min/max lookups are a bottleneck displaying large signals
    // - double[] limited to 60M points (250M in x64 mode) due to memory (tree uses from 2X to 4X memory)
    // - smaller data types have higher points count limits
    // - in x64 mode limit can be up to maximum array size (2G points) with special solution and 64 GB RAM (not tested)
    // - if source array is changed UpdateTrees() must be called
    // - source array can be change by call updateData(), updating by ranges much faster.
    public class PlottableSignalConst<T> : Plottable, IExportable where T : struct, IComparable
    {
        // Any changes must be sync with PlottableSignal
        public T[] ys;
        public double sampleRate;
        public double samplePeriod;
        public float markerSize;
        public double xOffset;
        public double yOffset;
        public Pen pen;
        public Brush brush;

        // using 2 x signal memory in best case: ys.Length is Pow2 
        // using 4 x signal memory in worst case: ys.Length is (Pow2 +1);
        T[] TreeMin;
        T[] TreeMax;
        private int n = 0; // size of each Tree
        public bool TreesReady = false;
        // precompiled lambda expressions for fast math on generic
        private static Func<T, T, T> MinExp;
        private static Func<T, T, T> MaxExp;
        private static Func<T, T, bool> EqualExp;
        private static Func<T> MaxValue;
        private static Func<T> MinValue;
        private static Func<T, T, bool> LessThanExp;
        private static Func<T, T, bool> GreaterThanExp;

        private void InitExp()
        {
            ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(T), "b");
            // add the parameters together
            ConditionalExpression bodyMin = Expression.Condition(Expression.LessThanOrEqual(paramA, paramB), paramA, paramB);
            ConditionalExpression bodyMax = Expression.Condition(Expression.GreaterThanOrEqual(paramA, paramB), paramA, paramB);
            BinaryExpression bodyEqual = Expression.Equal(paramA, paramB);
            MemberExpression bodyMaxValue = Expression.MakeMemberAccess(null, typeof(T).GetField("MaxValue"));
            MemberExpression bodyMinValue = Expression.MakeMemberAccess(null, typeof(T).GetField("MinValue"));
            BinaryExpression bodyLessThan = Expression.LessThan(paramA, paramB);
            BinaryExpression bodyGreaterThan = Expression.GreaterThan(paramA, paramB);
            // compile it
            MinExp = Expression.Lambda<Func<T, T, T>>(bodyMin, paramA, paramB).Compile();
            MaxExp = Expression.Lambda<Func<T, T, T>>(bodyMax, paramA, paramB).Compile();
            EqualExp = Expression.Lambda<Func<T, T, bool>>(bodyEqual, paramA, paramB).Compile();
            MaxValue = Expression.Lambda<Func<T>>(bodyMaxValue).Compile();
            MinValue = Expression.Lambda<Func<T>>(bodyMinValue).Compile();
            LessThanExp = Expression.Lambda<Func<T, T, bool>>(bodyLessThan, paramA, paramB).Compile();
            GreaterThanExp = Expression.Lambda<Func<T, T, bool>>(bodyGreaterThan, paramA, paramB).Compile();
        }
        public PlottableSignalConst(T[] ys, double sampleRate, double xOffset, double yOffset, Color color, double lineWidth, double markerSize, string label, bool useParallel)
        {
            if (ys == null)
                throw new Exception("Y data cannot be null");
            this.ys = ys;
            this.sampleRate = sampleRate;
            this.samplePeriod = 1.0 / sampleRate;
            this.markerSize = (float)markerSize;
            this.xOffset = xOffset;
            this.label = label;
            this.color = color;
            this.yOffset = yOffset;
            this.useParallel = useParallel;
            pointCount = ys.Length;
            brush = new SolidBrush(color);
            pen = new Pen(color, (float)lineWidth)
            {
                // this prevents sharp corners
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };
            try // runtime check
            {
                Convert.ToDouble(new T());
            }
            catch
            {
                throw new ArgumentOutOfRangeException("Unsupported data type, provide convertable to double data types");
            }
            InitExp();
            if (useParallel)
                UpdateTreesInBackground();
            else
                UpdateTrees();
        }

        public void updateData(int index, T newValue)
        {
            ys[index] = newValue;
            // Update Tree, can be optimized            
            if (index == ys.Length - 1) // last elem haven't pair
            {
                TreeMin[n / 2 + index / 2] = ys[index];
                TreeMax[n / 2 + index / 2] = ys[index];
            }
            else if (index % 2 == 0) // even elem have right pair
            {
                TreeMin[n / 2 + index / 2] = MinExp(ys[index], ys[index + 1]);
                TreeMax[n / 2 + index / 2] = MaxExp(ys[index], ys[index + 1]);
            }
            else // odd elem have left pair
            {
                TreeMin[n / 2 + index / 2] = MinExp(ys[index], ys[index - 1]);
                TreeMax[n / 2 + index / 2] = MaxExp(ys[index], ys[index - 1]);
            }

            T candidate;
            for (int i = (n / 2 + index / 2) / 2; i > 0; i /= 2)
            {
                candidate = MinExp(TreeMin[i * 2], TreeMin[i * 2 + 1]);
                if (EqualExp(TreeMin[i], candidate)) // if node same then new value don't need to recalc all upper
                    break;
                TreeMin[i] = candidate;
            }
            for (int i = (n / 2 + index / 2) / 2; i > 0; i /= 2)
            {
                candidate = MaxExp(TreeMax[i * 2], TreeMax[i * 2 + 1]);
                if (EqualExp(TreeMax[i], candidate)) // if node same then new value don't need to recalc all upper
                    break;
                TreeMax[i] = candidate;
            }
        }

        public void updateData(int from, int to, T[] newData, int fromData = 0) // RangeUpdate
        {
            //update source signal
            for (int i = from; i < to; i++)
            {
                ys[i] = newData[i - from + fromData];
            }

            for (int i = n / 2 + from / 2; i < n / 2 + to / 2; i++)
            {
                TreeMin[i] = MinExp(ys[i * 2 - n], ys[i * 2 + 1 - n]);
                TreeMax[i] = MaxExp(ys[i * 2 - n], ys[i * 2 + 1 - n]);
            }
            if (to == ys.Length) // last elem haven't pair
            {
                TreeMin[n / 2 + to / 2] = ys[to - 1];
                TreeMax[n / 2 + to / 2] = ys[to - 1];
            }
            else if (to % 2 == 1) //last elem even(to-1) and not last
            {
                TreeMin[n / 2 + to / 2] = MinExp(ys[to - 1], ys[to]);
                TreeMax[n / 2 + to / 2] = MaxExp(ys[to - 1], ys[to]);
            }

            from = (n / 2 + from / 2) / 2;
            to = (n / 2 + to / 2) / 2;

            T candidate;
            while (from != 0) // up to root elem, that is [1], [0] - is free elem
            {
                if (from != to)
                {
                    for (int i = from; i <= to; i++) // Recalc all level nodes in range 
                    {
                        TreeMin[i] = MinExp(TreeMin[i * 2], TreeMin[i * 2 + 1]);
                        TreeMax[i] = MaxExp(TreeMax[i * 2], TreeMax[i * 2 + 1]);
                    }
                }
                else
                {
                    // left == rigth, so no need more from to loop
                    for (int i = from; i > 0; i /= 2) // up to root node
                    {
                        candidate = MinExp(TreeMin[i * 2], TreeMin[i * 2 + 1]);
                        if (EqualExp(TreeMin[i], candidate)) // if node same then new value don't need to recalc all upper
                            break;
                        TreeMin[i] = candidate;
                    }

                    for (int i = from; i > 0; i /= 2) // up to root node
                    {
                        candidate = MaxExp(TreeMax[i * 2], TreeMax[i * 2 + 1]);
                        if (EqualExp(TreeMax[i], candidate)) // if node same then new value don't need to recalc all upper
                            break;
                        TreeMax[i] = candidate;
                    }
                    // all work done exit while loop
                    break;
                }
                // level up
                from = from / 2;
                to = to / 2;
            }
        }

        public void updateData(int from, T[] newData)
        {
            updateData(from, newData.Length, newData);
        }

        public void updateData(T[] newData)
        {
            updateData(0, newData.Length, newData);
        }

        public void UpdateTreesInBackground()
        {
            Task.Run(() => { UpdateTrees(); });
        }

        public void UpdateTrees()
        {
            // O(n) to build trees
            TreesReady = false;
            try
            {
                if (ys.Length == 0)
                    throw new ArgumentOutOfRangeException($"Array cant't be empty");
                // Size up to pow2
                if (ys.Length > 0x40_00_00_00) // pow 2 must be more then int.MaxValue
                    throw new ArgumentOutOfRangeException($"Array higher than {0x40_00_00_00} not supported by SignalConst");
                int pow2 = 1;
                while (pow2 < 0x40_00_00_00 && pow2 < ys.Length)
                    pow2 <<= 1;
                n = pow2;
                TreeMin = new T[n];
                TreeMax = new T[n];
                T maxValue = MaxValue();
                T minValue = MinValue();

                // fill bottom layer of tree
                for (int i = 0; i < ys.Length / 2; i++) // with source array pairs min/max
                {
                    TreeMin[n / 2 + i] = MinExp(ys[i * 2], ys[i * 2 + 1]);
                    TreeMax[n / 2 + i] = MaxExp(ys[i * 2], ys[i * 2 + 1]);
                }
                if (ys.Length % 2 == 1) // if array size odd, last element haven't pair to compare
                {
                    TreeMin[n / 2 + ys.Length / 2] = ys[ys.Length - 1];
                    TreeMax[n / 2 + ys.Length / 2] = ys[ys.Length - 1];
                }
                for (int i = n / 2 + (ys.Length + 1) / 2; i < n; i++) // min/max for pairs of nonexistent elements
                {
                    TreeMin[i] = minValue;
                    TreeMax[i] = maxValue;
                }
                // fill other layers
                for (int i = n / 2 - 1; i > 0; i--)
                {
                    TreeMin[i] = MinExp(TreeMin[2 * i], TreeMin[2 * i + 1]);
                    TreeMax[i] = MaxExp(TreeMax[2 * i], TreeMax[2 * i + 1]);
                }
                TreesReady = true;
            }
            catch (OutOfMemoryException)
            {
                TreeMin = null;
                TreeMax = null;
                TreesReady = false;
                return;
            }
        }

        //  O(log(n)) for each range min/max query
        protected void MinMaxRangeQuery(int l, int r, out double lowestValue, out double highestValue)
        {
            T lowestValueT;
            T highestValueT;
            // if the tree calculation isn't finished or if it crashed
            if (!TreesReady)
            {
                // use the original (slower) min/max calculated method
                lowestValueT = ys[l];
                highestValueT = ys[l];
                for (int i = l; i < r; i++)
                {
                    if (LessThanExp(ys[i], lowestValueT))
                        lowestValueT = ys[i];
                    if (GreaterThanExp(ys[i], highestValueT))
                        highestValueT = ys[i];
                }
                lowestValue = Convert.ToDouble(lowestValueT);
                highestValue = Convert.ToDouble(highestValueT);
                return;
            }

            lowestValueT = MaxValue();
            highestValueT = MinValue();
            if (l == r)
            {
                lowestValue = highestValue = Convert.ToDouble(ys[l]);
                return;
            }
            // first iteration on source array that virtualy bottom of tree
            if ((l & 1) != 1) // l is left child
            {
                lowestValueT = MinExp(lowestValueT, ys[l]);
                highestValueT = MaxExp(highestValueT, ys[l]);
            }
            if ((r & 1) == 1) // r is right child
            {
                lowestValueT = MinExp(lowestValueT, ys[r]);
                highestValueT = MaxExp(highestValueT, ys[r]);
            }
            // go up from array to bottom of Tree
            l = (l + n) / 2;
            r = (r + n) / 2;
            // next iterations on tree
            while (l <= r)
            {
                if ((l & 1) == 1) // l is right child
                {
                    lowestValueT = MinExp(lowestValueT, TreeMin[l]);
                    highestValueT = MaxExp(highestValueT, TreeMax[l]);
                }
                if ((r & 1) != 1) // r is left child
                {
                    lowestValueT = MinExp(lowestValueT, TreeMin[r]);
                    highestValueT = MaxExp(highestValueT, TreeMax[r]);
                }
                // go up one level
                l = (l + 1) / 2;
                r = (r - 1) / 2;
            }
            lowestValue = Convert.ToDouble(lowestValueT);
            highestValue = Convert.ToDouble(highestValueT);
        }

        public override Config.AxisLimits2D GetLimits()
        {
            double[] limits = new double[4];
            limits[0] = 0 + xOffset;
            limits[1] = samplePeriod * ys.Length + xOffset;
            MinMaxRangeQuery(0, ys.Length - 1, out limits[2], out limits[3]);
            limits[2] += yOffset;
            limits[3] += yOffset;

            // TODO: use features of 2d axis
            return new Config.AxisLimits2D(limits);
        }

        private void RenderSingleLine(Settings settings)
        {
            // this function is for when the graph is zoomed so far out its entire display is a single vertical pixel column
            double yMin, yMax;
            MinMaxRangeQuery(0, ys.Length - 1, out yMin, out yMax);
            PointF point1 = settings.GetPixel(xOffset, yMin + yOffset);
            PointF point2 = settings.GetPixel(xOffset, yMax + yOffset);
            settings.gfxData.DrawLine(pen, point1, point2);
        }

        private void RenderLowDensity(Settings settings, int visibleIndex1, int visibleIndex2)
        {
            // this function is for when the graph is zoomed in so individual data points can be seen

            List<PointF> linePoints = new List<PointF>(visibleIndex2 - visibleIndex1 + 2);
            if (visibleIndex2 > ys.Length - 2)
                visibleIndex2 = ys.Length - 2;
            if (visibleIndex1 < 0)
                visibleIndex1 = 0;
            for (int i = visibleIndex1; i <= visibleIndex2 + 1; i++)
                linePoints.Add(settings.GetPixel(samplePeriod * i + xOffset, Convert.ToDouble(ys[i]) + yOffset));

            if (linePoints.Count > 1)
            {
                if (pen.Width > 0)
                    settings.gfxData.DrawLines(pen, linePoints.ToArray());

                if (markerSize > 0)
                {
                    // make markers transition away smoothly by making them smaller as the user zooms out
                    float pixelsBetweenPoints = (float)(samplePeriod * settings.xAxisScale);
                    float zoomTransitionScale = Math.Min(1, pixelsBetweenPoints / 10);
                    float markerPxDiameter = markerSize * zoomTransitionScale;
                    float markerPxRadius = markerPxDiameter / 2;
                    foreach (PointF point in linePoints)
                        settings.gfxData.FillEllipse(brush: brush,
                            x: point.X - markerPxRadius, y: point.Y - markerPxRadius,
                            width: markerPxDiameter, height: markerPxDiameter);
                }
            }
        }

        private void RenderHighDensityParallel(Settings settings, double offsetPoints, double columnPointCount)
        {
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((ys.Length - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = Math.Min(settings.dataSize.Width, xPxEnd);
            if (xPxStart >= xPxEnd)
                return;
            PointF[] linePoints = new PointF[(xPxEnd - xPxStart) * 2];
            Parallel.For(xPxStart, xPxEnd, xPx =>
            {
                // determine data indexes for this pixel column
                int index1 = (int)(offsetPoints + columnPointCount * xPx);
                int index2 = (int)(offsetPoints + columnPointCount * (xPx + 1));

                if (index1 < 0)
                    index1 = 0;
                if (index2 > ys.Length - 1)
                    index2 = ys.Length - 1;

                // get the min and max value for this column                
                double lowestValue, highestValue;
                MinMaxRangeQuery(index1, index2, out lowestValue, out highestValue);
                float yPxHigh = (float)settings.GetPixelY(lowestValue + yOffset);
                float yPxLow = (float)settings.GetPixelY(highestValue + yOffset);


                linePoints[(xPx - xPxStart) * 2] = new PointF(xPx, yPxLow);
                linePoints[(xPx - xPxStart) * 2 + 1] = new PointF(xPx, yPxHigh);
            });

            // adjust order of points to enhance anti-aliasing
            PointF buf;
            for (int i = 1; i < linePoints.Length / 2; i++)
            {
                if (linePoints[i * 2].Y >= linePoints[i * 2 - 1].Y)
                {
                    buf = linePoints[i * 2];
                    linePoints[i * 2] = linePoints[i * 2 + 1];
                    linePoints[i * 2 + 1] = buf;
                }
            }

            settings.gfxData.DrawLines(pen, linePoints);
        }

        private void RenderHighDensity(Settings settings, double offsetPoints, double columnPointCount)
        {
            // this function is for when the graph is zoomed out so each pixel column represents the vertical span of multiple data points
            int xPxStart = (int)Math.Ceiling((-1 - offsetPoints) / columnPointCount - 1);
            int xPxEnd = (int)Math.Ceiling((ys.Length - offsetPoints) / columnPointCount);
            xPxStart = Math.Max(0, xPxStart);
            xPxEnd = Math.Min(settings.dataSize.Width, xPxEnd);
            if (xPxStart >= xPxEnd)
                return;
            List<PointF> linePoints = new List<PointF>((xPxEnd - xPxStart) * 2 + 1);
            for (int xPx = xPxStart; xPx < xPxEnd; xPx++)
            {
                // determine data indexes for this pixel column
                int index1 = (int)(offsetPoints + columnPointCount * xPx);
                int index2 = (int)(offsetPoints + columnPointCount * (xPx + 1));

                if (index1 < 0)
                    index1 = 0;
                if (index2 > ys.Length - 1)
                    index2 = ys.Length - 1;

                // get the min and max value for this column                
                double lowestValue, highestValue;
                MinMaxRangeQuery(index1, index2, out lowestValue, out highestValue);
                float yPxHigh = (float)settings.GetPixelY(lowestValue + yOffset);
                float yPxLow = (float)settings.GetPixelY(highestValue + yOffset);

                // adjust order of points to enhance anti-aliasing
                if ((linePoints.Count < 2) || (yPxLow < linePoints[linePoints.Count - 1].Y))
                {
                    linePoints.Add(new PointF(xPx, yPxLow));
                    linePoints.Add(new PointF(xPx, yPxHigh));
                }
                else
                {
                    linePoints.Add(new PointF(xPx, yPxHigh));
                    linePoints.Add(new PointF(xPx, yPxLow));
                }
            }

            if (linePoints.Count > 0)
                settings.gfxData.DrawLines(pen, linePoints.ToArray());
        }

        public override void Render(Settings settings)
        {
            double dataSpanUnits = ys.Length * samplePeriod;
            double columnSpanUnits = settings.axes.x.span / settings.dataSize.Width;
            double columnPointCount = (columnSpanUnits / dataSpanUnits) * ys.Length;
            double offsetUnits = settings.axes.x.min - xOffset;
            double offsetPoints = offsetUnits / samplePeriod;
            int visibleIndex1 = (int)(offsetPoints);
            int visibleIndex2 = (int)(offsetPoints + columnPointCount * (settings.dataSize.Width + 1));
            int visiblePointCount = visibleIndex2 - visibleIndex1;
            double pointsPerPixelColumn = visiblePointCount / settings.dataSize.Width;
            double dataWidthPx2 = visibleIndex2 - visibleIndex1 + 2;

            PointF firstPoint = settings.GetPixel(xOffset, Convert.ToDouble(ys[0]) + yOffset);
            PointF lastPoint = settings.GetPixel(samplePeriod * (ys.Length - 1) + xOffset, Convert.ToDouble(ys[ys.Length - 1]) + yOffset);
            double dataWidthPx = lastPoint.X - firstPoint.X;

            // use different rendering methods based on how dense the data is on screen
            if ((dataWidthPx <= 1) || (dataWidthPx2 <= 1))
            {
                RenderSingleLine(settings);
            }
            else if (pointsPerPixelColumn > 1)
            {
                if (useParallel)
                    RenderHighDensityParallel(settings, offsetPoints, columnPointCount);
                else
                    RenderHighDensity(settings, offsetPoints, columnPointCount);
            }
            else
            {
                RenderLowDensity(settings, visibleIndex1, visibleIndex2);
            }
        }

        public override string ToString()
        {
            return $"PlottableSignalConst with {pointCount} points ({typeof(T).Name}), trees {(TreesReady ? "" : "not")} calculated";
        }

        public void SaveCSV(string filePath, string delimiter = ", ", string separator = "\n")
        {
            System.IO.File.WriteAllText(filePath, GetCSV(delimiter, separator));
        }

        public string GetCSV(string delimiter = ", ", string separator = "\n")
        {
            StringBuilder csv = new StringBuilder();
            for (int i = 0; i < ys.Length; i++)
                csv.AppendFormat("{0}{1}{2}{3}", xOffset + i * samplePeriod, delimiter, Convert.ToDouble(ys[i]) + yOffset, separator);
            return csv.ToString();
        }
    }
}
