using OpenTK;
using OpenTK.Graphics;

namespace ScottPlot.WinForms.OpenGL.GLPrograms
{
    public interface IMarkersDrawProgram : IGLProgram
    {
        void SetColor(Color4 color);
        void SetMarkerSize(float size);
        void SetTransform(Matrix4d transform);
        void SetViewPortSize(float width, float height);
    }
}