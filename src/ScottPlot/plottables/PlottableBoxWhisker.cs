using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
	public class PlottableBoxWhisker : Plottable
	{
		public double[] xs;
		public double[][] ys;
		public double lineWidth;
		private double[] yLimits;
		public double boxWidth;
		private double padding = 50;
		Pen pen;
		Brush brush;

		public PlottableBoxWhisker(double[] xs, double[][] ys, Color color, string label, double lineWidth, double boxWidth)
		{
			if (xs == null || ys == null)
				throw new Exception("Y data cannot be null");

			if (xs.Length != ys.Length)
			{
				throw new ArgumentException("X positions must be the same length as Y values");
			}

			yLimits = new double[2];
			yLimits[0] = ys[0][0];
			yLimits[1] = ys[0][0];

			for (int i = 0; i < ys.Length; i++)
			{
				for (int j = 0; j < ys[i].Length; j++)
				{
					if (ys[i][j] < yLimits[0])
					{
						yLimits[0] = ys[i][j];
					}
					else if (ys[i][j] > yLimits[1])
					{
						yLimits[1] = ys[i][j];
					}
				}
			}

			this.xs = xs;
			this.ys = ys;
			this.color = color;
			this.label = label;
			this.lineWidth = lineWidth;
			this.boxWidth = boxWidth;

			pen = new Pen(color, (float)lineWidth);
			brush = new SolidBrush(color);
		}

		public override Config.AxisLimits2D GetLimits()
		{
			double[] limits = new double[4];
			limits[0] = xs.Min();
			limits[1] = xs.Max();
			limits[2] = yLimits[0];
			limits[3] = yLimits[1];

			// TODO: use features of 2d axis
			return new Config.AxisLimits2D(limits);
		}

		public override void Render(Settings settings)
		{
			for (int i = 0; i < xs.Length; i++)
			{
				int QSize = (int)Math.Floor(ys[i].Length / 4.0);
				double[] sortedYs = ys[i].OrderBy(y => y).ToArray();
				double Q1 = sortedYs[QSize];
				double Q3 = sortedYs[sortedYs.Length - QSize - 1];

				double leftSide = ((xs[i] - boxWidth / (2 * settings.xAxisScale)) * settings.xAxisScale);
				settings.gfxData.DrawRectangle(pen, (int)leftSide, (int)(Q1 * settings.yAxisScale), (int)boxWidth, (int)((Q3 - Q1) * settings.yAxisScale));
			}

			//	throw new NotImplementedException();
		}

		public override string ToString()
		{
			return $"PlottableBoxWhisker with {xs.Length} boxes";
		}
	}
}
