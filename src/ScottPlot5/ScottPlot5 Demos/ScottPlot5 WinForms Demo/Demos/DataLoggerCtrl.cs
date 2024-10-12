using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using ScottPlot;
using ScottPlot.AxisPanels;
using ScottPlot.Collections;
using Crosshair = ScottPlot.Plottables.Crosshair;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace WinForms_Demo.Demos
{
    public partial class DataLoggerCtrl : UserControl
    {
        private bool _rotated;
        private BottomAxis? _secondXAxis;
        private RightAxis? _secondYAxis;
        private Crosshair _crossHair1;
        private bool _tracking;

        public DataLoggerCtrl()
        {
            InitializeComponent();

            // disable interactivity by default
            formsPlot.UserInputProcessor.Disable();

            InitPlots();
        }

        [MemberNotNull(nameof(Logger1))]
        [MemberNotNull(nameof(Logger2))]
        [MemberNotNull(nameof(_crossHair1))]
        private void InitPlots()
        {
            formsPlot.Plot.Clear();

            // create two horizontal loggers and add them to the plot
            var data1 = new CircularBuffer<Coordinates>(1000);
            Logger1 = new ScottPlot.Plottables.DataLogger(data1) { Color = Colors.C0, Rotated = Rotated };
            formsPlot.Plot.Add.Plottable(Logger1);

            var data2 = new CircularBuffer<Coordinates>(1000);
            Logger2 = new ScottPlot.Plottables.DataLogger(data2) { Color = Colors.C1, Rotated = Rotated };
            formsPlot.Plot.Add.Plottable(Logger2);

            if (Rotated)
            {
                // use the bottom axis (already there by default) for the first logger
                BottomAxis axis1 = (BottomAxis)formsPlot.Plot.Axes.Bottom;
                Logger1.Axes.XAxis = axis1;
                Logger1.Axes.YAxis = formsPlot.Plot.Axes.Right;
                axis1.Color(Logger1.Color);

                // create and add a secondary bottom axis to use for the other logger
                _secondXAxis = formsPlot.Plot.Axes.AddBottomAxis();
                Logger2.Axes.XAxis = _secondXAxis;
                Logger2.Axes.YAxis = formsPlot.Plot.Axes.Right;
                _secondXAxis.Color(Logger2.Color);

                if (_secondYAxis != null)
                {
                    formsPlot.Plot.Axes.Remove(_secondYAxis);
                    _secondYAxis = null;
                }
            }
            else
            {
                // use the right axis (already there by default) for the first logger
                RightAxis axis1 = (RightAxis)formsPlot.Plot.Axes.Right;
                Logger1.Axes.YAxis = axis1;
                axis1.Color(Logger1.Color);

                // create and add a secondary right axis to use for the other logger
                _secondYAxis = formsPlot.Plot.Axes.AddRightAxis();
                Logger2.Axes.YAxis = _secondYAxis;
                _secondYAxis.Color(Logger2.Color);

                if (_secondXAxis != null)
                {
                    formsPlot.Plot.Axes.Remove(_secondXAxis);
                    _secondXAxis = null;
                }
            }

            _crossHair1 = formsPlot.Plot.Add.Crosshair(0, 0);
            _crossHair1.IsVisible = false;
            _crossHair1.MarkerShape = MarkerShape.OpenCircle;
            _crossHair1.MarkerSize = 15;
            _crossHair1.Axes.XAxis = Logger1.Axes.XAxis;
            _crossHair1.Axes.YAxis = Logger1.Axes.YAxis;
        }

        private void btnFull_Click(object sender, EventArgs e)
        {
            Logger1.ViewFull();
            Logger2.ViewFull();
        }

        private void btnJump_Click(object sender, EventArgs e)
        {
            Logger1.ViewJump();
            Logger2.ViewJump();
        }

        private void btnSlide_Click(object sender, EventArgs e)
        {
            Logger1.ViewSlide();
            Logger2.ViewSlide();
        }

        private void cbInverted_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot.Plot.Axes.AutoScale(invertX: cbInvertedX.Checked, invertY: cbInvertedY.Checked);
        }

        public ScottPlot.Plottables.DataLogger Logger1 { get; private set; }
        public ScottPlot.Plottables.DataLogger Logger2 { get; private set; }

        public void RefreshPlot()
        {
            if (Logger1.HasNewData || Logger2.HasNewData)
            {
                formsPlot.Refresh();
            }
        }

        private void formsPlot_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Tracking)
            {
                return;
            }

            // determine where the mouse is and get the nearest point
            Pixel mousePixel = new(e.Location.X, e.Location.Y);
            Coordinates mouseLocation = formsPlot.Plot.GetCoordinates(mousePixel, Logger1.Axes.XAxis, Logger1.Axes.YAxis);

            DataPoint nearest = Logger1.GetNearest(mouseLocation, formsPlot.Plot.LastRender.DataRect);

            // place the crosshair over the highlighted point
            if (nearest.IsReal)
            {
                _crossHair1.IsVisible = true;

                var coordinates = nearest.Coordinates;
                _crossHair1.Position = Rotated ? coordinates.Rotated : coordinates;

                formsPlot.Refresh();
                tbStatus.Text = $"Selected Index={nearest.Index}, X={nearest.X:0.##}, Y={nearest.Y:0.##}";
            }

            //hide the crosshair when no point is selected
            if (!nearest.IsReal && _crossHair1.IsVisible)
            {
                _crossHair1.IsVisible = false;
                formsPlot.Refresh();
                tbStatus.Text = $"No point selected";
            }
        }

        [Description("True if the control is rotated")]
        [DefaultValue(false)]
        public bool Rotated
        {
            get => _rotated;
            set
            {
                bool changed = _rotated != value;

                _rotated = value;

                if (changed)
                {
                    InitPlots();
                }
            }
        }

        [Description("True if tracking is enabled")]
        [DefaultValue(false)]
        public bool Tracking
        {
            get => _tracking;
            set
            {
                if (_tracking == value)
                {
                    return;
                }

                if (!value)
                {
                    _crossHair1.IsVisible = false;
                    formsPlot.Refresh();
                }

                _tracking = value;
            }
        }
    }
}
