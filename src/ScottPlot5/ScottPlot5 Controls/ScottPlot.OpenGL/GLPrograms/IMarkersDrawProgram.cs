using OpenTK;
using OpenTK.Graphics;

namespace ScottPlot.OpenGL.GLPrograms;

public interface IMarkersDrawProgram : IGLProgram
{
    void SetFillColor(Color4 color);
    void SetOutlineColor(Color4 color);
    void SetMarkerSize(float size);
    void SetTransform(Matrix4d transform);
    void SetViewPortSize(float width, float height);
    void SetOpenFactor(float factor);
}
