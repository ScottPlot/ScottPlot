namespace ScottPlot;

/// <summary>
/// Describes X/Y location in pixel or coordinate space
/// </summary>
public class Location
{
    public LocationUnit Unit { get; init; }
    public double X { get; set; }
    public double Y { get; set; }

    private Location()
    {
    }

    public Pixel GetPixel()
    {
        if (Unit != LocationUnit.Pixel)
            throw new InvalidOperationException("units are not pixels");

        return new Pixel(X, Y);
    }

    public Coordinates GetCoordinates()
    {
        if (Unit != LocationUnit.Coordinates)
            throw new InvalidOperationException("units are not pixels");

        return new Coordinates(X, Y);
    }

    public static Location Pixel(float x, float y)
    {
        return new Location()
        {
            X = x,
            Y = y,
            Unit = LocationUnit.Pixel
        };
    }

    public static Location Coordinates(float x, float y)
    {
        return new Location()
        {
            X = x,
            Y = y,
            Unit = LocationUnit.Coordinates
        };
    }

    public static Location Unspecified
    {
        get
        {
            return new Location()
            {
                X = double.NaN,
                Y = double.NaN,
                Unit = LocationUnit.Unspecified
            };
        }
    }
}
