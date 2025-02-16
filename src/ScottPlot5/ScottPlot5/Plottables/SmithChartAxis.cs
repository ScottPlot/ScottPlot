namespace ScottPlot.Plottables;

/// <summary>
/// A polar axes uses spoke lines and circles to describe a polar coordinate system
/// where points are represented by a radius and angle. 
/// This class draws a polar axes and has options to customize spokes and circles.
/// </summary>
public class SmithChartAxis : IPlottable, IManagesAxisLimits
{
    public static Coordinates CalculateGamma(Coordinates normalizedImpedance)
    {
        // Γ = (z - 1) / (z + 1)
        //   = (R + jX - 1) / (R + jX + 1)
        //   = a + jc 

        double R = normalizedImpedance.X;
        double X = normalizedImpedance.Y;

        // Calculate the denominator (R + jX + 1)
        double denominatorReal = R + 1;
        double denominatorImaginary = X;

        // Magnitude squared of the denominator
        double denominatorMagnitudeSquared =
            Math.Pow(denominatorReal, 2) + Math.Pow(denominatorImaginary, 2);

        // Calculate the real and imaginary parts of Γ
        double a = ((R - 1) * denominatorReal + X * denominatorImaginary) / denominatorMagnitudeSquared;
        double c = (X * denominatorReal - (R - 1) * denominatorImaginary) / denominatorMagnitudeSquared;

        return new(a, c);
    }

    public sealed class RealTick :
        PolarAxisCircle
    {
        public double Re { get; set; }

        public RealTick(double Re) :
            base(Math.Abs(1 / (1 + Re)))
        {
            this.Re = Re;

            double p = 1 / (1 + Re);
            Origin = new Coordinates(1 - p, 0);
        }

        public AxisLimits GetAxisLimits()
        {
            return new(Origin.X - Radius, Origin.X + Radius,
                       Origin.Y - Radius, Origin.Y + Radius);
        }
    }

    public sealed class ImaginaryTick :
        PolarAxisCircle
    {
        public double Lm { get; set; }

        public IReadOnlyList<Coordinates> Points { get; }

        public ImaginaryTick(
            double Lm, Coordinates center = default, double radius = 1) :
            base(Math.Abs(1 / Lm))
        {
            this.Lm = Lm;
            Origin = new(1, 1 / Lm);

            Points = FindIntersectionPoints(center, radius, Origin, Radius).ToList();
            if (Points.Count >= 2)
            {
                Angle ang1 = GetAngle(Points[0], Origin);
                Angle ang2 = GetAngle(Points[1], Origin);

                StartAngle = ang2;
                SweepAngle = ang1 - ang2;
            }
        }

        /// <summary>
        /// Calculate the angle of the target point relative to the center of the circle.
        /// </summary>
        /// <param name="point">Target point.</param>
        /// <param name="center">The coordinates of the center of the circle.</param>
        public static Angle GetAngle(Coordinates point, Coordinates center)
        {
            // Make sure the angle range is between 0 and 360.
            return Angle
                .FromRadians(Math.Atan2(point.Y - center.Y, point.X - center.X))
                .Normalized;
        }

        /// <summary>
        /// Calculates a point on a specified angle base.
        /// </summary>
        /// <param name="center">The coordinates of the center of the circle.</param>
        /// <param name="radius">radius of circle.</param>
        /// <param name="angle">Angle of target point.</param>
        /// <returns>Point coordinates at a specified angle on the circle.</returns>
        public static Coordinates GetPointOnCircle(
            Coordinates center, double radius, Angle angle)
        {
            return new(
                center.X + radius * Math.Cos(angle.Radians),
                center.Y + radius * Math.Sin(angle.Radians));
        }

        public AxisLimits GetAxisLimits()
        {
            if (Lm == 0)
            {
                return AxisLimits.NoLimits;
            }

            if (SweepAngle.Normalized.Degrees == 0)
            {
                return new(Origin.X - Radius, Origin.X + Radius,
                               Origin.Y - Radius, Origin.Y + Radius);
            }

            Coordinates pt1 = GetPointOnCircle(Origin, Radius, StartAngle);
            Coordinates pt2 = GetPointOnCircle(Origin, Radius, StartAngle + SweepAngle);
            return new(Math.Min(pt1.X, pt2.X), Math.Max(pt1.X, pt2.X),
                       Math.Min(pt1.Y, pt2.Y), Math.Max(pt1.Y, pt2.Y));
        }
    }

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    /// <summary>
    /// Concentric circular tick lines
    /// </summary>
    public List<RealTick> RealTicks { get; } = [];

    /// <summary>
    /// Curves extending from the right side of the outer circle to various points around its circumference
    /// </summary>
    public List<ImaginaryTick> ImaginaryTicks { get; } = [];

    /// <summary>
    /// Rotates the axis clockwise from its default position (where 0 points right)
    /// </summary>
    public Angle Rotation { get; set; } = Angle.FromDegrees(0);

    /// <summary>
    /// Enable this to modify the axis limits at render time to achieve "square axes"
    /// where the units/px values are equal for horizontal and vertical axes, allowing
    /// circles to always appear as circles instead of ellipses.
    /// </summary>
    public bool ManageAxisLimits { get; set; } = true;

    /// <summary>
    /// Default style of the curved lines extending from the right edge 
    /// to points around the circumference of the chart outline
    /// </summary>
    public LineStyle RealLineStyle { get; set; } = new()
    {
        Width = 1,
        Color = Colors.Black.WithAlpha(.2),
    };

    /// <summary>
    /// Default style of the concentric circular axis lines
    /// </summary>
    public LineStyle Imaginary { get; set; } = new()
    {
        Width = 1,
        Color = Colors.Black.WithAlpha(.2),
    };

    public LabelStyle LabelStyle { get; } = new();
    public string? LabelText { get; set; } = null;

    /// <summary>
    /// Distance to offset label text
    /// </summary>
    public double LabelPaddingFraction { get; set; } = 0.1;

    public static IEnumerable<Coordinates> FindIntersectionPoints(
        Coordinates pt1, double r1, Coordinates pt2, double r2)
    {
        // Calculate distance between circle centers
        double dx = pt2.X - pt1.X;
        double dy = pt2.Y - pt1.Y;
        double d = Math.Sqrt(dx * dx + dy * dy);

        // If the distance exceeds the sum of the radii of the two circles,
        // or is less than the difference in radii,
        // an exception is thrown (should not happen)
        if (d >= r1 + r2 || d <= Math.Abs(r1 - r2))
        {
            yield break;
        }

        // Calculate the base point of the intersection line
        double a = (r1 * r1 - r2 * r2 + d * d) / (2 * d);
        double h = Math.Sqrt(r1 * r1 - a * a);

        // datum vector between circle centers
        double cx = pt1.X + a * dx / d;
        double cy = pt1.Y + a * dy / d;

        // Calculate intersection point
        double offsetX = h * dy / d;
        double offsetY = h * dx / d;

        yield return new(cx + offsetX, cy - offsetY);
        yield return new(cx - offsetX, cy + offsetY);
    }

    public RealTick AddRealTick(double value)
    {
        RealTick part = new(value)
        {
            LineStyle = RealLineStyle.Clone(),
        };
        RealTicks.Add(part);
        return part;
    }

    public ImaginaryTick AddImaginaryTick(double value)
    {
        ImaginaryTick part = new(value)
        {
            LineStyle = RealLineStyle.Clone(),
        };
        ImaginaryTicks.Add(part);
        return part;
    }

    /// <summary>
    /// Return the X/Y position of a point defined in polar space
    /// </summary>
    public Coordinates GetCoordinates(double radius, Angle angle)
    {
        return GetCoordinates(new PolarCoordinates(radius, angle));
    }

    /// <summary>
    /// Return the X/Y position of a point defined in polar space
    /// </summary>
    public Coordinates GetCoordinates(PolarCoordinates point)
    {
        return point.WithAngle(point.Angle + Rotation).ToCartesian();
    }

    /// <summary>
    /// Return the X/Y position of the given impedance
    /// </summary>
    public Coordinates GetCoordinates(double resistance, double reactance)
    {
        Coordinates normalizedImpedance = new(resistance, reactance);
        return CalculateGamma(normalizedImpedance);
    }

    public AxisLimits GetAxisLimits()
    {
        AxisLimits limits = new(-1, 1, -1, 1);
        RealTicks.ForEach(i => limits = limits.Expanded(i.GetAxisLimits()));
        ImaginaryTicks.ForEach(i => limits = limits.Expanded(i.GetAxisLimits()));
        return limits;
    }

    public virtual void UpdateAxisLimits(Plot plot)
    {
        foreach (IAxisRule rule in plot.Axes.Rules)
        {
            if (rule is AxisRules.SquareZoomOut)
                return;
        }

        AxisRules.SquareZoomOut squareRule = new(Axes.XAxis, Axes.YAxis);
        plot.Axes.Rules.Add(squareRule);
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        RenderConstantRealCircles(rp, paint);
        RenderConstantImaginaryCurves(rp, paint);
        RenderCircleLabels(rp, paint);
    }

    private void RenderCircleLabels(RenderPack rp, SKPaint paint)
    {
        foreach (PolarAxisCircle circle in ImaginaryTicks)
        {
            Coordinates c = GetCoordinates(radius: circle.Radius, angle: circle.LabelAngle);
            Pixel px = Axes.GetPixel(c);
            circle.LabelStyle.Render(rp.Canvas, px, paint, circle.LabelText);
        }
    }

    private void RenderConstantRealCircles(RenderPack rp, SKPaint paint)
    {
        double pxPerUnit = rp.DataRect.Width / Axes.XAxis.Width;
        LabelStyle.Rotation = (float)Rotation.Degrees - 90;
        LabelStyle.Alignment = Alignment.LowerLeft;

        foreach (RealTick part in RealTicks)
        {
            float radiusPx = (float)(pxPerUnit * part.Radius);
            Pixel originPx = Axes.GetPixel(part.Origin);
            Drawing.DrawCircle(rp.Canvas, originPx, radiusPx, part.LineStyle, paint);

            if (part.Re > 0)
            {
                LabelStyle.Text = $"{part.Re}";
                Coordinates labelPoint = new PolarCoordinates
                    (part.Radius * (1 + LabelPaddingFraction / 2), Angle.FromDegrees(180))
                    .ToCartesian()
                    .WithDelta(part.Origin.X, part.Origin.Y);
                Pixel labelPixel = Axes.GetPixel(labelPoint).WithOffset(0, -3);
                LabelStyle.Render(rp.Canvas, labelPixel, paint);
            }
        }
    }

    private void RenderConstantImaginaryCurves(RenderPack rp, SKPaint paint)
    {
        double pxPerUnit = rp.DataRect.Width / Axes.XAxis.Width;
        LabelStyle.Rotation = (float)Rotation.Degrees;
        LabelStyle.Alignment = Alignment.MiddleCenter;

        for (int i = 0; i < ImaginaryTicks.Count; i++)
        {
            float radiusPx = (float)(pxPerUnit * ImaginaryTicks[i].Radius);
            Pixel originPx = Axes.GetPixel(ImaginaryTicks[i].Origin);
            if (ImaginaryTicks[i].Lm == 0)
            {
                Drawing.DrawLine(rp.Canvas, paint,
                    Axes.GetPixel(ImaginaryTick.GetPointOnCircle(new(), 1, Angle.FromDegrees(180))),
                    Axes.GetPixel(ImaginaryTick.GetPointOnCircle(new(), 1, Angle.FromDegrees(0))),
                    ImaginaryTicks[i].LineStyle);

                PolarCoordinates labelPoint =
                    new(1 * (1 + LabelPaddingFraction), Angle.FromDegrees(0));

                LabelStyle.Text = "∞";
                LabelStyle.Render(rp.Canvas, Axes.GetPixel(labelPoint.ToCartesian()), paint);

                LabelStyle.Text = "0";
                labelPoint = labelPoint.WithAngleDegrees(180);
                LabelStyle.Render(rp.Canvas, Axes.GetPixel(labelPoint.ToCartesian()), paint);
            }
            else if (ImaginaryTicks[i].SweepAngle.Degrees == 360)
            {
                Drawing.DrawCircle(rp.Canvas, originPx, radiusPx, ImaginaryTicks[i].LineStyle, paint);
            }
            else
            {
                PixelRect rect = new(
                    originPx.X - radiusPx,
                    originPx.X + radiusPx,
                    originPx.Y + radiusPx,
                    originPx.Y - radiusPx);
                Drawing.DrawArc(rp.Canvas, paint, ImaginaryTicks[i].LineStyle, rect,
                    (float)-ImaginaryTicks[i].StartAngle.Degrees,
                    (float)-ImaginaryTicks[i].SweepAngle.Degrees);

                Coordinates point = (ImaginaryTicks[i].Lm < 0)
                    ? ImaginaryTicks[i].Points[0]
                    : ImaginaryTicks[i].Points[1];

                StringBuilder sb = new();
                sb.Append((ImaginaryTicks[i].Lm < 0) ? '-' : '+');
                sb.Append('j');
                sb.Append(Math.Abs(ImaginaryTicks[i].Lm));
                LabelStyle.Text = sb.ToString();

                Angle angle = ImaginaryTick.GetAngle(point, new(0, 0));
                PolarCoordinates labelPoint =
                    new(1 * (1 + LabelPaddingFraction), angle);
                Pixel labelPixel = Axes.GetPixel(labelPoint.ToCartesian());
                LabelStyle.Render(rp.Canvas, labelPixel, paint);
            }
        }
    }
}
