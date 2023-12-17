namespace ScottPlot.DataSources
{
    public class FunctionSource : IFunctionSource
    {
        public CoordinateRange RangeX { get; set; } = CoordinateRange.Infinity;
        public Func<double, double> EvaluateFunc { get; set; }
        public Func<CoordinateRange, CoordinateRange>? GetRangeYFunc { get; set; } = null;

        public FunctionSource(Func<double, double> evaluateFunc)
        {
            EvaluateFunc = evaluateFunc;
        }

        public double Get(double x) => EvaluateFunc(x);

        public CoordinateRange GetRangeY(CoordinateRange xs) =>
            GetRangeYFunc is not null
            ? GetRangeYFunc(xs)
            : new CoordinateRange(double.NaN, double.NaN);
    }
}
