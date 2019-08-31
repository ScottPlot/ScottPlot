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
        }

        public string Figure_01a_Scatter_Sin()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_01b_Automatic_Margins()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.AxisAuto(0, .5); // no horizontal padding, 50% vertical padding
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_01c_Defined_Axis_Limits()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.Axis(2, 8, .2, 1.1); // x1, x2, y1, y2
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_01d_Zoom_and_Pan()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.AxisZoom(2, 2);
            plt.AxisPan(-10, .5);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }
        public string Figure_01e_Legend()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, label: "first");
            plt.PlotScatter(dataXs, dataCos, label: "second");
            plt.Legend(location: ScottPlot.legendLocation.lowerLeft);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_01f_Custom_Marker_Shapes()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, label: "sin", markerShape: ScottPlot.MarkerShape.openCircle);
            plt.PlotScatter(dataXs, dataCos, label: "cos", markerShape: ScottPlot.MarkerShape.filledSquare);
            plt.Legend();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_01g_All_Marker_Shapes()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("ScottPlot Marker Shapes");
            plt.Grid(false);

            // plot a sine wave for every marker available
            string[] markerShapeNames = Enum.GetNames(typeof(ScottPlot.MarkerShape));
            for (int i = 0; i < markerShapeNames.Length; i++)
            {
                string markerShapeName = markerShapeNames[i];
                var markerShape = (ScottPlot.MarkerShape)Enum.Parse(typeof(ScottPlot.MarkerShape), markerShapeName);
                double[] stackedSin = ScottPlot.DataGen.Sin(dataXs.Length, 2, -i);
                plt.PlotScatter(dataXs, stackedSin, label: markerShapeName, markerShape: markerShape);
            }

            plt.Legend(fontSize: 10);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_02_Styling_Scatter_Plots()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, color: Color.Magenta, lineWidth: 0, markerSize: 10);
            plt.PlotScatter(dataXs, dataCos, color: Color.Green, lineWidth: 5, markerSize: 0);
            plt.AxisAuto(0); // no horizontal margin (default 10% vertical margin)
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_03_Plot_XY_Data()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            Random rand = new Random(0);
            int pointCount = 50;
            double[] dataRandom1 = ScottPlot.DataGen.RandomNormal(rand, pointCount);
            double[] dataRandom2 = ScottPlot.DataGen.RandomNormal(rand, pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataRandom1, dataRandom2);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_04_Plot_Lines_Only()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            Random rand = new Random(0);
            int pointCount = 50;
            double[] dataRandom1 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 1);
            double[] dataRandom2 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 2);
            double[] dataRandom3 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 3);
            double[] dataRandom4 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 4);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataRandom1, dataRandom2, markerSize: 0);
            plt.PlotScatter(dataRandom3, dataRandom4, markerSize: 0);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_05_Plot_Points_Only()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            Random rand = new Random(0);
            int pointCount = 50;
            double[] dataRandom1 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 1);
            double[] dataRandom2 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 2);
            double[] dataRandom3 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 3);
            double[] dataRandom4 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 4);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataRandom1, dataRandom2, lineWidth: 0);
            plt.PlotScatter(dataRandom3, dataRandom4, lineWidth: 0);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_06_Styling_XY_Plots()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            Random rand = new Random(0);
            int pointCount = 50;
            double[] dataRandom1 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 1);
            double[] dataRandom2 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 2);
            double[] dataRandom3 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 3);
            double[] dataRandom4 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 4);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataRandom1, dataRandom2, color: Color.Magenta, lineWidth: 3, markerSize: 15);
            plt.PlotScatter(dataRandom3, dataRandom4, color: Color.Green, lineWidth: 3, markerSize: 15);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_06b_Custom_LineStyles()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            Random rand = new Random(0);
            int pointCount = 50;
            double[] dataRandom1 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 1);
            double[] dataRandom2 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 2);
            double[] dataRandom3 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 3);
            double[] dataRandom4 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 4);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataRandom1, dataRandom2, label: "dash", lineStyle: ScottPlot.LineStyle.Dash);
            plt.PlotScatter(dataRandom3, dataRandom4, label: "dash dot dot", lineStyle: ScottPlot.LineStyle.DashDotDot);
            plt.Legend();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_07_Plotting_Points()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.PlotPoint(25, 0.8);
            plt.PlotPoint(30, 0.3, color: Color.Magenta, markerSize: 15);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_08_Plotting_Text()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.PlotPoint(25, 0.8);
            plt.PlotPoint(30, 0.3, color: Color.Magenta, markerSize: 15);
            plt.PlotText("important point", 25, 0.8);
            plt.PlotText("more important", 30, .3, fontSize: 16, bold: true, alignment: ScottPlot.TextAlignment.upperCenter);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_09_Clearing_Plots()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            Random rand = new Random(0);
            double[] dataRandom1 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 1);
            double[] dataRandom2 = ScottPlot.DataGen.RandomNormal(rand, pointCount, 2);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.Clear();
            plt.PlotScatter(dataRandom1, dataRandom2);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_10_Modifying_Plotted_Data()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);

            // After an array is plotted with PlotSignal() or PlotScatter() its contents 
            //   can be updated (by changing values in the array) and they will be displayed 
            //   at the next render. This makes it easy to create live displays.

            for (int i = 10; i < 20; i++)
            {
                dataSin[i] = i / 10.0;
                dataCos[i] = 2 * i / 10.0;
            }

            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_11_Modify_Styles_After_Plotting()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);

            // All Plot functions return the object that was just created.
            var scatter1 = plt.PlotScatter(dataXs, dataSin);
            var scatter2 = plt.PlotScatter(dataXs, dataCos);
            var horizontalLine = plt.PlotHLine(0, lineWidth: 3);

            // This allows you to modify the object's properties later.
            scatter1.color = Color.Pink;
            scatter2.markerShape = ScottPlot.MarkerShape.openCircle;
            horizontalLine.position = 0.7654;

            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_12_Date_Axis()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            Random rand = new Random(0);
            double[] price = ScottPlot.DataGen.RandomWalk(rand, 60 * 8);
            DateTime start = new DateTime(2019, 08, 25, 8, 30, 00);
            double pointsPerDay = 24 * 60; // one point per minute

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotSignal(price, sampleRate: pointsPerDay, xOffset: start.ToOADate());
            plt.Ticks(dateTimeX: true);
            plt.YLabel("Price");
            plt.XLabel("Date and Time");

            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_20_Small_Plot()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(200, 150);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_21a_Title_and_Axis_Labels()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);

            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");

            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_21b_Extra_Padding()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);

            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");

            plt.TightenLayout(padding: 40);

            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_22_Custom_Colors()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            Color figureBgColor = ColorTranslator.FromHtml("#001021");
            Color dataBgColor = ColorTranslator.FromHtml("#021d38");
            plt.Style(figBg: figureBgColor, dataBg: dataBgColor);
            plt.Grid(color: ColorTranslator.FromHtml("#273c51"));
            plt.Ticks(color: Color.LightGray);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.Title("Very Complicated Data", color: Color.White);
            plt.XLabel("Experiment Duration", color: Color.LightGray);
            plt.YLabel("Productivity", color: Color.LightGray);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_23_Frameless_Plot()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            Color figureBgColor = ColorTranslator.FromHtml("#001021");
            Color dataBgColor = ColorTranslator.FromHtml("#021d38");
            plt.Style(figBg: figureBgColor, dataBg: dataBgColor);
            plt.Grid(color: ColorTranslator.FromHtml("#273c51"));
            plt.Ticks(displayTicksX: false, displayTicksY: false);
            plt.Frame(drawFrame: false);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_24_Disable_the_Grid()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.Grid(false);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_25_Corner_Axis_Frame()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.Grid(false);
            plt.Frame(right: false, top: false);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_26_Horizontal_Ticks_Only()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.Grid(false);
            plt.Ticks(displayTicksY: false);
            plt.Frame(left: false, right: false, top: false);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_27_Very_Large_Numbers()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            Random rand = new Random(0);
            int pointCount = 100;
            double[] largeXs = ScottPlot.DataGen.Consecutive(pointCount, 1e17);
            double[] largeYs = ScottPlot.DataGen.Random(rand, pointCount, 1e21);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(largeXs, largeYs);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_28_Axis_Exponent_And_Offset()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            double bigNumber = 9876;

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("panned far and really zoomed in");
            plt.Axis(bigNumber, bigNumber + .00001, bigNumber, bigNumber + .00001);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_28b_Multiplier_Notation_Default()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            double[] tenMillionPoints = ScottPlot.DataGen.SinSweep(10_000_000, 8);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotSignal(tenMillionPoints);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_28c_Multiplier_Notation_Disabled()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            double[] tenMillionPoints = ScottPlot.DataGen.SinSweep(10_000_000, 8);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotSignal(tenMillionPoints);
            plt.Ticks(useMultiplierNotation: false); // <-- THIS
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_29_Very_Large_Images()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(2000, 1000);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_30a_Signal()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            double[] tenMillionPoints = ScottPlot.DataGen.SinSweep(10_000_000, 8);

            // PlotSignal() is much faster than PlotScatter() for large arrays of evenly-spaed data.
            // To plot more than 2GB of data, enable "gcAllowVeryLargeObjects" in App.config (Google it)

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Displaying 10 million points with PlotSignal()");
            plt.Benchmark();
            plt.PlotSignal(tenMillionPoints, sampleRate: 20_000);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_30b_Signal_With_Parallel_Processing()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            double[] tenMillionPoints = ScottPlot.DataGen.SinSweep(10_000_000, 8);

            // PlotSignal() can get a speed boost using parallel processing.
            // this isn't extensively tested yet, so use this feature at your own risk.

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Displaying 10 million points with PlotSignal() + parallel");
            plt.Benchmark();
            plt.Parallel(true);
            plt.PlotSignal(tenMillionPoints, sampleRate: 20_000);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_30c_SignalConst()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            double[] tenMillionPoints = ScottPlot.DataGen.SinSweep(10_000_000, 8);

            // SignalConst() is faster than PlotSignal() for very large data plots
            // - its data cannot be modified after it is loaded
            // - here threading was turned off so it renders properly in a console application
            // - in GUI applications threading allows it to initially render faster but here it is turned off

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Displaying 10 million points with PlotSignalConst()");
            plt.Benchmark();
            plt.PlotSignalConst(tenMillionPoints, sampleRate: 20_000);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_30d_SignalConst_One_Billion_Points()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            // SignalConst() accepts generic data types (here a byte array with a billion points)
            byte[] oneBillionPoints = ScottPlot.DataGen.SinSweepByte(1_000_000_000, 8);

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Display One Billion points with PlotSignalConst()");
            plt.Benchmark();
            plt.Parallel(false);
            plt.PlotSignalConst(oneBillionPoints, sampleRate: 20_000_000);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_32_Signal_Styling()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            double[] tenMillionPoints = ScottPlot.DataGen.SinSweep(10_000_000, 8);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotSignal(tenMillionPoints, 20000, lineWidth: 3, color: Color.Red);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_40_Vertical_and_Horizontal_Lines()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.PlotVLine(17);
            plt.PlotHLine(-.25, color: Color.Red, lineWidth: 3);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_41_Axis_Spans()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);

            // things plotted after before spans are covered by them
            plt.PlotScatter(dataXs, dataSin, label: "below",
                color: Color.Red, markerShape: ScottPlot.MarkerShape.filledCircle);

            // vertical lines and horizontal spans both take X-axis positions
            plt.PlotVLine(17, label: "vertical line");
            plt.PlotVSpan(19, 27, label: "horizontal span", color: Color.Blue);

            // horizontal lines and vertical spans both take Y-axis positions
            plt.PlotHLine(-.6, label: "horizontal line");
            plt.PlotHSpan(-.25, 0.33, label: "vertical span", color: Color.Green);

            // things plotted after are displayed on top of the spans
            plt.PlotScatter(dataXs, dataCos, label: "above",
                color: Color.Red, markerShape: ScottPlot.MarkerShape.filledSquare);

            plt.Legend();

            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_50_StyleBlue1()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, label: "sin");
            plt.PlotScatter(dataXs, dataCos, label: "cos");
            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");
            plt.Legend();
            plt.Style(ScottPlot.Style.Blue1);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_51_StyleBlue2()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, label: "sin");
            plt.PlotScatter(dataXs, dataCos, label: "cos");
            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");
            plt.Legend();
            plt.Style(ScottPlot.Style.Blue2);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_52_StyleBlue3()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, label: "sin");
            plt.PlotScatter(dataXs, dataCos, label: "cos");
            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");
            plt.Legend();
            plt.Style(ScottPlot.Style.Blue3);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_53_StyleLight1()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, label: "sin");
            plt.PlotScatter(dataXs, dataCos, label: "cos");
            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");
            plt.Legend();
            plt.Style(ScottPlot.Style.Light1);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_54_StyleLight2()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, label: "sin");
            plt.PlotScatter(dataXs, dataCos, label: "cos");
            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");
            plt.Legend();
            plt.Style(ScottPlot.Style.Light2);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_55_StyleGray1()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, label: "sin");
            plt.PlotScatter(dataXs, dataCos, label: "cos");
            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");
            plt.Legend();
            plt.Style(ScottPlot.Style.Gray1);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_56_StyleGray2()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, label: "sin");
            plt.PlotScatter(dataXs, dataCos, label: "cos");
            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");
            plt.Legend();
            plt.Style(ScottPlot.Style.Gray2);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_57_StyleBlack()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, label: "sin");
            plt.PlotScatter(dataXs, dataCos, label: "cos");
            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");
            plt.Legend();
            plt.Style(ScottPlot.Style.Black);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_58_StyleDefault()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, label: "sin");
            plt.PlotScatter(dataXs, dataCos, label: "cos");
            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");
            plt.Legend();
            plt.Style(ScottPlot.Style.Default);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_59_StyleControl()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin, label: "sin");
            plt.PlotScatter(dataXs, dataCos, label: "cos");
            plt.Title("Very Complicated Data");
            plt.XLabel("Experiment Duration");
            plt.YLabel("Productivity");
            plt.Legend();
            plt.Style(ScottPlot.Style.Control);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_60_Plotting_With_Errorbars()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            var plt = new ScottPlot.Plot(width, height);
            plt.Grid(false);

            for (int plotNumber = 0; plotNumber < 3; plotNumber++)
            {
                // create random data to plot
                Random rand = new Random(plotNumber);
                int pointCount = 20;
                double[] dataX = new double[pointCount];
                double[] dataY = new double[pointCount];
                double[] errorY = new double[pointCount];
                double[] errorX = new double[pointCount];
                for (int i = 0; i < pointCount; i++)
                {
                    dataX[i] = i + rand.NextDouble();
                    dataY[i] = rand.NextDouble() * 100 + 100 * plotNumber;
                    errorX[i] = rand.NextDouble();
                    errorY[i] = rand.NextDouble() * 10;
                }

                // demonstrate different ways to plot errorbars
                if (plotNumber == 0)
                    plt.PlotScatter(dataX, dataY, lineWidth: 0, errorY: errorY, errorX: errorX,
                        label: $"X and Y errors");
                else if (plotNumber == 1)
                    plt.PlotScatter(dataX, dataY, lineWidth: 0, errorY: errorY,
                        label: $"Y errors only");
                else
                    plt.PlotScatter(dataX, dataY, errorY: errorY, errorX: errorX,
                        label: $"Connected Errors");
            }

            plt.Title("Scatter Plot with Errorbars");
            plt.Legend();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_61_Plot_Bar_Data()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            // create demo data to use for errorbars
            double[] yErr = new double[dataSin.Length];
            for (int i = 0; i < yErr.Length; i++)
                yErr[i] = dataSin[i] / 5 + .025;

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Bar Plot With Error Bars");
            plt.PlotBar(dataXs, dataSin, barWidth: .5, errorY: yErr, errorCapSize: 2);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_62_Plot_Bar_Data_Fancy()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            // generate some more complex data
            Random rand = new Random(0);
            int pointCount = 10;
            double[] Xs = new double[pointCount];
            double[] dataA = new double[pointCount];
            double[] errorA = new double[pointCount];
            double[] dataB = new double[pointCount];
            double[] errorB = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                Xs[i] = i * 10;
                dataA[i] = rand.NextDouble() * 100;
                dataB[i] = rand.NextDouble() * 100;
                errorA[i] = rand.NextDouble() * 10;
                errorB[i] = rand.NextDouble() * 10;
            }

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Multiple Bar Plots");
            plt.Grid(false);
            // customize barWidth and xOffset to squeeze grouped bars together
            plt.PlotBar(Xs, dataA, errorY: errorA, label: "data A", barWidth: 3.2, xOffset: -2);
            plt.PlotBar(Xs, dataB, errorY: errorB, label: "data B", barWidth: 3.2, xOffset: 2);
            plt.Axis(null, null, 0, null);
            plt.Legend();
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_63_Step_Plot()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotStep(dataXs, dataSin);
            plt.PlotStep(dataXs, dataCos);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_64_Manual_Grid_Spacing()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.PlotScatter(dataXs, dataCos);
            plt.Grid(xSpacing: 2, ySpacing: .1);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_65_Histogram()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            Random rand = new Random(0);
            double[] values1 = ScottPlot.DataGen.RandomNormal(rand, pointCount: 1000, mean: 50, stdDev: 20);
            var hist1 = new ScottPlot.Histogram(values1, min: 0, max: 100);

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Histogram");
            plt.YLabel("Count (#)");
            plt.XLabel("Value (units)");
            plt.PlotBar(hist1.bins, hist1.counts, barWidth: 1);
            plt.Axis(null, null, 0, null);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_66_CPH()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            Random rand = new Random(0);
            double[] values1 = ScottPlot.DataGen.RandomNormal(rand, pointCount: 1000, mean: 50, stdDev: 20);
            double[] values2 = ScottPlot.DataGen.RandomNormal(rand, pointCount: 1000, mean: 45, stdDev: 25);
            var hist1 = new ScottPlot.Histogram(values1, min: 0, max: 100);
            var hist2 = new ScottPlot.Histogram(values2, min: 0, max: 100);

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Cumulative Probability Histogram");
            plt.YLabel("Probability (fraction)");
            plt.XLabel("Value (units)");
            plt.PlotStep(hist1.bins, hist1.cumulativeFrac, lineWidth: 1.5, label: "sample A");
            plt.PlotStep(hist2.bins, hist2.cumulativeFrac, lineWidth: 1.5, label: "sample B");
            plt.Legend();
            plt.Axis(null, null, 0, 1);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_67_Candlestick()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            Random rand = new Random(0);
            int pointCount = 60;
            ScottPlot.OHLC[] ohlcs = ScottPlot.DataGen.RandomStockPrices(rand, pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Candlestick Chart");
            plt.YLabel("Stock Price (USD)");
            plt.XLabel("Day (into Q4)");
            plt.PlotCandlestick(ohlcs);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_68_OHLC()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            Random rand = new Random(0);
            int pointCount = 60;
            ScottPlot.OHLC[] ohlcs = ScottPlot.DataGen.RandomStockPrices(rand, pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Open/High/Low/Close (OHLC) Chart");
            plt.YLabel("Stock Price (USD)");
            plt.XLabel("Day (into Q4)");
            plt.PlotOHLC(ohlcs);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_70_Save_Scatter_Data()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotScatter(dataXs, dataSin);
            plt.GetPlottables()[0].SaveCSV("scatter.csv");
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_71_Save_Signal_Data()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.PlotSignal(dataCos, sampleRate: 20_000);
            plt.GetPlottables()[0].SaveCSV("signal.csv");
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }

        public string Figure_72_Custom_Fonts()
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name.Replace("Figure_", "");
            string fileName = System.IO.Path.GetFullPath($"{outputFolderName}/{name}.png");

            int pointCount = 50;
            double[] dataXs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] dataSin = ScottPlot.DataGen.Sin(pointCount);
            double[] dataCos = ScottPlot.DataGen.Cos(pointCount);

            var plt = new ScottPlot.Plot(width, height);
            plt.Title("Impressive Graph", fontName: "courier new", fontSize: 24, color: Color.Purple, bold: true);
            plt.YLabel("vertical units", fontName: "impact", fontSize: 24, color: Color.Red, bold: true);
            plt.XLabel("horizontal units", fontName: "georgia", fontSize: 24, color: Color.Blue, bold: true);
            plt.PlotScatter(dataXs, dataSin, label: "sin");
            plt.PlotScatter(dataXs, dataCos, label: "cos");
            plt.PlotText("very graph", 25, .8, fontName: "comic sans ms", fontSize: 24, color: Color.Blue, bold: true);
            plt.PlotText("so data", 0, 0, fontName: "comic sans ms", fontSize: 42, color: Color.Magenta, bold: true);
            plt.PlotText("many documentation", 3, -.6, fontName: "comic sans ms", fontSize: 18, color: Color.DarkCyan, bold: true);
            plt.PlotText("wow.", 10, .6, fontName: "comic sans ms", fontSize: 36, color: Color.Green, bold: true);
            plt.PlotText("NuGet", 32, 0, fontName: "comic sans ms", fontSize: 24, color: Color.Gold, bold: true);
            plt.Legend(fontName: "comic sans ms", fontSize: 16, bold: true, fontColor: Color.DarkBlue);
            plt.SaveFig(fileName);
            Console.WriteLine($"Saved: {System.IO.Path.GetFileName(fileName)}");
            return name + ":" + ScottPlot.Tools.BitmapHash(plt.GetBitmap());
        }
    }
}
