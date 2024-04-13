using System;

using Avalonia.Controls;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public partial class RightClickMenu : Window
    {
        public RightClickMenu()
        {
            this.InitializeComponent();

            avaPlot1.Plot.AddSignal(DataGen.Sin(51));
            avaPlot1.Plot.AddSignal(DataGen.Cos(51));
            avaPlot1.Refresh();

            ContextMenu contextMenu = new ContextMenu
            {
                ItemsSource = new[] {
                    MakeMenuItem("Add Sine Wave", AddSine),
                    MakeMenuItem("Clear Plot", ClearPlot)
                }
            };

            avaPlot1.ContextMenu = contextMenu;
        }

        private static MenuItem MakeMenuItem(string label, Action onClick)
        {
            MenuItem menuItem = new MenuItem() { Header = label };
            menuItem.Click += (sender, e) => onClick();
            return menuItem;
        }

        private void AddSine()
        {
            Random rand = new Random();
            avaPlot1.Plot.AddSignal(DataGen.Sin(51, phase: rand.NextDouble() * 1000));
            avaPlot1.Plot.AxisAuto();
            avaPlot1.Refresh();
        }

        private void ClearPlot()
        {
            avaPlot1.Plot.Clear();
            avaPlot1.Plot.AxisAuto();
            avaPlot1.Refresh();
        }
    }
}
