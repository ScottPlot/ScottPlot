namespace ScottPlot.OpenGL.GLPrograms;

public class MarkerFillCircleProgram : MarkersProgram
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

        out noperspective vec2 g_uv;

        void main()
        {
            vec4 center = gl_in[0].gl_Position;

            vec2 offset = vec2(marker_size, marker_size) / u_viewport_size;

            g_uv = vec2(-1, -1);
            gl_Position = center + vec4(g_uv * offset, 0, 0);
            EmitVertex();
            g_uv = vec2(1, -1);
            gl_Position = center + vec4(g_uv * offset, 0, 0);
            EmitVertex();
            g_uv = vec2(-1, 1);
            gl_Position = center + vec4(g_uv * offset, 0, 0);
            EmitVertex();
            g_uv = vec2(1, 1);
            gl_Position = center + vec4(g_uv * offset, 0, 0);
            EmitVertex();

            EndPrimitive();
        }";

    protected override string FragmentShaderSource =>
    @"#version 430 core

        uniform vec4 pathColor;
        in noperspective vec2 g_uv;
        out vec4 FragColor;

        void main()
        {
            float distance = length(g_uv);
            if (distance <= 1)
                FragColor = pathColor;
        }";
}
