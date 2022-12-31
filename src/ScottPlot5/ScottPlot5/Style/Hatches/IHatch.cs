namespace ScottPlot.Style.Hatches;

public interface IHatch
{
    SKShader GetShader(Color backgroundColor, Color hatchColor);
}
