using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
	public class CoxcombPlot : IPlottable
	{
		private double[] _values;
		public double[] Values
		{
			get => _values;
			set {
				_values = value;
				Normalized = Normalize(value);
			}
		}
		public Color[] FillColors { get; set; }
		public Color WebColor { get; set; }

		/// <summary>
		/// Controls rendering style of the concentric circles (ticks) of the web
		/// </summary>
		public RadarAxis AxisType { get; set; } = RadarAxis.Circle;
		public bool IsVisible { get; set; } = true;
		public int XAxisIndex { get; set; } = 0;
		public int YAxisIndex { get; set; } = 0;

		private double[] Normalized;

		public CoxcombPlot(double[] values, Color[] fillColors)
		{
			Values = values;
			FillColors = fillColors;
		}

		public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
		{
			int numCategories = Normalized.Length;
			PointF origin = new PointF(dims.GetPixelX(0), dims.GetPixelY(0));
			double sweepAngle = 360f / numCategories;
			double maxDiameterPixels = .9 * Math.Min(dims.DataWidth, dims.DataHeight);

			using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
			using SolidBrush sliceFillBrush = (SolidBrush)GDI.Brush(Color.Black);


			RenderAxis(gfx, dims, bmp, lowQuality);


			double start = -90;
			for (int i = 0; i < numCategories; i++)
			{
				double angle = (Math.PI / 180) * ((sweepAngle + 2 * start) / 2);
				float diameter = (float)(maxDiameterPixels * Normalized[i]);
				sliceFillBrush.Color = FillColors[i];

				gfx.FillPie(brush: sliceFillBrush,
					x: (int)origin.X - diameter / 2,
					y: (int)origin.Y - diameter / 2,
					width: diameter,
					height: diameter,
					startAngle: (float)start,
					sweepAngle: (float)sweepAngle);

				start += sweepAngle;
			}
		}

		private void RenderAxis(Graphics gfx, PlotDimensions dims, Bitmap bmp, bool lowQuality)
		{
			double[,] Norm = new double[Normalized.Length, 1];
			for(int i = 0; i < Normalized.Length; i++)
			{
				Norm[i, 0] = Normalized[i];
			}

			StarAxis axis = new()
			{
				Norm = Norm,
				NormMax = Normalized.Max(),
				NormMaxes = null,
				CategoryLabels = null,
				AxisType = AxisType,
				WebColor = WebColor,
				IndependentAxes = false,
				ShowAxisValues = true,
				Graphics = gfx
			};

			axis.Render(dims, bmp, lowQuality);
		}

		private static double[] Normalize(double[] values)
		{
			double Max = values.Max();

			if(Max == 0)
			{
				return values.Select(_ => 0.0).ToArray();
			}

			return values.Select(v => v / Max).ToArray();
		}

		public LegendItem[] GetLegendItems()
		{
			return null; // TODO: Do once we have labels
		}
		
		public AxisLimits GetAxisLimits() => new AxisLimits(-0.5, 0.5, -1, 1);

		public void ValidateData(bool deep = false)
		{
			// TODO
		}
	}
}
