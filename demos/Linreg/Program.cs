using System;
using System.Diagnostics;

namespace Linreg
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			double[] dataX = new double[] { 1, 2, 3, 4, 5 };
			double[] dataY = new double[] { 1, 4, 9, 16, 25 };
			var plt = new ScottPlot.Plot(600, 400);

			var linreg = new ScottPlot.LinearRegressionLine(dataX, dataY);
			plt=linreg.Draw(plt);
			plt = linreg.DrawResidual(plt);

			plt.PlotScatter(dataX, dataY, lineWidth: 0);
			string filename = "qucikstart.png";
			plt.SaveFig(filename);

			using (Process process = new Process()) {//So the photo opens automatically (you need to use xdg-open on Linux)
				process.StartInfo.FileName=filename;
				process.StartInfo.UseShellExecute = true;
				process.Start();
			}
		}
	}
}
