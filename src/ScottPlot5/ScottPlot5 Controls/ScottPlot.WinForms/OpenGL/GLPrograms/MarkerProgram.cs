using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ScottPlot.WinForms.OpenGL.GLPrograms;

public class MarkersProgram : GLProgramBase, IMarkersDrawProgram
{
    protected override string VertexShaderSource =>
    @"# version 430 core
        layout(location = 0) in dvec2 aPosition;
        uniform dmat4 transform;

        void main()
        {
            dvec4 posd = dvec4(aPosition, 0.0, 1.0);
            dvec4 transformedD = posd * transform;
            gl_Position = vec4(transformedD);
        }";

    protected override string GeometryShaderSource =>
    @"# version 430 core
        layout(points) in;
        layout(triangle_strip, max_vertices=4) out;

        layout(location = 1) uniform vec2 u_viewport_size;
        layout(location = 2) uniform float marker_size;

        void main()
        {
            vec4 center = gl_in[0].gl_Position;
            float offset_x = marker_size / u_viewport_size[0];
            float offset_y = marker_size / u_viewport_size[1];

            gl_Position = center + vec4(-offset_x, -offset_y, 0, 0);
            EmitVertex();
            gl_Position = center + vec4(offset_x, -offset_y, 0, 0);
            EmitVertex();
            gl_Position = center + vec4(-offset_x, offset_y, 0, 0);
            EmitVertex();
            gl_Position = center + vec4(offset_x, offset_y, 0, 0);
            EmitVertex();

            EndPrimitive();
        }";

    protected override string FragmentShaderSource =>
    @"#version 430 core
        out vec4 FragColor;
        uniform vec4 pathColor;

        void main()
        {
            FragColor = pathColor;
        }";

    public void SetTransform(Matrix4d transform)
    {
        var location = GetUniformLocation("transform");
        GL.UniformMatrix4(location, true, ref transform);
    }

    public void SetColor(Color4 color)
    {
        var location = GetUniformLocation("pathColor");
        GL.Uniform4(location, color);
    }

    public void SetMarkerSize(float size)
    {
        var location = GetUniformLocation("marker_size");
        GL.Uniform1(location, size);
    }

    public void SetViewPortSize(float width, float height)
    {
        int location = GetUniformLocation("u_viewport_size");
        Vector2 viewPortSize = new Vector2(width, height);
        GL.Uniform2(location, viewPortSize);
    }
}
