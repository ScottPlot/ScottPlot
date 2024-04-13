using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ScottPlot.OpenGL.GLPrograms;

public class MarkerOpenSquareProgram : MarkerFillSquareProgram
{
    protected override string GeometryShaderSource =>
    @"# version 430 core
        layout(points) in;
        layout(triangle_strip, max_vertices=4) out;

        layout(location = 1) uniform vec2 u_viewport_size;
        layout(location = 2) uniform float marker_size;

        out noperspective vec2 g_uv;

        void main()
        {
            vec4 center = gl_in[0].gl_Position;
            vec2 offset = marker_size / u_viewport_size;

            g_uv = vec2(-1, -1);
            gl_Position = center + vec4(g_uv*offset, 0, 0);
            EmitVertex();
            g_uv = vec2(1, -1);
            gl_Position = center + vec4(g_uv*offset, 0, 0);
            EmitVertex();
            g_uv = vec2(-1, 1);
            gl_Position = center + vec4(g_uv*offset, 0, 0);
            EmitVertex();
            g_uv = vec2(1, 1);
            gl_Position = center + vec4(g_uv*offset, 0, 0);
            EmitVertex();

            EndPrimitive();
        }";

    protected override string FragmentShaderSource =>
    @"#version 430 core
        out vec4 FragColor;
        uniform vec4 pathColor;
        uniform float openFactor;

        in noperspective vec2 g_uv;

        void main()
        {
            vec2 g_uv_abs = abs(g_uv);
            if (g_uv_abs.x > openFactor || g_uv_abs.y > openFactor)
                FragColor = pathColor;
        }";

    public override void SetFillColor(Color4 color)
    {

    }

    public override void SetOpenFactor(float openFactor)
    {
        var location = GetUniformLocation("openFactor");
        GL.Uniform1(location, openFactor);
    }

    public override void SetOutlineColor(Color4 color)
    {
        var location = GetUniformLocation("pathColor");
        GL.Uniform4(location, color);
    }
}
