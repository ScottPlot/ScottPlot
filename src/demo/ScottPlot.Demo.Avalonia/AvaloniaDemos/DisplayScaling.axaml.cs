using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using System;
using System.Collections.Generic;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class DisplayScalingViewModel
    {
        private Action _onChange;
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

    public class DisplayScaling : Window
    {
        private readonly DisplayScalingViewModel viewModel;

        private readonly AvaPlot avaPlot1;

        public DisplayScaling()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            avaPlot1 = this.Find<AvaPlot>("avaPlot1");

            avaPlot1.Plot.AddSignal(DataGen.Sin(51));
            avaPlot1.Plot.AddSignal(DataGen.Cos(51));
            avaPlot1.Refresh();

            viewModel = new DisplayScalingViewModel(() => this.CheckChanged());
            this.DataContext = viewModel;
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
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
