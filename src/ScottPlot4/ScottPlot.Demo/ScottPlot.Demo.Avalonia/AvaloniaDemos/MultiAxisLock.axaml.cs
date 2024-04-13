using System;

using Avalonia.Controls;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class MultiAxisLockViewModel
    {
        private bool _primary = true;
        public bool Primary
        {
            get => _primary; set
            {
                _primary = value;
                _onChange();
            }
        }
        private bool _secondary = true;

        public bool Secondary
        {
            get => _secondary; set
            {
                _secondary = value;
                _onChange();
            }
        }
        private bool _tertiary = true;

        public bool Tertiary
        {
            get => _tertiary; set
            {
                _tertiary = value;
                _onChange();
            }
        }
        private readonly Action _onChange;

        public MultiAxisLockViewModel(Action onChange)
        {
            _onChange = onChange;
        }
    }
    /// <summary>
    /// Interaction logic for MultiAxisLock.axaml
    /// </summary>
    public partial class MultiAxisLock : Window
    {
        private readonly ScottPlot.Renderable.Axis YAxis3;
        private readonly MultiAxisLockViewModel viewModel;

        public MultiAxisLock()
        {
            InitializeComponent();

            viewModel = new MultiAxisLockViewModel(() => this.CheckChanged());
            this.DataContext = viewModel;

            Random rand = new Random();

            // Add 3 signals each with a different vertical axis index.
            // Each signal defaults to X axis index 0 so their horizontal axis will be shared.

            var plt1 = AvaPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100, mult: 1));
            plt1.YAxisIndex = 0;
            plt1.LineWidth = 3;
            plt1.Color = System.Drawing.Color.Magenta;

            var plt2 = AvaPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100, mult: 10));
            plt2.YAxisIndex = 1;
            plt2.LineWidth = 3;
            plt2.Color = System.Drawing.Color.Green;

            var plt3 = AvaPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100, mult: 100));
            plt3.YAxisIndex = 2;
            plt3.LineWidth = 3;
            plt3.Color = System.Drawing.Color.Navy;

            // The horizontal axis is shared by these signal plots (XAxisIndex defaults to 0)
            AvaPlot1.Plot.XAxis.Label("Horizontal Axis");

            // Customize the primary (left) and secondary (right) axes
            AvaPlot1.Plot.YAxis.Color(System.Drawing.Color.Magenta);
            AvaPlot1.Plot.YAxis.Label("Primary Axis");
            AvaPlot1.Plot.YAxis2.Color(System.Drawing.Color.Green);
            AvaPlot1.Plot.YAxis2.Label("Secondary Axis");

            // the secondary (right) axis ticks are hidden by default so enable them
            AvaPlot1.Plot.YAxis2.Ticks(true);

            // Create an additional vertical axis and customize it
            YAxis3 = AvaPlot1.Plot.AddAxis(Renderable.Edge.Left, 2);
            YAxis3.Color(System.Drawing.Color.Navy);
            YAxis3.Label("Tertiary Axis");

            // adjust axis limits to fit the data once before locking them
            AvaPlot1.Plot.AxisAuto();
            CheckChanged();
        }

        private void CheckChanged()
        {
            if (AvaPlot1 is null)
            {
                return;
            }

            AvaPlot1.Plot.YAxis.LockLimits(!viewModel.Primary);
            AvaPlot1.Plot.YAxis2.LockLimits(!viewModel.Secondary);
            YAxis3.LockLimits(!viewModel.Tertiary);
        }
    }
}
