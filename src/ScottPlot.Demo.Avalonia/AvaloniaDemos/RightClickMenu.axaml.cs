using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using ScottPlot.Interactive;
using System;
using System.Collections.Generic;

namespace ScottPlot.Demo.Avalonia.AvaloniaDemos
{
    public class RightClickMenu : Window
    {
        AvaPlot avaPlot1;

        public RightClickMenu()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            avaPlot1 = this.Find<AvaPlot>("avaPlot1");

            avaPlot1.plt.PlotSignal(DataGen.Sin(51));
            avaPlot1.plt.PlotSignal(DataGen.Cos(51));
            avaPlot1.Render();

            List<ContextMenuItem> contextMenu = new List<ContextMenuItem>();
            contextMenu.Add(new ContextMenuItem()
            {
                itemName = "Add Sine Wave",
                onClick = AddSine
            });

            contextMenu.Add(new ContextMenuItem()
            {
                itemName = "Clear Plot",
                onClick = ClearPlot
            });

            avaPlot1.SetContextMenu(contextMenu);
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void AddSine()
        {
            Random rand = new Random();
            avaPlot1.plt.PlotSignal(DataGen.Sin(51, phase: rand.NextDouble() * 1000));
            avaPlot1.plt.AxisAuto();
            avaPlot1.Render();
        }

        private void ClearPlot()
        {
            avaPlot1.plt.Clear();
            avaPlot1.plt.AxisAuto();
            avaPlot1.Render();
        }
    }
}
