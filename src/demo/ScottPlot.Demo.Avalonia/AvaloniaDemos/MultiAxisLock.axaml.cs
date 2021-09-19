using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ScottPlot.Avalonia;
using System;

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
        private Action _onChange;

        public MultiAxisLockViewModel(Action onChange)
        {
            _onChange = onChange;
        }

    }
    /// <summary>
    /// Interaction logic for MultiAxisLock.axaml
    /// </summary>
    public class MultiAxisLock : Window
    {
        private readonly ScottPlot.Renderable.Axis YAxis3;
        private readonly AvaPlot avaPlot1;
        private readonly MultiAxisLockViewModel viewModel;

        public MultiAxisLock()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            viewModel = new MultiAxisLockViewModel(() => this.CheckChanged());
            this.DataContext = viewModel;
            avaPlot1 = this.Find<AvaPlot>("AvaPlot1");

            Random rand = new Random();

            // Add 3 signals each with a different vertical axis index.
            // Each signal defaults to X axis index 0 so their horizontal axis will be shared.

            var plt1 = avaPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100, mult: 1));
            plt1.YAxisIndex = 0;
            plt1.LineWidth = 3;
            plt1.Color = System.Drawing.Color.Magenta;

            var plt2 = avaPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100, mult: 10));
            plt2.YAxisIndex = 1;
            plt2.LineWidth = 3;
            plt2.Color = System.Drawing.Color.Green;

            var plt3 = avaPlot1.Plot.AddSignal(DataGen.RandomWalk(rand, 100, mult: 100));
            plt3.YAxisIndex = 2;
            plt3.LineWidth = 3;
            plt3.Color = System.Drawing.Color.Navy;

            // The horizontal axis is shared by these signal plots (XAxisIndex defaults to 0)
            avaPlot1.Plot.XAxis.Label("Horizontal Axis");

            // Customize the primary (left) and secondary (right) axes
            avaPlot1.Plot.YAxis.Color(System.Drawing.Color.Magenta);
            avaPlot1.Plot.YAxis.Label("Primary Axis");
            avaPlot1.Plot.YAxis2.Color(System.Drawing.Color.Green);
            avaPlot1.Plot.YAxis2.Label("Secondary Axis");

            // the secondary (right) axis ticks are hidden by default so enable them
            avaPlot1.Plot.YAxis2.Ticks(true);

            // Create an additional vertical axis and customize it
            YAxis3 = avaPlot1.Plot.AddAxis(Renderable.Edge.Left, 2);
            YAxis3.Color(System.Drawing.Color.Navy);
            YAxis3.Label("Tertiary Axis");

            // adjust axis limits to fit the data once before locking them
            avaPlot1.Plot.AxisAuto();
            CheckChanged();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void CheckChanged()
        {
            if (avaPlot1 is null)
            {
                return;
            }

            avaPlot1.Plot.YAxis.LockLimits(!viewModel.Primary);
            avaPlot1.Plot.YAxis2.LockLimits(!viewModel.Secondary);
            YAxis3.LockLimits(!viewModel.Tertiary);
        }
    }
}
