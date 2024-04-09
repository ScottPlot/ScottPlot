namespace ScottPlot.PathStrategies;

public static class Arrows // TODO: use an interface to let users inject custom logic
{
    public static SKPath GetPath(IEnumerable<RootedPixelVector> vectors, ArrowStyle style)
    {
        const float bladeLengthFactor = 0.35f; // How long the arrowhead is as a proportion of arrow length
        const float bladeWidthFactor = 0.2f; // How wide the arrowhead is as a proportion of arrow length
        const float cutInFactor = 0.15f; // How much the base of the arrowhead is cut away as a proportion of arrow length (0 for perfect triangle)

        SKPath path = new();

        foreach (RootedPixelVector vector in vectors)
        {
            SKPoint start = style.Anchor switch
            {
                ArrowAnchor.Center => new(vector.Point.X - 0.5f * vector.Vector.X, vector.Point.Y - 0.5f * vector.Vector.Y),
                ArrowAnchor.Tail => vector.Point.ToSKPoint(),
                ArrowAnchor.Tip => new(vector.Point.X - vector.Vector.X, vector.Point.Y - vector.Vector.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(style), "Unexpected arrow anchor value"),
            };

            path.MoveTo(start);
            path.RLineTo(vector.Vector.X, vector.Vector.Y);
            var head = path.LastPoint;

            var junction = head + new SKPoint(-bladeLengthFactor * vector.Vector.X, -bladeLengthFactor * vector.Vector.Y);

            var perp = new SKPoint(-vector.Vector.Y, vector.Vector.X);
            var bladeTip1 = junction + new SKPoint(bladeWidthFactor * perp.X, bladeWidthFactor * perp.Y);
            var bladeTip2 = junction - new SKPoint(bladeWidthFactor * perp.X, bladeWidthFactor * perp.Y);

            var arrowHeadBegin = junction + new SKPoint(cutInFactor * vector.Vector.X, cutInFactor * vector.Vector.Y);

            path.AddPoly([head, bladeTip1, arrowHeadBegin, bladeTip2]);
        }

        return path;
    }
}
