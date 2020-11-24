namespace ScottPlot.Plottable
{
    public interface IValidatable
    {
        string ErrorMessage(bool deepValidation = false);
    }
}
