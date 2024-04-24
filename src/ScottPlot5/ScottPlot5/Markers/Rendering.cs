namespace ScottPlot.Markers;

public static class Rendering
{
    // TODO: obsolete this in favor of a method that doesn't modify properties
    public static IMarker GetRendererWithSpecialProperties(this MarkerShape shape)
    {
        return shape switch
        {
            MarkerShape.FilledCircle => new Circle() { Fill = true, LineWidth = 0 },
            MarkerShape.OpenCircle => new Circle() { Fill = false, LineWidth = 1 },
            MarkerShape.FilledSquare => new Square() { Fill = true, LineWidth = 0 },
            MarkerShape.OpenSquare => new Square() { Fill = false, LineWidth = 1 },
            MarkerShape.FilledTriangleUp => new TriangleUp() { Fill = true, LineWidth = 0 },
            MarkerShape.OpenTriangleUp => new TriangleUp() { Fill = false, LineWidth = 1 },
            MarkerShape.FilledTriangleDown => new TriangleDown() { Fill = true, LineWidth = 0 },
            MarkerShape.OpenTriangleDown => new TriangleDown() { Fill = false, LineWidth = 1 },
            MarkerShape.FilledDiamond => new Diamond() { Fill = true, LineWidth = 0 },
            MarkerShape.OpenDiamond => new Diamond() { Fill = false, LineWidth = 1 },
            MarkerShape.Eks => new Eks() { Fill = false, LineWidth = 1 },
            MarkerShape.Cross => new Cross() { Fill = false, LineWidth = 1 },
            MarkerShape.VerticalBar => new VerticalBar() { Fill = false, LineWidth = 1 },
            MarkerShape.HorizontalBar => new HorizontalBar() { Fill = false, LineWidth = 1 },
            MarkerShape.TriUp => new TriUp() { Fill = false, LineWidth = 1 },
            MarkerShape.TriDown => new TriDown() { Fill = false, LineWidth = 1 },
            MarkerShape.Asterisk => new Asterisk() { Fill = false, LineWidth = 1 },
            MarkerShape.HashTag => new HashTag() { Fill = false, LineWidth = 1 },
            MarkerShape.None => new None() { Fill = false, LineWidth = 0 },
            _ => throw new NotImplementedException(shape.ToString()),
        };
    }
}
