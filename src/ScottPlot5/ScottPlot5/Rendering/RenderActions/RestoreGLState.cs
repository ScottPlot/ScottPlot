namespace ScottPlot.Rendering.RenderActions;

public class RestoreGLState : IRenderAction
{
    public void Render(RenderPack rp)
    {
        var glPlottables = rp.Plot.PlottableList.OfType<IPlottableGL>();

        glPlottables.FirstOrDefault()?.RestoreGLState();
    }
}
