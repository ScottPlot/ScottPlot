using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScottPlot.Demo.WPF.WpfDemos
{
    public class PlotElement
    {
        public int columnIndex { get; set; }
        public int rowIndex { get; set; }
        public WpfPlot wpfPlot { get; set; }

        public PlotElement(int columnIndex, int rowIndex, Plot plt)
        {
            this.columnIndex = columnIndex;
            this.rowIndex = rowIndex;
            wpfPlot = new WpfPlot();
            wpfPlot.Reset(plt);
            wpfPlot.Refresh();
        }
    }

    public partial class ManyPlots : Window
    {
        public List<PlotElement> plots { get; set; } = new List<PlotElement>();
        Random rand = new Random(0);

        public ManyPlots()
        {
            InitializeComponent();

            int columnCount = 12;
            int rowCount = 8;
            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var plt = new ScottPlot.Plot();
                    plt.AddSignal(DataGen.RandomWalk(rand, 10));
                    plt.Title($"Well {(char)(65 + rowIndex)} {columnIndex + 1}");
                    plt.XAxis.TickLabelStyle(fontSize: 10);
                    plt.YAxis.TickLabelStyle(fontSize: 10);
                    plt.Grid(false);
                    plots.Add(new PlotElement(columnIndex, rowIndex, plt));
                }
            }
        }
    }
}
