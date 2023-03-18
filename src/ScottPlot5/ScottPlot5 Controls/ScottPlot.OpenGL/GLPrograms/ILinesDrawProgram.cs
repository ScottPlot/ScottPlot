using OpenTK;
using OpenTK.Graphics;

namespace ScottPlot.OpenGL.GLPrograms;

public interface ILinesDrawProgram : IGLProgram
{
    void SetLineWidth(float lineWidth);
    void SetViewPortSize(float width, float height);
    void SetTransform(Matrix4d transform);
    void SetColor(Color4 color);
}
