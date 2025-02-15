namespace ScottPlot.Rendering.RenderActions;

public class StoreGLState : IRenderAction
{
    public void Render(RenderPack rp)
    {
        var glPlottables = rp.Plot.PlottableList.OfType<IPlottableGL>();

        glPlottables.FirstOrDefault()?.StoreGLState();
    }
}
