using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ScottPlot.OpenGL.GLPrograms;

/// <summary>
/// A lines program which allows customization of color and width
/// </summary>
public class LinesProgramCustom : GLProgramBase, ILinesDrawProgram
{
    protected override string VertexShaderSource =>
    @"# version 430 core
        layout(location = 0) in dvec2 aPosition;
        layout(location = 0) uniform dmat4 transform;

        void main()
        {
            dvec4 posd = dvec4(aPosition, 0.0, 1.0);
            dvec4 transformedD = posd * transform;
            gl_Position = vec4(transformedD);
        }";

    protected override string GeometryShaderSource =>
    @"# version 430 core
        layout(lines) in;
        layout(triangle_strip, max_vertices=4) out;

        uniform vec2 u_viewport_size;
        uniform float v_line_width;

        void main()
        {
            float u_width        = u_viewport_size[0];
            float u_height       = u_viewport_size[1];
            float u_aspect_ratio = u_height / u_width;

            vec2 ndc_a = gl_in[0].gl_Position.xy;
            vec2 ndc_b = gl_in[1].gl_Position.xy;

            vec2 line_vector = ndc_b - ndc_a;
            vec2 dir = normalize(vec2( line_vector.x, line_vector.y * u_aspect_ratio ));

            float line_width  = max(1.0, v_line_width);
        
            vec2 normal    = vec2( -dir.y, dir.x );
            vec2 normal_p  = vec2( line_width/u_width, line_width/u_height ) * normal;

            gl_Position = vec4( (ndc_a + normal_p), 0, 1);
            EmitVertex();

            gl_Position = vec4( (ndc_a - normal_p), 0, 1);
            EmitVertex();

            gl_Position = vec4( (ndc_b + normal_p), 0, 1);
            EmitVertex();

            gl_Position = vec4( (ndc_b - normal_p), 0, 1);
            EmitVertex();
            EndPrimitive();
        }";

    protected override string FragmentShaderSource =>
    @"#version 430 core

    uniform vec4 pathColor;

    out vec4 FragColor;

    void main()
    {
      FragColor = pathColor;
    }";

    public void SetLineWidth(float lineWidth)
    {
        int location = GetUniformLocation("v_line_width");
        GL.Uniform1(location, lineWidth);
    }

    public void SetViewPortSize(float width, float height)
    {
        int location = GetUniformLocation("u_viewport_size");
        Vector2 viewPortSize = new Vector2(width, height);
        GL.Uniform2(location, viewPortSize);
    }

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
}
