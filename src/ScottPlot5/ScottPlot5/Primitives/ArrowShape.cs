namespace ScottPlot;

public enum ArrowShape
{
    Single,
    SingleLine,
    Double,
    DoubleLine,
    Arrowhead,
    ArrowheadLine,
    Pentagon,
    Chevron,
}

public static class ArrowShapeExtensions
{
    public static IArrowShape GetShape(this ArrowShape shape)
    {
        return shape switch
        {
            ArrowShape.Single => new ArrowShapes.Single(),
            ArrowShape.SingleLine => new ArrowShapes.SingleLine(),
            ArrowShape.Double => new ArrowShapes.Double(),
            ArrowShape.DoubleLine => new ArrowShapes.DoubleLine(),
            ArrowShape.Arrowhead => new ArrowShapes.Arrowhead(),
            ArrowShape.ArrowheadLine => new ArrowShapes.ArrowheadLine(),
            ArrowShape.Pentagon => new ArrowShapes.Pentagon(),
            ArrowShape.Chevron => new ArrowShapes.Chevron(),
            _ => throw new NotImplementedException(shape.ToString()),
        };
    }
}
