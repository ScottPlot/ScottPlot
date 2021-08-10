using ScottPlot.Drawing;
using ScottPlot.Statistics;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace ScottPlot.Plottable
{
	 /// <summary>
	 /// The VectorField displays arrows representing a 2D array of 2D vectors
	 /// </summary>
	 public class VectorField : IPlottable
	 {
		  // data
		  private readonly double[] Xs;
		  private readonly double[] Ys;
		  private readonly bool fancyCaps;
		  private readonly bool anchorAtStart;
		  private readonly Vector2[,] Vectors;
		  private readonly Color[] VectorColors;
		  private readonly PointF[] _poly = new PointF[5];

		  // customization
		  public string Label;
		  public bool IsVisible { get; set; } = true;
		  public int XAxisIndex { get; set; } = 0;
		  public int YAxisIndex { get; set; } = 0;

		  /// <summary>
		  /// 
		  /// </summary>
		  /// <param name="vectors"></param>
		  /// <param name="xs"></param>
		  /// <param name="ys"></param>
		  /// <param name="colormap"></param>
		  /// <param name="scaleFactor"></param>
		  /// <param name="defaultColor"></param>
		  /// <param name="fancyCaps">Plot custom arrow heads; slower but nicer looking.</param>
		  /// <param name="anchorAtStart">Draw arrow starting from the x,y coordinate, instead of centering the arrow on the x,y coordinate.</param>
		  public VectorField (Vector2[,] vectors, double[] xs, double[] ys, Colormap colormap, double scaleFactor, Color defaultColor,
				bool fancyCaps = false, bool anchorAtStart = false)
		  {
				double minMagnitudeSquared = vectors[0, 0].LengthSquared();
				double maxMagnitudeSquared = vectors[0, 0].LengthSquared();
				for (int i = 0; i < xs.Length; i++)
				{
					 for (int j = 0; j < ys.Length; j++)
					 {
						  if (vectors[i, j].LengthSquared() > maxMagnitudeSquared)
								maxMagnitudeSquared = vectors[i, j].LengthSquared();
						  else if (vectors[i, j].LengthSquared() < minMagnitudeSquared)
								minMagnitudeSquared = vectors[i, j].LengthSquared();
					 }
				}
				double minMagnitude = Math.Sqrt(minMagnitudeSquared);
				double maxMagnitude = Math.Sqrt(maxMagnitudeSquared);

				double[,] intensities = new double[xs.Length, ys.Length];
				for (int i = 0; i < xs.Length; i++)
				{
					 for (int j = 0; j < ys.Length; j++)
					 {
						  if (colormap != null)
								intensities[i, j] = (vectors[i, j].Length() - minMagnitude) / (maxMagnitude - minMagnitude);
						  vectors[i, j] = Vector2.Multiply(vectors[i, j], (float)(scaleFactor / (maxMagnitude * 1.2)));
					 }
				}

				double[] flattenedIntensities = intensities.Cast<double>().ToArray();
				VectorColors = colormap is null ?
					 Enumerable.Range(0, flattenedIntensities.Length).Select(x => defaultColor).ToArray() :
					 Colormap.GetColors(flattenedIntensities, colormap);

				this.Vectors = vectors;
				this.Xs = xs;
				this.Ys = ys;
				this.fancyCaps = fancyCaps;
				this.anchorAtStart = anchorAtStart;
		  }

		  public void ValidateData (bool deep = false) { /* validation occurs in constructor */ }

		  public LegendItem[] GetLegendItems ()
		  {
				var singleLegendItem = new LegendItem()
				{
					 label = Label,
					 color = VectorColors[0],
					 lineWidth = 10,
					 markerShape = MarkerShape.none
				};
				return new LegendItem[] { singleLegendItem };
		  }

		  public AxisLimits GetAxisLimits () => new AxisLimits(Xs.Min() - 1, Xs.Max() + 1, Ys.Min() - 1, Ys.Max() + 1);

		  public int PointCount { get => Vectors.Length; }

		  public void Render (PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
		  {
				using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
				using (Pen pen = GDI.Pen(Color.Black))
				{
					 float tipScale = 0;
					 float headAngle = 0;
					 if (!fancyCaps)
					 {
						  pen.CustomEndCap = new AdjustableArrowCap(2, 2);
					 }
					 else
					 {
						  // The width of the arrow head part, relative to the length of the arrow.
						  var arrowheadWidth = 0.15f;
						  // The length of the arrow head part, relative to the total length of the arrow.
						  var arrowheadLength = 0.5f;

						  tipScale = (float)Math.Sqrt(arrowheadLength * arrowheadLength + arrowheadWidth * arrowheadWidth);
						  headAngle = (float)Math.Atan2(arrowheadWidth, arrowheadLength);
					 }

					 for (int i = 0; i < Xs.Length; i++)
					 {
						  for (int j = 0; j < Ys.Length; j++)
						  {
								Vector2 v = Vectors[i, j];
								float tailX, tailY, endX, endY;
								if (anchorAtStart)
								{
									 tailX = dims.GetPixelX(Xs[i]);
									 tailY = dims.GetPixelY(Ys[j]);
									 endX = dims.GetPixelX(Xs[i] + v.X);
									 endY = dims.GetPixelY(Ys[j] + v.Y);
								}
								else
								{
									 tailX = dims.GetPixelX(Xs[i] - v.X / 2);
									 tailY = dims.GetPixelY(Ys[j] - v.Y / 2);
									 endX = dims.GetPixelX(Xs[i] + v.X / 2);
									 endY = dims.GetPixelY(Ys[j] + v.Y / 2);
								}
								pen.Color = VectorColors[i * Ys.Length + j];
								if (fancyCaps)
									 DrawFancyArrow(gfx, pen, tailX, tailY, endX, endY, headAngle, tipScale);
								else
									 gfx.DrawLine(pen, tailX, tailY, endX, endY);
						  }
					 }
				}
		  }

          /// <summary>
          /// Draw arrow with tips without using CustomEndCap.
          /// </summary>
          /// <param name="gfx"></param>
          /// <param name="pen"></param>
          /// <param name="baseX"></param>
          /// <param name="baseY"></param>
          /// <param name="tipX"></param>
          /// <param name="tipY"></param>
          /// <param name="headAngle">Determines how 'pointy' the tip is.</param>
          /// <param name="tipScale">Length of tip part, relative to total length.</param>
          private void DrawFancyArrow (
			 Graphics gfx, Pen pen,
			 float baseX,
			 float baseY,
			 float tipX,
			 float tipY,
			 float headAngle,
			 float tipScale)
		  {
				var dx = tipX - baseX;
				var dy = tipY - baseY;
				var arrowAngle = (float)Math.Atan2(dy, dx);
				var sinA1 = (float)Math.Sin(headAngle - arrowAngle);
				var cosA1 = (float)Math.Cos(headAngle - arrowAngle);
				var sinA2 = (float)Math.Sin(headAngle + arrowAngle);
				var cosA2 = (float)Math.Cos(headAngle + arrowAngle);
				var len = (float)Math.Sqrt(dx * dx + dy * dy);
				var hypLen = len * tipScale;

				var corner1X = tipX - hypLen * cosA1;
				var corner1Y = tipY + hypLen * sinA1;
				var corner2X = tipX - hypLen * cosA2;
				var corner2Y = tipY - hypLen * sinA2;

				_poly[0] = new PointF(baseX, baseY);
				_poly[1] = new PointF(tipX, tipY);
				_poly[2] = new PointF(corner1X, corner1Y);
				_poly[3] = new PointF(tipX, tipY);
				_poly[4] = new PointF(corner2X, corner2Y);
				gfx.DrawLines(pen, _poly);
		  }



		  public override string ToString ()
		  {
				string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
				return $"PlottableVectorField{label} with {PointCount} vectors";
		  }
	 }
}
