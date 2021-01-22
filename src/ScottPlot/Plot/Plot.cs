using ScottPlot.Plottable;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace ScottPlot
{
    public partial class Plot
    {
        /// <summary>
        /// The settings object stores all state (configuration and data) for a plot
        /// </summary>
        private readonly Settings settings = new Settings();

        /// <summary>
        /// A ScottPlot stores data in plottable objects and draws it on a bitmap when Render() is called
        /// </summary>
        public Plot(int width = 800, int height = 600)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("width and height must each be greater than 0");

            StyleTools.SetStyle(this, ScottPlot.Style.Default);
            SetSize(width, height);
        }

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
                string versionString = $"{v.Major}.{v.Minor}.{v.Build}-beta";
                return versionString;
            }
        }

        #region add, clear, and remove plottables

        /// <summary>
        /// Add a plottable to the plot
        /// </summary>
        public void Add(IPlottable plottable) => settings.Plottables.Add(plottable);

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
        public void Clear(Type plottableType)
        {
            settings.Plottables.RemoveAll(x => x.GetType() == plottableType);

            if (settings.Plottables.Count == 0)
                settings.ResetAxisLimits();
        }

        /// <summary>
        /// Remove a specific plottable
        /// </summary>
        public void Remove(IPlottable plottable)
        {
            settings.Plottables.Remove(plottable);

            if (settings.Plottables.Count == 0)
                settings.ResetAxisLimits();
        }

        /// <summary>
        /// Return a copy of the list of plottables
        /// </summary>
        /// <returns></returns>
        public IPlottable[] GetPlottables() => settings.Plottables.ToArray();

        /// <summary>
        /// Return a copy of the list of draggable plottables
        /// </summary>
        /// <returns></returns>
        public IDraggable[] GetDraggables() => settings.Plottables.Where(x => x is IDraggable).Select(x => (IDraggable)x).ToArray();

        /// <summary>
        /// Return the draggable plottable under the mouse cursor (or null if there isn't one)
        /// </summary>
        public IDraggable GetDraggableUnderMouse(double pixelX, double pixelY, int snapDistancePixels = 5)
        {
            double snapWidth = GetSettings(false).XAxis.Dims.UnitsPerPx * snapDistancePixels;
            double snapHeight = GetSettings(false).YAxis.Dims.UnitsPerPx * snapDistancePixels;

            foreach (IDraggable draggable in GetDraggables())
                if (draggable.IsUnderMouse(GetCoordinateX((float)pixelX), GetCoordinateY((float)pixelY), snapWidth, snapHeight))
                    if (draggable.DragEnabled)
                        return draggable;

            return null;
        }

        #endregion

        #region plottable validation

        /// <summary>
        /// Throw an exception if any plottable contains an invalid state. Deep validation is more thorough but slower.
        /// </summary>
        public void Validate(bool deep = true)
        {
            foreach (var plottable in settings.Plottables)
                plottable.ValidateData(deep);
        }

        #endregion

        #region plot settings and styling

        /// <summary>
        /// Get access to the plot settings module (not exposed by default because its internal API changes frequently)
        /// </summary>
        public Settings GetSettings(bool showWarning = true)
        {
            if (showWarning)
                Debug.WriteLine("WARNING: GetSettings() is only for development and testing. " +
                                "Be aware its class structure changes frequently!");
            return settings;
        }

        /// <summary>
        /// Set the default size for new renders
        /// </summary>
        public void SetSize(float width, float height) => settings.Resize(width, height);

        /// <summary>
        /// Return a new color from the Pallette based on the number of plottables already in the plot.
        /// Use this to ensure every plottable gets a unique color.
        /// </summary>
        public Color GetNextColor(double alpha = 1) => Color.FromArgb((byte)(alpha * 255), settings.GetNextColor());

        /// <summary>
        /// Change the default color palette for new plottables
        /// </summary>
        public void Colorset(Drawing.Palette colorset) => settings.PlottablePalette = colorset;

        /// <summary>
        /// Set colors of all plot components using pre-defined styles
        /// </summary>
        public void Style(Style style) => StyleTools.SetStyle(this, style);

        /// <summary>
        /// Set the color of specific plot components
        /// </summary>
        public void Style(
            Color? figureBackground = null,
            Color? dataBackground = null,
            Color? grid = null,
            Color? tick = null,
            Color? axisLabel = null,
            Color? titleLabel = null)
        {
            settings.FigureBackground.Color = figureBackground ?? settings.FigureBackground.Color;
            settings.DataBackground.Color = dataBackground ?? settings.DataBackground.Color;

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

            XAxis2.TickLabelStyle(color: titleLabel);
        }

        #endregion

        #region renderable customization

        /// <summary>
        /// Display render benchmark information on the plot
        /// </summary>
        public void Benchmark(bool enable = true) => settings.BenchmarkMessage.IsVisible = enable;
        public void BenchmarkToggle() => settings.BenchmarkMessage.IsVisible = !settings.BenchmarkMessage.IsVisible;

        /// <summary>
        /// Set legend visibility and location. Save the returned object for additional customizations.
        /// </summary>
        public Renderable.Legend Legend(bool enable = true, Alignment location = Alignment.LowerRight)
        {
            settings.CornerLegend.IsVisible = enable;
            settings.CornerLegend.Location = location;
            return settings.CornerLegend;
        }

        #endregion

        #region copy

        /// <summary>
        /// Return a new Plot with all the same Plottables (and some of the styles) of this one.
        /// This is called when you right-click a plot in a control and hit "open in new window".
        /// </summary>
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

        #endregion
    }
}
