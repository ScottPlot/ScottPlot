namespace ScottPlot.Plottable
{
    public interface IHasDataValidation
    {
        /// <summary>
        /// Throw InvalidOperationException if ciritical variables are null or have incorrect sizes. 
        /// Deep validation is slower but also checks every value for NaN and Infinity.
        /// </summary>
        void ValidateData(bool deep = false);
    }
}
