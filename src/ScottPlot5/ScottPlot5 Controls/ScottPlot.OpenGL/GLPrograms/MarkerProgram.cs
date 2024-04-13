using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ScottPlot.OpenGL.GLPrograms;

public abstract class MarkersProgram : GLProgramBase, IMarkersDrawProgram
{
    protected override string? VertexShaderSource => null;

    protected override string? GeometryShaderSource => null;

    protected override string? FragmentShaderSource => null;

    public void SetTransform(Matrix4d transform)
    {
        var location = GetUniformLocation("transform");
        GL.UniformMatrix4(location, true, ref transform);
    }

    public virtual void SetFillColor(Color4 color)
    {
        var location = GetUniformLocation("pathColor");
        GL.Uniform4(location, color);
    }

    public void SetMarkerSize(float size)
    {
        var location = GetUniformLocation("marker_size");
        GL.Uniform1(location, size);
    }

    public virtual void SetOutlineColor(Color4 color)
    {

    }

    public void SetViewPortSize(float width, float height)
    {
        int location = GetUniformLocation("u_viewport_size");
        Vector2 viewPortSize = new(width, height);
        GL.Uniform2(location, viewPortSize);
    }

    public virtual void SetOpenFactor(float factor)
    {

    }
}
