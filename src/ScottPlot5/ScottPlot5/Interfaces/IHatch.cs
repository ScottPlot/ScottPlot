namespace ScottPlot;

public interface IHatch
{
    SKShader GetShader(Color backgroundColor, Color hatchColor, PixelRect rect);
}
