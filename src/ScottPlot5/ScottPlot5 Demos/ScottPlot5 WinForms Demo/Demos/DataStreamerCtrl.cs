using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using ScottPlot;
using ScottPlot.AxisPanels;
using ScottPlot.Collections;
using ScottPlot.DataSources;

namespace WinForms_Demo.Demos
{
    public partial class DataStreamerCtrl : UserControl
    {
        private bool _rotated;
        private BottomAxis? _secondXAxis;
        private RightAxis? _secondYAxis;

        public DataStreamerCtrl()
        {
            InitializeComponent();

            // disable interactivity by default
            formsPlot.Interaction.Disable();

            InitPlots();
        }

        [MemberNotNull(nameof(Logger1))]
        [MemberNotNull(nameof(Logger2))]
        private void InitPlots()
        {
            formsPlot.Plot.Clear();

            // create two horizontal loggers and add them to the plot
            var data1 = new DataStreamer2Source(new CircularBuffer<Coordinates>(1000));
            Logger1 = new ScottPlot.Plottables.Experimental.DataStreamer2(data1) { Color = Colors.C0, Rotated = Rotated };
            formsPlot.Plot.Add.Plottable(Logger1);

            var data2 = new DataStreamer2Source(new CircularBuffer<Coordinates>(1000));
            Logger2 = new ScottPlot.Plottables.Experimental.DataStreamer2(data2) { Color = Colors.C1, Rotated = Rotated };
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

        public ScottPlot.Plottables.Experimental.DataStreamer2 Logger1 { get; private set; }
        public ScottPlot.Plottables.Experimental.DataStreamer2 Logger2 { get; private set; }

        public void RefreshPlot()
        {
            if (Logger1.HasNewData || Logger2.HasNewData)
            {
                formsPlot.Refresh();
            }
        }

        [Description("True if the control is rotated")]
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
    }
}
