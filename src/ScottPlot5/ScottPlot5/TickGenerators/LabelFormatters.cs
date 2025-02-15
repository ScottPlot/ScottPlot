namespace ScottPlot.TickGenerators;

// TODO: Consider creating a `LabelFormatter` type or `ILabelFormatter` because we use this pattern in a lot of places.

/* Maybe something like this?

public interface ILabelFormatter
{
    public string GetLabel<T>(T value);
}
*/

/// <summary>
/// A collection of methods which contain logic for choosing how a value is representing with text
/// </summary>
public class LabelFormatters
{
    public static string Numeric(double value)
    {
        // use system culture
        // https://github.com/ScottPlot/ScottPlot/issues/3688

        // if the number is round or large, use the numeric format
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-numeric-n-format-specifier
        bool isRoundNumber = (int)value == value;
        bool isLargeNumber = Math.Abs(value) > 1000;
        if (isRoundNumber || isLargeNumber)
            return value.ToString("N0");

        // otherwise the number is probably small or very precise to use the general format (with slight rounding)
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-general-g-format-specifier
        string label = Math.Round(value, 10).ToString("G");
        return label == "-0" ? "0" : label;
    }
}
