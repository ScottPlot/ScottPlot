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

namespace WPF.Examples
{
    /// <summary>
    /// Interaction logic for ProceduralPlots.xaml
    /// </summary>
    public partial class ProceduralPlots : Window
    {
        public ProceduralPlots()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            CreateAndFillGrid(5, 7);
        }

        private void CreateAndFillGrid(int columnCount, int rowCount)
        {
            MainGrid.ColumnDefinitions.Clear();
            for(int i=0; i<columnCount; i++)
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());

            MainGrid.RowDefinitions.Clear();
            for (int i = 0; i < rowCount; i++)
                MainGrid.RowDefinitions.Add(new RowDefinition());

            Random rand = new Random();
            for(int rowIndex=0; rowIndex<rowCount; rowIndex++)
            {
                for (int colIndex = 0; colIndex < columnCount; colIndex++)
                {
                    var wpfPlot = new ScottPlot.WpfPlot();
                    wpfPlot.plt.Title($"column {colIndex + 1}, row {rowIndex + 1}");
                    wpfPlot.plt.PlotSignal(ScottPlot.DataGen.RandomWalk(rand, 100));
                    wpfPlot.Render();

                    Grid.SetColumn(wpfPlot, colIndex);
                    Grid.SetRow(wpfPlot, rowIndex);
                    MainGrid.Children.Add(wpfPlot);
                }
            }
        }
    }
}
