using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotCookbook
{
    class Recipes
    {
        int width;
        int height;
        string outputFolderName;
        Random rand = new Random(0);

        public Recipes(string outputFolderName, int width, int height)
        {
            this.outputFolderName = outputFolderName;
            this.width = width;
            this.height = height;
            PrepareDataSmall();
        }

        double[] dataXs;
        double[] dataSin;
        double[] dataCos;
        double[] dataRandom1;
        double[] dataRandom2;
        double[] dataRandom3;
        double[] dataRandom4;

        private void PrepareDataSmall(int pointCount = 50)
        {
            dataXs = new double[pointCount];
            dataSin = new double[pointCount];
            dataCos = new double[pointCount];
            dataRandom1 = new double[pointCount];
            dataRandom2 = new double[pointCount];
            dataRandom3 = new double[pointCount];
            dataRandom4 = new double[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                dataXs[i] = i;
                dataSin[i] = Math.Sin(i * 2 * Math.PI / pointCount);
                dataCos[i] = Math.Cos(i * 2 * Math.PI / pointCount);
                dataRandom1[i] = rand.NextDouble() * 100 - 50;
                dataRandom2[i] = rand.NextDouble() * 100 - 50;
                dataRandom3[i] = rand.NextDouble() * 100;
                dataRandom4[i] = rand.NextDouble() * 100;
            }
        }

        #region plot types

        public void Figure_01_Scatter_Sin()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.AxisAuto();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
        }

        public void Figure_02_Scatter_Sin_Styled()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, color: Color.Magenta, lineWidth: 0, markerSize: 10);
            plt.PlotScatter(dataXs, dataCos, color: Color.Green, lineWidth: 5, markerSize: 0);
            plt.AxisAuto();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
        }

        public void Figure_03_Scatter_XY()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataRandom1, dataRandom2);
            plt.AxisAuto();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
        }

        public void Figure_04_Scatter_XY_Lines_Only()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataRandom1, dataRandom2, markerSize: 0);
            plt.PlotScatter(dataRandom3, dataRandom4, markerSize: 0);
            plt.AxisAuto();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
        }

        public void Figure_05_Scatter_XY_Points_Only()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataRandom1, dataRandom2, lineWidth: 0);
            plt.PlotScatter(dataRandom3, dataRandom4, lineWidth: 0);
            plt.AxisAuto();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
        }

        public void Figure_06_Scatter_XY_Styling()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataRandom1, dataRandom2, color: Color.Magenta, lineWidth: 3, markerSize: 15);
            plt.PlotScatter(dataRandom3, dataRandom4, color: Color.Green, lineWidth: 3, markerSize: 15);
            plt.AxisAuto();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
        }

        public void Figure_07_Point()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.PlotPoint(25, 0.8);
            plt.PlotPoint(30, 0.3, color: Color.Magenta, markerSize: 15);
            plt.AxisAuto();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
        }

        public void Figure_08_Text()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.PlotPoint(25, 0.8);
            plt.PlotPoint(30, 0.3, color: Color.Magenta, markerSize: 15);
            plt.PlotText("important point", 25, 0.8);
            plt.PlotText("more important", 30, .3, fontSize: 16, bold: true);
            plt.AxisAuto();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
        }
        #endregion

        #region customization and styling

        public void Figure_20_Small_Plot()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(200, 150);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.AxisAuto();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
        }

        public void Figure_21_Title_and_Axis_Labels()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.AxisAuto();

            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");

            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
        }

        public void Figure_22_Custom_Colors()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);

            plt.settings.figureBackgroundColor = ColorTranslator.FromHtml("#001021");
            plt.settings.dataBackgroundColor = ColorTranslator.FromHtml("#021d38");
            plt.settings.gridColor = ColorTranslator.FromHtml("#273c51");
            plt.settings.titleColor = Color.White;
            plt.settings.tickColor = Color.LightGray;
            plt.settings.axisLabelColor = Color.LightGray;

            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");
            plt.AxisAuto();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
        }

        public void Figure_23_Frameless()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);
            plt.settings.figureBackgroundColor = ColorTranslator.FromHtml("#001021");
            plt.settings.dataBackgroundColor = ColorTranslator.FromHtml("#021d38");
            plt.settings.gridColor = ColorTranslator.FromHtml("#273c51");
            plt.settings.displayTicksX = false;
            plt.settings.displayTicksY = false;
            plt.settings.displayFrame = false;
            plt.TightenLayout(0);

            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.AxisAuto();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
        }

        #endregion

    }
}
