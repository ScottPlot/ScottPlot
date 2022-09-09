using ScottPlot.Plottable;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime;

namespace ScottPlot
{
    public partial class Plot
    {
        /// <summary>
        /// The settings object stores all state (configuration and data) for a plot
        /// </summary>
        private readonly Settings settings = new Settings();

        /// <summary>
        /// Plot image width (pixels)
        /// </summary>
        public float Width { get => settings.Width; set => Resize(value, settings.Height); }

        /// <summary>
        /// Plot image height (pixels)
        /// </summary>
        public float Height { get => settings.Height; set => Resize(settings.Width, value); }

        /// <summary>
        /// A ScottPlot stores data in plottable objects and draws it on a bitmap when Render() is called
        /// </summary>
        /// <param name="width">default width (pixels) to use when rendering</param>
        /// <param name="height">default height (pixels) to use when rendering</param>
        public Plot(int width = 800, int height = 600)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("width and height must each be greater than 0");

            Style(ScottPlot.Style.Default);
            Resize(width, height);
        }

        /// <summary>
        /// Brief description of this plot
        /// </summary>
        /// <returns>plot description</returns>
        public override string ToString() =>
            $"ScottPlot ({settings.Width}x{settings.Height}) " +
            $"with {settings.Plottables.Count:n0} plottables";

        /// <summary>
        /// ScottPlot version in the format "1.2.3" (or "1.2.3-beta" for pre-releases)
        /// </summary>
        public static string Version
        {
            get
            {
                Version v = typeof(Plot).Assembly.GetName().Version;
                string versionString = $"{v.Major}.{v.Minor}.{v.Build}";
                return versionString;
            }
        }

        #region add, clear, and remove plottables

        /// <summary>
        /// Add a plottable to the plot
        /// </summary>
        /// <param name="plottable">a plottable the user created</param>
        public void Add(IPlottable plottable)
        {
            settings.Plottables.Add(plottable);
        }

        /// <summary>
        /// Clear all plottables
        /// </summary>
        public void Clear()
        {
            settings.Plottables.Clear();
            settings.ResetAxisLimits();
        }

        /// <summary>
        /// Remove all plottables of the given type
        /// </summary>
        /// <param name="plottableType">all plottables of this type will be removed</param>
        public void Clear(Type plottableType)
        {
            var plottablesWithSameType = settings.Plottables.Where(x => x.GetType() == plottableType).ToList();

            while (plottablesWithSameType.Count > 0)
            {
                settings.Plottables.Remove(plottablesWithSameType[0]);
                plottablesWithSameType.RemoveAt(0);
            }

            if (settings.Plottables.Count == 0)
                settings.ResetAxisLimits();
        }

        /// <summary>
        /// Remove a specific plottable
        /// </summary>
        /// <param name="plottable">The plottable to remove</param>
        public void Remove(IPlottable plottable)
        {
            settings.Plottables.Remove(plottable);

            if (settings.Plottables.Count == 0)
                settings.ResetAxisLimits();
        }

        /// <summary>
        /// Remove the plottable at the specified index of the list
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove</param>
        public void RemoveAt(int index)
        {
            settings.Plottables.RemoveAt(index);

            if (settings.Plottables.Count == 0)
                settings.ResetAxisLimits();
        }

        /// <summary>
        /// Move a plottable in the list. Plottables near the end are rendered last (on top).
        /// </summary>
        public void Move(int oldIndex, int newIndex) => settings.Plottables.Move(oldIndex, newIndex);

        /// <summary>
        /// Move a plottable to the front so it is rendered first and appears beneath all others.
        /// </summary>
        public void MoveFirst(IPlottable plottable)
        {
            settings.Plottables.Remove(plottable);
            settings.Plottables.Insert(0, plottable);
        }

        /// <summary>
        /// Move a plottable to the end so it is rendered last and appears above all others.
        /// </summary>
        public void MoveLast(IPlottable plottable)
        {
            settings.Plottables.Remove(plottable);
            settings.Plottables.Add(plottable);
        }

        /// <summary>
        /// Return a copy of the list of plottables
        /// </summary>
        /// <returns>list of plottables</returns>
        public IPlottable[] GetPlottables() => settings.Plottables.ToArray();

        /// <summary>
        /// Return the draggable plottable under the mouse cursor (or null if there isn't one)
        /// </summary>
        public IDraggable GetDraggable(double xPixel, double yPixel, int snapDistancePixels = 5)
        {
            IDraggable[] enabledDraggables = settings.Plottables
                           .Where(x => x is IDraggable)
                           .Select(x => (IDraggable)x)
                           .Where(x => x.DragEnabled)
                           .Where(x => x is IPlottable p && p.IsVisible)
                           .ToArray();

            foreach (IDraggable draggable in enabledDraggables)
            {
                int xAxisIndex = ((IPlottable)draggable).XAxisIndex;
                int yAxisIndex = ((IPlottable)draggable).YAxisIndex;
                double xUnitsPerPx = settings.GetXAxis(xAxisIndex).Dims.UnitsPerPx;
                double yUnitsPerPx = settings.GetYAxis(yAxisIndex).Dims.UnitsPerPx;

                double snapWidth = xUnitsPerPx * snapDistancePixels;
                double snapHeight = yUnitsPerPx * snapDistancePixels;
                double xCoords = GetCoordinateX((float)xPixel, xAxisIndex);
                double yCoords = GetCoordinateY((float)yPixel, yAxisIndex);
                if (draggable.IsUnderMouse(xCoords, yCoords, snapWidth, snapHeight))
                    return draggable;
            }

            return null;
        }

        /// <summary>
        /// Return the highest hittable plottable at the given point (or null if no hit)
        /// </summary>
        public IPlottable GetHittable(double xPixel, double yPixel)
        {
            foreach (var plottable in GetPlottables().Where(x => x is IHittable).Reverse())
            {
                int xAxisIndex = plottable.XAxisIndex;
                int yAxisIndex = plottable.YAxisIndex;

                double xCoords = GetCoordinateX((float)xPixel, xAxisIndex);
                double yCoords = GetCoordinateY((float)yPixel, yAxisIndex);
                Coordinate c = new(xCoords, yCoords);

                IHittable hittable = (IHittable)plottable;
                if (hittable.HitTest(c))
                    return plottable;
            }

            return null;
        }

        #endregion

        #region plottable validation

        /// <summary>
        /// Throw an exception if any plottable contains an invalid state.
        /// </summary>
        /// <param name="deep">Check every individual value for validity. This is more thorough, but slower.</param>
        public void Validate(bool deep = true)
        {
            foreach (var plottable in settings.Plottables)
                plottable.ValidateData(deep);
        }

        #endregion

        #region plot settings and styling

        /// <summary>
        /// The Settings module stores manages plot state and advanced configuration.
        /// Its class structure changes frequently, and users are highly advised not to interact with it directly.
        /// This method returns the settings module for advanced users and developers to interact with.
        /// </summary>
        /// <param name="showWarning">Show a warning message indicating this method is only intended for developers</param>
        /// <returns>Settings used by the plot</returns>
        public Settings GetSettings(bool showWarning = true)
        {
            if (showWarning)
                Debug.WriteLine("WARNING: GetSettings() is only for development and testing. " +
                                "Be aware its class structure changes frequently!");
            return settings;
        }

        /// <summary>
        /// Update the default size for new renders
        /// </summary>
        /// <param name="width">width (pixels) for future renders</param>
        /// <param name="height">height (pixels) for future renders</param>
        public void Resize(float width, float height) => settings.Resize(width, height);

        /// <summary>
        /// Return a new color from the Pallette based on the number of plottables already in the plot.
        /// Use this to ensure every plottable gets a unique color.
        /// </summary>
        /// <param name="alpha">value from 0 (transparent) to 1 (opaque)</param>
        /// <returns>new color</returns>
        public Color GetNextColor(double alpha = 1) => Color.FromArgb((byte)(alpha * 255), settings.GetNextColor());

        /// <summary>
        /// The palette defines default colors to use for new plottables.
        /// </summary>
        public IPalette Palette
        {
            get => settings.PlottablePalette;
            set => settings.PlottablePalette = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Set the colors and fonts of many plot components at once using a predefined theme
        /// </summary>
        public void Style(Styles.IStyle style)
        {
            if (style is null)
                throw new ArgumentNullException(nameof(style));

            settings.FigureBackground.Color = style.FigureBackgroundColor;
            settings.DataBackground.Color = style.DataBackgroundColor;

            foreach (var axis in settings.Axes)
            {
                axis.LabelStyle(color: style.AxisLabelColor, fontName: style.AxisLabelFontName);
                axis.TickLabelStyle(color: style.TickLabelColor, fontName: style.TickLabelFontName);
                axis.MajorGrid(color: style.GridLineColor);
                axis.MinorGrid(color: style.GridLineColor);
                axis.TickMarkColor(majorColor: style.TickMajorColor, minorColor: style.TickMinorColor);
                axis.Line(color: style.FrameColor);
            }

            XAxis2.LabelStyle(color: style.TitleFontColor, fontName: style.TitleFontName);

            foreach (IStylable plottable in settings.Plottables.Where(x => x is IStylable))
                plottable.SetStyle(style.TickMajorColor, style.TickLabelColor);
        }

        /// <summary>
        /// Set the color of specific plot components
        /// </summary>
        /// <param name="figureBackground">Color for area beneath the axis ticks and labels and around the data area</param>
        /// <param name="dataBackground">Color for area inside the data frame but beneath the grid and plottables</param>
        /// <param name="grid">Color for grid lines</param>
        /// <param name="tick">Color for axis tick marks and frame lines</param>
        /// <param name="axisLabel">Color for axis labels and tick labels</param>
        /// <param name="titleLabel">Color for the top axis label (XAxis2's title)</param>
        /// <param name="dataBackgroundImage">Bitmap to display behind the data area</param>
        /// <param name="figureBackgroundImage">Bitmap to display behind the entire figure</param>
        public void Style(
            Color? figureBackground = null,
            Color? dataBackground = null,
            Color? grid = null,
            Color? tick = null,
            Color? axisLabel = null,
            Color? titleLabel = null,
            Bitmap dataBackgroundImage = null,
            Bitmap figureBackgroundImage = null)
        {
            settings.FigureBackground.Color = figureBackground ?? settings.FigureBackground.Color;
            settings.DataBackground.Color = dataBackground ?? settings.DataBackground.Color;

            settings.FigureBackground.Bitmap ??= figureBackgroundImage;
            settings.DataBackground.Bitmap ??= dataBackgroundImage;

            foreach (var axis in settings.Axes)
            {
                axis.Label(color: axisLabel);
                axis.TickLabelStyle(color: tick);
                axis.MajorGrid(color: grid);
                axis.MinorGrid(color: grid);
                if (tick.HasValue)
                    axis.TickMarkColor(color: tick.Value);
                axis.Line(color: tick);
            }

            XAxis2.Label(color: titleLabel);

            foreach (IStylable plottable in settings.Plottables.Where(x => x is IStylable))
                plottable.SetStyle(tick, axisLabel);
        }

        /// <summary>
        /// Reset axis padding to the default values for all axes
        /// </summary>
        public void ResetLayout()
        {
            foreach (var axis in settings.Axes)
                axis.ResetLayout();
        }

        #endregion

        #region renderable customization
        /// <summary>
        /// If enabled, the benchmark displays render information in the corner of the plot.
        /// </summary>
        /// <param name="enable">True/false defines whether benchmark is enabled. Null will not change the benchmark.</param>
        /// <returns>true if the benchmark is enabled</returns>
        public bool Benchmark(bool? enable = true)
        {
            if (enable.HasValue)
                settings.BenchmarkMessage.IsVisible = enable.Value;

            return settings.BenchmarkMessage.IsVisible;
        }

        /// <summary>
        /// Return an array of times for the last several renders.
        /// The last element of the array is the most recently rendered frame time.
        /// </summary>
        public double[] BenchmarkTimes() => settings.BenchmarkMessage.GetRenderTimes();

        /// <summary>
        /// Configure legend visibility and location. 
        /// Optionally you can further customize the legend by interacting with the object it returns.
        /// </summary>
        /// <param name="enable">whether or not the legend is visible (or null for no change)</param>
        /// <param name="location">position of the legend relative to the data area</param>
        /// <returns>The legend itself. Use public fields to further customize its appearance and behavior.</returns>
        public Renderable.Legend Legend(bool? enable = true, Alignment? location = Alignment.LowerRight)
        {
            if (enable.HasValue)
            {
                settings.CornerLegend.IsVisible = enable.Value;
                settings.CornerLegend.Location = location ?? settings.CornerLegend.Location;
            }

            return settings.CornerLegend;
        }

        #endregion

        #region copy and equals

        /// <summary>
        /// Return a new Plot with all the same Plottables (and some of the styles) of this one.
        /// </summary>
        /// <returns>A new plot similar to this one.</returns>
        public Plot Copy()
        {
            Settings oldSettings = settings;
            Plot oldPlot = this;

            Plot newPlot = new Plot(oldSettings.Width, oldSettings.Height);

            foreach (IPlottable oldPlottable in oldPlot.GetPlottables())
                newPlot.Add(oldPlottable);
            newPlot.AxisAuto();

            newPlot.XLabel(oldSettings.XAxis.Label());
            newPlot.YLabel(oldSettings.YAxis.Label());
            newPlot.Title(oldSettings.XAxis2.Label());

            return newPlot;
        }

        /// <summary>
        /// The GUID helps identify individual plots
        /// </summary>
        private readonly Guid Guid = Guid.NewGuid();

        /// <summary>
        /// Every plot has a globally unique ID (GUID) that can help differentiate it from other plots
        /// </summary>
        /// <returns>A string representing the GUID</returns>
        public string GetGuid() => Guid.ToString();

        /// <summary>
        /// Returns true if the given plot is the exact same plot as this one
        /// </summary>
        /// <param name="obj">the plot to compare this one to</param>
        /// <returns>true if the two plots have the same GUID</returns>
        public override bool Equals(object obj) => obj.GetHashCode() == GetHashCode();

        /// <summary>
        /// Returns an integer unique to this instance (based on the GUID)
        /// </summary>
        /// <returns>An integer representing the GUID</returns>
        public override int GetHashCode() => Guid.GetHashCode();

        #endregion
    }
}
