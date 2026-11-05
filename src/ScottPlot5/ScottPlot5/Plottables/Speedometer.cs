namespace ScottPlot.Plottables;

/// <summary>
/// A speedometer shows a single value with a needle on a semicircular band of colored sections from
/// <see cref="Min"/> (left) to <see cref="Max"/> (right), numbers around the band, and a bar beneath it
/// showing the value and <see cref="Label"/> on the color of the section the needle points into.
/// The sections and the bar have thin black edges.
/// It is sized in pixels to fill the data area (or the <see cref="FractionRect"/> portion of it) and centered in it.
/// </summary>
public class Speedometer : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    /// <summary>
    /// Value at the left end of the scale
    /// </summary>
    public double Min { get; set; }

    /// <summary>
    /// Value at the right end of the scale
    /// </summary>
    public double Max { get; set; }

    /// <summary>
    /// The value the needle points to. NaN hides the needle and the value in the bar.
    /// If <see cref="ExpandToFitValue"/> is enabled, a value outside the scale expands the scale.
    /// </summary>
    public double Value
    {
        get => _value;
        set
        {
            _value = value;
            if (ExpandToFitValue)
                ExpandToFit(value);
        }
    }

    private double _value;

    /// <summary>
    /// If enabled, setting <see cref="Value"/> beyond <see cref="Min"/> or <see cref="Max"/> moves that end
    /// of the scale at least <see cref="ExpandMargin"/> past the value, then on to round numbers:
    /// the scale becomes a whole number of sections, each a step of 1, 1.5, 2, 2.5, 3, 4, 5, 6, or 8 times a power of ten.
    /// </summary>
    public bool ExpandToFitValue { get; set; } = false;

    /// <summary>
    /// Fraction of the value an expanded scale extends beyond it so the needle does not rest at the end of the band
    /// </summary>
    public double ExpandMargin { get; set; } = 0.05;

    /// <summary>
    /// Text displayed after the value in the bar beneath the dial
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Colors of the band's sections from left to right.
    /// The number of colors sets the number of sections.
    /// </summary>
    public Color[] Colors { get; set; } =
    [
        new(0xE8, 0x43, 0x1E),
        new(0xF7, 0x82, 0x0C),
        new(0xFF, 0xCB, 0x2B),
        new(0x8D, 0xCB, 0x15),
        new(0x20, 0xB8, 0x20),
    ];

    /// <summary>
    /// If enabled, the band blends smoothly from one color to the next instead of filling each section with one color
    /// </summary>
    public bool SmoothColors { get; set; } = false;

    /// <summary>
    /// Optional pattern drawn over the band and the value bar (e.g., <see cref="Hatches.Glitter"/>)
    /// </summary>
    public IHatch? Hatch { get; set; } = null;

    /// <summary>
    /// Color of the <see cref="Hatch"/> pattern
    /// </summary>
    public Color HatchColor { get; set; } = ScottPlot.Colors.White;

    /// <summary>
    /// Fill of the needle and its hub
    /// </summary>
    public FillStyle NeedleFillStyle { get; set; } = new() { Color = new(0x13, 0x29, 0x33) };

    /// <summary>
    /// Style of the numbers around the band.
    /// The font size is set automatically to scale with the dial.
    /// </summary>
    public LabelStyle ScaleLabelStyle { get; set; } = new() { ForeColor = new(0x40, 0x40, 0x40), Alignment = Alignment.MiddleCenter };

    /// <summary>
    /// Style of the text in the value bar.
    /// The font size is set automatically to match the numbers around the band.
    /// </summary>
    public LabelStyle ValueLabelStyle { get; set; } = new() { Alignment = Alignment.MiddleCenter };

    /// <summary>
    /// Color of the text in the value bar.
    /// If null, black or white is used, whichever is easier to read on the bar's color.
    /// </summary>
    public Color? ValueLabelColor { get; set; } = null;

    /// <summary>
    /// Controls whether the bar showing the value and <see cref="Label"/> is displayed beneath the dial
    /// </summary>
    public bool ShowValueBar { get; set; } = true;

    /// <summary>
    /// Converts values to the text displayed around the band and in the value bar.
    /// If null, numbers are abbreviated with K and M (e.g., 88K and 2.5M).
    /// </summary>
    public Func<double, string>? LabelFormatter { get; set; } = null;

    /// <summary>
    /// The speedometer is drawn inside this fractional portion of the data area
    /// </summary>
    public FractionRect FractionRect { get; set; } = FractionRect.Full;

    // fractions of the band's outer radius R
    private const float TopSpace = 0.12f; // empty space above the band
    private const float BandWidth = 0.36f; // the band spans 0.64 R to R
    private const float GapWidth = 0.035f; // the gaps between sections have parallel sides
    private const float LabelGap = 0.03f; // from a band end down to its number, and from a gap out to its number
    private const float NumbersAt = 1 + LabelGap; // where the corner of a gap's number nearest the band is placed
    private const float BarGap = 0.04f; // from the numbers under the band (or the hub) down to the value bar
    private const float FontFraction = 0.10f;
    private const float NeedleLength = 0.76f;
    private const float NeedleBaseHalfWidth = 0.065f;
    private const float HubRadius = 0.085f;
    private const float HubDotRadius = 0.035f;

    // fractions of the font size
    private const float BarPaddingY = 0.25f;
    private const float BarPaddingX = 0.6f;

    private const float MinimumFontSize = 10; // a small dial shrinks to keep its text this size, unless the value bar's text is wider than the plot
    private const float Margin = 4; // pixels between the dial and the edges of its rectangle
    private const float FewerNumbersFraction = 0.85f; // numbers at fewer gaps when showing all of them would shrink the dial below this fraction
    private static readonly Color NoValueBarColor = new(0xE0, 0xE0, 0xE0);
    private static readonly LineStyle EdgeStyle = new() { Width = 1, Color = ScottPlot.Colors.Black }; // around each section of the band and the value bar
    private static readonly double[] RoundSteps = [1, 1.5, 2, 2.5, 3, 4, 5, 6, 8, 10];

    public Speedometer(double min, double max)
    {
        if (!(max >= min))
            throw new ArgumentException($"{nameof(max)} must not be less than {nameof(min)}");

        Min = min;
        Max = max;
        _value = min;
    }

    public override string ToString() => $"Speedometer from {Min} to {Max} with value {Value}";

    private int SectionCount => Math.Max(1, Colors.Length);

    /// <summary>
    /// Expand the scale to include the given value (see <see cref="ExpandToFitValue"/>)
    /// </summary>
    public void ExpandToFit(double value)
    {
        if (double.IsNaN(value))
            return;

        if (value < Min)
            Min = Max - RoundSpan(Max - (value - ExpandMargin * Math.Abs(value)), SectionCount);

        if (value > Max)
            Max = Min + RoundSpan(value + ExpandMargin * Math.Abs(value) - Min, SectionCount);
    }

    /// <summary>
    /// The smallest span of at least <paramref name="span"/> made of <paramref name="sections"/> equal round steps
    /// </summary>
    private static double RoundSpan(double span, int sections)
    {
        if (!(span > 0))
            return Math.Max(0, span);

        double step = span / sections;
        double power = Math.Pow(10, Math.Floor(Math.Log10(step)));
        double roundStep = RoundSteps.First(x => x * power >= step * (1 - 1e-12));
        return sections * roundStep * power;
    }

    /// <summary>
    /// Text for a number on a scale whose largest number is <paramref name="largest"/>:
    /// thousands and millions abbreviated with K and M, with one decimal only when the largest is below 10 in its unit.
    /// </summary>
    private static string ShortNumber(double value, double largest)
    {
        static (double scaled, string suffix) InUnit(double v) =>
            Math.Abs(v) >= 1e6 ? (v / 1e6, "M") :
            Math.Abs(v) >= 1e3 ? (v / 1e3, "K") :
            (v, string.Empty);

        (double scaled, string suffix) = InUnit(value);
        string format = Math.Abs(InUnit(largest).scaled) < 10 ? "0.#" : "0";
        return scaled.ToString(format) + suffix;
    }

    /// <summary>
    /// A number on the scale: where it stands (0 at the left end, 1 at the right), its text, and its width per pixel of font size
    /// </summary>
    private readonly record struct ScaleNumber(float Fraction, string Text, float Width)
    {
        public bool AtEnd => Fraction is 0 or 1;
    }

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        PixelRect rect = rp.DataRect.Fraction(FractionRect);
        double min = Min, max = Max, value = Value;
        Color[] colors = Colors.Length > 0 ? Colors : [ScottPlot.Colors.Gray];
        int sections = colors.Length;
        double largest = Math.Max(Math.Abs(min), Math.Abs(max));
        string Format(double v) => LabelFormatter?.Invoke(v) ?? ShortNumber(v, largest);

        // sizes of the text per pixel of font size
        ScaleLabelStyle.FontSize = 100;
        ValueLabelStyle.FontSize = 100;
        float numberHeight = ScaleLabelStyle.Measure("0", rp.Paint).Height / 100;
        float barHeight = ShowValueBar ? ValueLabelStyle.Measure("0", rp.Paint).Height / 100 + 2 * BarPaddingY : 0;
        ScaleNumber[] Measured(IEnumerable<(float fraction, double value)> values) => values
            .Select(x => new ScaleNumber(x.fraction, Format(x.value), ScaleLabelStyle.Measure(Format(x.value), rp.Paint).Width / 100))
            .ToArray();

        // the value bar's text is measured with the widest number the value may have so the dial keeps its size as the value changes
        string BarText(string number) => (number + " " + Label).Trim();
        string barText = double.IsNaN(value) ? Label : BarText(Format(value));
        float barTextWidth = ShowValueBar
            ? new[] { BarText(Format(min)), BarText(Format(max)), barText }.Max(x => ValueLabelStyle.Measure(x, rp.Paint).Width) / 100
            : 0;

        // numbers at every gap between sections, else at the first and last gap only, else at the ends of the band only
        List<ScaleNumber[]> choices = [];
        if (max > min && sections > 1)
        {
            (float, double) AtGap(int i) => ((float)i / sections, min + (max - min) * i / sections);
            choices.Add(Measured([(0, min), .. Enumerable.Range(1, sections - 1).Select(AtGap), (1, max)]));
            choices.Add(Measured([(0, min), AtGap(1), AtGap(sections - 1), (1, max)]));
        }
        choices.Add(Measured([(0, min), (1, max)]));
        float largestRadius = Fit(rect, choices[^1], numberHeight, barHeight, barTextWidth).radius;
        (ScaleNumber[] numbers, (float r, float fontSize)) = choices
            .Select(x => (x, Fit(rect, x, numberHeight, barHeight, barTextWidth)))
            .First(x => x.Item2.radius >= FewerNumbersFraction * largestRadius);

        if (r <= 4)
            return;

        // centered vertically: above the center the empty space and the band or the highest number,
        // below it the hub or the numbers under the band ends, then the value bar
        float above = (TopSpace + 1) * r;
        foreach (ScaleNumber number in numbers.Where(x => !x.AtEnd))
        {
            float numberTop = (TopSpace + NumbersAt * Math.Abs(Direction(number.Fraction).Y)) * r + numberHeight * fontSize;
            above = Math.Max(above, numberTop);
        }

        float below = Math.Max(HubRadius * r, LabelGap * r + numberHeight * fontSize);
        float barGap = ShowValueBar ? BarGap * r : 0;
        float height = above + below + barGap + barHeight * fontSize;
        Pixel center = new(rect.HorizontalCenter, rect.VerticalCenter - height / 2 + above);
        float fraction = double.IsNaN(value) || !(max > min) ? 0 : (float)NumericConversion.Clamp((value - min) / (max - min), 0, 1);
        int section = Math.Min(sections - 1, (int)(fraction * sections));

        RenderBand(rp, center, r, colors);

        ScaleLabelStyle.FontSize = fontSize;
        foreach (ScaleNumber number in numbers)
        {
            RenderScaleNumber(rp, center, r, number);
        }

        if (ShowValueBar)
        {
            float barTop = center.Y + below + barGap;
            PixelRect barRect = new(
                left: center.X - Math.Max(r, barTextWidth * fontSize / 2 + BarPaddingX * fontSize),
                right: center.X + Math.Max(r, barTextWidth * fontSize / 2 + BarPaddingX * fontSize),
                bottom: barTop + barHeight * fontSize,
                top: barTop);
            Color barColor = double.IsNaN(value) ? NoValueBarColor : SectionColor(colors, section);
            RenderValueBar(rp, barRect, barColor, barText, fontSize, !double.IsNaN(value));
        }

        RenderNeedle(rp, center, r, double.IsNaN(value) ? null : fraction);
    }

    /// <summary>
    /// The band's outer radius and the font size of its text that fit the dial into <paramref name="rect"/>:
    /// the font scales with the dial, or stays at <see cref="MinimumFontSize"/> as a small dial shrinks around it
    /// (smaller only where the value bar's text would be wider than the rectangle)
    /// </summary>
    private (float radius, float fontSize) Fit(PixelRect rect, ScaleNumber[] numbers, float numberHeight, float barHeight, float barTextWidth)
    {
        float r = Radius(rect, numbers, numberHeight, barHeight, barTextWidth, FontFraction, 0);
        if (FontFraction * r >= MinimumFontSize)
            return (r, FontFraction * r);

        float fontSize = ShowValueBar
            ? Math.Min(MinimumFontSize, (rect.Width - 2 * Margin) / (barTextWidth + 2 * BarPaddingX))
            : MinimumFontSize;

        return (Radius(rect, numbers, numberHeight, barHeight, barTextWidth, 0, fontSize), fontSize);
    }

    /// <summary>
    /// The largest outer radius R that fits the dial into <paramref name="rect"/> with its text at font size
    /// <paramref name="fontFraction"/> * R + <paramref name="fontFixed"/>
    /// </summary>
    private float Radius(PixelRect rect, ScaleNumber[] numbers, float numberHeight, float barHeight, float barTextWidth, float fontFraction, float fontFixed)
    {
        float halfWidth = rect.Width / 2 - Margin;
        float height = rect.Height - 2 * Margin;
        float r = float.MaxValue;

        // each extent is p * R + q * fontSize with fontSize = fontFraction * R + fontFixed
        void Limit(float p, float q, float room)
        {
            float perRadius = p + q * fontFraction;
            if (perRadius > 0)
                r = Math.Min(r, (room - q * fontFixed) / perRadius);
        }

        Limit(1, 0, halfWidth);

        if (ShowValueBar)
            Limit(0, barTextWidth / 2 + BarPaddingX, halfWidth);

        // numbers at the gaps extend outward and upward from their corner
        List<(float p, float q)> tops = [(1, 0)];
        foreach (ScaleNumber number in numbers.Where(x => !x.AtEnd))
        {
            PixelOffset direction = Direction(number.Fraction);
            Limit(NumbersAt * Math.Abs(direction.X), number.Width, halfWidth);
            tops.Add((NumbersAt * Math.Abs(direction.Y), numberHeight));
        }

        // above the center the band or the highest number, below it the hub or the numbers under the band ends, then the value bar
        float barGap = ShowValueBar ? BarGap : 0;
        foreach ((float p, float q) in tops)
        {
            Limit(TopSpace + p + HubRadius + barGap, q + barHeight, height);
            Limit(TopSpace + p + LabelGap + barGap, q + numberHeight + barHeight, height);
        }

        return r;
    }

    private void RenderBand(RenderPack rp, Pixel center, float r, Color[] colors)
    {
        int sections = colors.Length;
        float inner = r * (1 - BandWidth);
        PixelRect outerRect = new(center, r);

        // gaps between sections with parallel sides, cut out of the band
        using SKPath gaps = new();
        for (int i = 1; i < sections; i++)
        {
            PixelOffset d = Direction((float)i / sections);
            PixelOffset across = new(-d.Y * GapWidth * r / 2, d.X * GapWidth * r / 2);
            Pixel from = At(center, d, inner - 1);
            Pixel to = At(center, d, r + 1);
            gaps.MoveTo(from.X + across.X, from.Y + across.Y);
            gaps.LineTo(to.X + across.X, to.Y + across.Y);
            gaps.LineTo(to.X - across.X, to.Y - across.Y);
            gaps.LineTo(from.X - across.X, from.Y - across.Y);
            gaps.Close();
        }

        if (SmoothColors && sections > 1)
        {
            using SKPath band = BandPath(center, r, inner, 0, 1, gaps);
            FillStyle fill = new()
            {
                Color = colors[0],
                Hatch = new Gradient(GradientType.Sweep) { Colors = colors, StartAngle = 180, EndAngle = 360 },
            };
            Drawing.FillPath(rp.Canvas, rp.Paint, band, fill, outerRect);
            RenderHatch(rp, band, outerRect);
            Drawing.DrawPath(rp.Canvas, rp.Paint, band, EdgeStyle);
            return;
        }

        for (int i = 0; i < sections; i++)
        {
            using SKPath path = BandPath(center, r, inner, (float)i / sections, (float)(i + 1) / sections, gaps);
            Drawing.FillPath(rp.Canvas, rp.Paint, path, new FillStyle(colors[i]), outerRect);
            RenderHatch(rp, path, outerRect);
            Drawing.DrawPath(rp.Canvas, rp.Paint, path, EdgeStyle);
        }
    }

    /// <summary>
    /// Draw the <see cref="Hatch"/> pattern over what is already drawn inside the path
    /// </summary>
    private void RenderHatch(RenderPack rp, SKPath path, PixelRect rect)
    {
        if (Hatch is null)
            return;

        // the pattern's background is the hatch color made transparent so only the pattern shows
        Color background = new(HatchColor.Red, HatchColor.Green, HatchColor.Blue, 0);
        rp.Paint.Color = ScottPlot.Colors.White;
        rp.Paint.IsStroke = false;
        rp.Paint.IsAntialias = true;
        rp.Paint.SKShader = Hatch.GetShader(background, HatchColor, rect);
        rp.Canvas.DrawPath(path, rp.Paint.SKPaint);
        rp.Paint.SKShader = null;
    }

    /// <summary>
    /// Min's number under the left end of the band flush with its outer edge, Max's under the right end flush with its outer edge,
    /// and a gap's number beyond the outer end of the gap: ending there left of the middle, starting there right of it
    /// </summary>
    private void RenderScaleNumber(RenderPack rp, Pixel center, float r, ScaleNumber number)
    {
        MeasuredText size = ScaleLabelStyle.Measure(number.Text, rp.Paint);
        int outward = number.Fraction < 0.5 ? -1 : 1;

        Pixel px;
        if (number.AtEnd)
        {
            px = new(center.X + outward * (r - size.Width / 2), center.Y + LabelGap * r + size.Height / 2);
        }
        else
        {
            Pixel corner = At(center, Direction(number.Fraction), r * NumbersAt);
            px = new(corner.X + outward * size.Width / 2, corner.Y - size.Height / 2);
        }

        ScaleLabelStyle.Render(rp.Canvas, px, rp.Paint, number.Text);
    }

    private void RenderValueBar(RenderPack rp, PixelRect barRect, Color barColor, string text, float fontSize, bool hasValue)
    {
        Drawing.FillRectangle(rp.Canvas, barRect, rp.Paint, new FillStyle(barColor));
        if (hasValue)
        {
            using SKPath path = new();
            path.AddRect(barRect.ToSKRect());
            RenderHatch(rp, path, barRect);
        }

        Drawing.DrawRectangle(rp.Canvas, barRect, rp.Paint, EdgeStyle);

        if (string.IsNullOrEmpty(text))
            return;

        Color textColor = ValueLabelColor ?? (PrefersDarkText(barColor) ? ScottPlot.Colors.Black : ScottPlot.Colors.White);
        Pixel px = new(barRect.HorizontalCenter, barRect.VerticalCenter);
        ValueLabelStyle.FontSize = fontSize;

        // over a pattern the text gets a soft outline of the opposite color so the pattern does not run into the letters
        if (Hatch is not null && hasValue)
        {
            ValueLabelStyle.ForeColor = textColor == ScottPlot.Colors.Black
                ? ScottPlot.Colors.White.WithAlpha(0.8)
                : ScottPlot.Colors.Black.WithAlpha(0.7);

            float d = Math.Max(1, 0.08f * fontSize);
            foreach ((float dx, float dy) in new[] { (-d, 0f), (d, 0f), (0f, -d), (0f, d), (-d, -d), (d, -d), (-d, d), (d, d) })
            {
                ValueLabelStyle.Render(rp.Canvas, new Pixel(px.X + dx, px.Y + dy), rp.Paint, text);
            }
        }

        ValueLabelStyle.ForeColor = textColor;
        ValueLabelStyle.Render(rp.Canvas, px, rp.Paint, text);
    }

    /// <summary>
    /// A wedge from the center to the value with a rounded tip, then the hub with a white dot
    /// </summary>
    private void RenderNeedle(RenderPack rp, Pixel center, float r, float? fraction)
    {
        PixelRect rect = new(center, r);

        if (fraction is not null)
        {
            PixelOffset d = Direction(fraction.Value);
            PixelOffset across = new(-d.Y, d.X);
            Pixel tip = At(center, d, r * NeedleLength);
            Pixel left = At(center, across, r * NeedleBaseHalfWidth);
            Pixel right = At(center, across, -r * NeedleBaseHalfWidth);

            using SKPath path = new();
            path.MoveTo(tip.ToSKPoint());
            path.LineTo(left.ToSKPoint());
            path.LineTo(right.ToSKPoint());
            path.Close();

            LineStyle outline = new()
            {
                Width = 0.02f * r,
                Color = NeedleFillStyle.Color,
                StrokeJoin = SKStrokeJoin.Round,
            };

            Drawing.DrawPath(rp.Canvas, rp.Paint, path, outline);
            Drawing.FillPath(rp.Canvas, rp.Paint, path, NeedleFillStyle, rect);
        }

        Drawing.FillCircle(rp.Canvas, center, HubRadius * r, NeedleFillStyle, rp.Paint);
        Drawing.FillCircle(rp.Canvas, center, HubDotRadius * r, new FillStyle(ScottPlot.Colors.White), rp.Paint);
    }

    /// <summary>
    /// A section of the band from <paramref name="from"/> to <paramref name="to"/> (fractions of the band, 0 at the left)
    /// with the gaps cut out of it
    /// </summary>
    private static SKPath BandPath(Pixel center, float r, float inner, float from, float to, SKPath gaps)
    {
        float start = 180 + 180 * from;
        float sweep = 180 * (to - from);

        using SKPath sector = new();
        sector.ArcTo(new PixelRect(center, r).ToSKRect(), start, sweep, true);
        sector.ArcTo(new PixelRect(center, inner).ToSKRect(), start + sweep, -sweep, false);
        sector.Close();

        return sector.Op(gaps, SKPathOp.Difference) ?? new SKPath(sector);
    }

    /// <summary>
    /// The color of a section, or the color in the middle of the section when the band blends its colors
    /// </summary>
    private Color SectionColor(Color[] colors, int section)
    {
        if (!SmoothColors || colors.Length < 2)
            return colors[section];

        double position = (section + 0.5) / colors.Length * (colors.Length - 1);
        int index = Math.Min(colors.Length - 2, (int)position);
        return Color.InterpolateRgb(colors[index], colors[index + 1], position - index);
    }

    /// <summary>
    /// Whether black text is easier to read on the given background than white text,
    /// judged by APCA lightness contrast which matches perception better than the WCAG 2 contrast ratio
    /// (that ratio prefers black on saturated reds and blues where white is easier to read)
    /// </summary>
    private static bool PrefersDarkText(Color background)
    {
        static double Luminance(double r, double g, double b)
        {
            double y = 0.2126729 * Math.Pow(r, 2.4) + 0.7151522 * Math.Pow(g, 2.4) + 0.0721750 * Math.Pow(b, 2.4);
            return y < 0.022 ? y + Math.Pow(0.022 - y, 1.414) : y;
        }

        double back = Luminance(background.Red / 255.0, background.Green / 255.0, background.Blue / 255.0);
        double darkText = 1.14 * (Math.Pow(back, 0.56) - Math.Pow(Luminance(0, 0, 0), 0.57)) - 0.027;
        double lightText = 1.14 * (Math.Pow(Luminance(1, 1, 1), 0.62) - Math.Pow(back, 0.65)) - 0.027;
        return darkText >= lightText;
    }

    /// <summary>
    /// Unit vector from the center toward a fraction of the band: left at 0, up at 0.5, right at 1 (pixel Y points down)
    /// </summary>
    private static PixelOffset Direction(float fraction)
    {
        double angle = Math.PI * (1 + fraction);
        return new PixelOffset((float)Math.Cos(angle), (float)Math.Sin(angle));
    }

    private static Pixel At(Pixel center, PixelOffset direction, float distance)
    {
        return new Pixel(center.X + direction.X * distance, center.Y + direction.Y * distance);
    }
}
