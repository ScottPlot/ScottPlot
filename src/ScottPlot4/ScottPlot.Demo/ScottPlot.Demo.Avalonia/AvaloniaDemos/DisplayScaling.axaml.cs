using System;

using Avalonia.Controls;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class DisplayScalingViewModel
    {
        private readonly Action _onChange;
        public DisplayScalingViewModel(Action onChange)
        {
            _onChange = onChange;
        }

        private bool _primary = true;
        public bool Primary
        {
            get => _primary;
            set
            {
                _primary = value;
                _onChange();
            }
        }
    }

    public partial class DisplayScaling : Window
    {
        private readonly DisplayScalingViewModel viewModel;

        public DisplayScaling()
        {
            this.InitializeComponent();

            avaPlot1.Plot.AddSignal(DataGen.Sin(51));
            avaPlot1.Plot.AddSignal(DataGen.Cos(51));
            avaPlot1.Refresh();

            viewModel = new DisplayScalingViewModel(() => this.CheckChanged());
            this.DataContext = viewModel;
        }

        private void CheckChanged()
        {
            avaPlot1.Configuration.DpiStretch = viewModel.Primary;
            avaPlot1.Plot.Title(
                $"System Scaling: {Drawing.GDI.GetScaleRatio() * 100}%\n" +
                $"DPI Stretch Ratio: {avaPlot1.Configuration.DpiStretchRatio}");
            avaPlot1.Refresh();
        }
    }
}
