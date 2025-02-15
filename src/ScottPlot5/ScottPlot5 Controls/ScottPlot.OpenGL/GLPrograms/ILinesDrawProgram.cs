using OpenTK;
using OpenTK.Graphics;
#if NETCOREAPP || NET
using OpenTK.Mathematics;
#endif

namespace ScottPlot.OpenGL.GLPrograms;

public interface ILinesDrawProgram : IGLProgram
{
    void SetLineWidth(float lineWidth);
    void SetViewPortSize(float width, float height);
    void SetTransform(Matrix4d transform);
    void SetColor(Color4 color);
}
