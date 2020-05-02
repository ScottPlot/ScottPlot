using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ScottPlot.Config;

namespace ScottPlot
{
	public class PlottableMultiBar : Plottable
	{
		public double[][] xs;
		public double[][] ys;
		public double[][] yErr;

		public LineStyle lineStyle;
		public Color[] fillColors;
		public string label;

		private double errorCapSize;

		private double barWidth;
		private double valueBase = 0;

		public bool fill;
		private Brush[] fillBrushes;
		private Pen errorPen;
		private Pen outlinePen;

		public bool verticalBars;
		public bool showValues;

		Font valueTextFont;
		Brush valueTextBrush;

		public PlottableMultiBar(double[][] xs, double[][] ys, string label,
			double barWidth,
			bool fill, Color[] fillColors,
			double outlineWidth, Color outlineColor,
			double[][] yErr, double errorLineWidth, double errorCapSize, Color errorColor,
			bool horizontal, bool showValues
			)
		{
			if (ys is null || ys.Length == 0)
				throw new ArgumentException("ys must contain data values");

			if (xs is null)
			{
				xs = new double[ys.Length][];
				for (int i = 0; i < xs.Length; i++)
				{
					xs[i] = DataGen.Consecutive(ys[i].Length);
				}
			}

			if (xs.Length != ys.Length)
				throw new ArgumentException("xs and ys must have same number of elements");

			if (yErr is null)
			{
				yErr = new double[ys.Length][];
				for (int i = 0; i < yErr.Length; i++)
				{
					yErr[i] = DataGen.Zeros(ys[i].Length);
				}
			}

			if (yErr.Length != ys.Length)
				throw new ArgumentException("yErr and ys must have same number of elements");

			this.xs = xs;
			this.ys = ys;
			this.yErr = yErr;
			this.label = label;
			this.verticalBars = !horizontal;
			this.showValues = showValues;

			this.barWidth = barWidth / xs.Length;
			this.errorCapSize = errorCapSize;

			this.fill = fill;
			this.fillColors = fillColors;

			fillBrushes = fillColors.Select(c => new SolidBrush(c)).ToArray();

			outlinePen = new Pen(outlineColor, (float)outlineWidth);
			errorPen = new Pen(errorColor, (float)errorLineWidth);

			valueTextFont = new Font(Fonts.GetDefaultFontName(), 12);
			valueTextBrush = new SolidBrush(Color.Black);
		}

		public override AxisLimits2D GetLimits()
		{
			double valueMin = double.PositiveInfinity;
			double valueMax = double.NegativeInfinity;
			double positionMin = double.PositiveInfinity;
			double positionMax = double.NegativeInfinity;

			for (int i = 0; i < xs.Length; i++)
			{
				for (int j = 0; j < xs[i].Length; j++)
				{
					valueMin = Math.Min(valueMin, ys[i][j] - yErr[i][j]);
					valueMax = Math.Max(valueMax, ys[i][j] + yErr[i][j]);
					positionMin = Math.Min(positionMin, xs[i][j]);
					positionMax = Math.Max(positionMax, xs[i][j]);
				}
			}

			valueMin = Math.Min(valueMin, valueBase);
			valueMax = Math.Max(valueMax, valueBase);

			if (showValues)
				valueMax += (valueMax - valueMin) * .1; // increase by 10% to accomodate label

			positionMin -= barWidth / 2;
			positionMax += barWidth / 2;

			if (verticalBars)
				return new AxisLimits2D(positionMin, positionMax, valueMin, valueMax);
			else
				return new AxisLimits2D(valueMin, valueMax, positionMin, positionMax);
		}

		public override void Render(Settings settings)
		{
			for (int i = 0; i < xs.Length; i++)
			{
				for (int j = 0; j < xs[i].Length; j++)
				{
					if (verticalBars)
						RenderBarVertical(settings, xs[i][j], ys[i][j], yErr[i][j], fillBrushes[i], i, xs.Length);
					else
						RenderBarHorizontal(settings, xs[i][j], ys[i][j], yErr[i][j], fillBrushes[i], i, xs.Length);
				}
			}
		}

		private void RenderBarVertical(Settings settings, double position, double value, double valueError, Brush fillBrush, int index, int count)
		{
			double marginFactor = 0.1;
			double margin = marginFactor * barWidth;

			float centerPx = (float)settings.GetPixelX(position);
			double edge1 = position - (count * barWidth / 2) + index * (barWidth + margin);
			double value1 = Math.Min(valueBase, value);
			double value2 = Math.Max(valueBase, value);
			double valueSpan = value2 - value1;

			var rect = new RectangleF(
				x: (float)settings.GetPixelX(edge1),
				y: (float)settings.GetPixelY(value2),
				width: (float)(barWidth * settings.xAxisScale * (1 - marginFactor)),
				height: (float)(valueSpan * settings.yAxisScale));

			settings.gfxData.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);

			if (outlinePen.Width > 0)
				settings.gfxData.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);

			if (errorPen.Width > 0 && valueError > 0)
			{
				double error1 = value2 - Math.Abs(valueError);
				double error2 = value2 + Math.Abs(valueError);

				float capPx1 = (float)settings.GetPixelX(position - errorCapSize * barWidth / 2 - (count * barWidth / 3) + index * (barWidth + margin));
				float capPx2 = (float)settings.GetPixelX(position + errorCapSize * barWidth / 2 - (count * barWidth / 3) + index * (barWidth + margin));
				float thisCenterPx = (capPx1 + capPx2) / 2;
				float errorPx2 = (float)settings.GetPixelY(error2);
				float errorPx1 = (float)settings.GetPixelY(error1);

				settings.gfxData.DrawLine(errorPen, thisCenterPx, errorPx1, thisCenterPx, errorPx2);
				settings.gfxData.DrawLine(errorPen, capPx1, errorPx1, capPx2, errorPx1);
				settings.gfxData.DrawLine(errorPen, capPx1, errorPx2, capPx2, errorPx2);
			}

			if (showValues)
				settings.gfxData.DrawString(value.ToString(), valueTextFont, valueTextBrush, centerPx, rect.Y, settings.misc.sfSouth);
		}

		private void RenderBarHorizontal(Settings settings, double position, double value, double valueError, Brush fillBrush, int index, int count)
		{
			double marginFactor = 0.1;
			double margin = marginFactor * barWidth;

			float centerPx = (float)settings.GetPixelY(position);
			double edge2 = position + (count * barWidth / 2) + index * (barWidth + margin);
			double value1 = Math.Min(valueBase, value);
			double value2 = Math.Max(valueBase, value);
			double valueSpan = value2 - value1;

			var rect = new RectangleF(
				x: (float)settings.GetPixelX(valueBase),
				y: (float)settings.GetPixelY(edge2),
				height: (float)(barWidth * settings.yAxisScale * (1 - marginFactor)),
				width: (float)(valueSpan * settings.xAxisScale));

			settings.gfxData.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);

			if (outlinePen.Width > 0)
				settings.gfxData.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);

			if (errorPen.Width > 0 && valueError > 0)
			{
				double error1 = value2 - Math.Abs(valueError);
				double error2 = value2 + Math.Abs(valueError);

				float thisCenter = (float)(edge2 - barWidth / 2);
				float capPx1 = (float)settings.GetPixelY(thisCenter + 0.5 * errorCapSize);
				float capPx2 = (float)settings.GetPixelY(thisCenter - 0.5 * errorCapSize);
				float errorPx2 = (float)settings.GetPixelX(error2);
				float errorPx1 = (float)settings.GetPixelX(error1);

				float thisCenterPx = (float)settings.GetPixelY(thisCenter);

				settings.gfxData.DrawLine(errorPen, errorPx1, thisCenterPx, errorPx2, thisCenterPx);
				settings.gfxData.DrawLine(errorPen, errorPx1, capPx2, errorPx1, capPx1);
				settings.gfxData.DrawLine(errorPen, errorPx2, capPx2, errorPx2, capPx1);
			}

			if (showValues)
				settings.gfxData.DrawString(value.ToString(), valueTextFont, valueTextBrush, rect.X + rect.Width, centerPx, settings.misc.sfWest);
		}

		public override string ToString()
		{
			return $"PlottableBar with {GetPointCount()} points";
		}

		public override int GetPointCount()
		{
			return ys.Length;
		}

		public override LegendItem[] GetLegendItems()
		{
			var singleLegendItem = new LegendItem(label, fillColors[0], lineWidth: 10, markerShape: MarkerShape.none);
			return new LegendItem[] { singleLegendItem };
		}
	}
}
