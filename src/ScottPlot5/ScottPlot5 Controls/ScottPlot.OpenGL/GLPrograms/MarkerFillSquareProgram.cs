namespace ScottPlot.OpenGL.GLPrograms;

public class MarkerFillSquareProgram : MarkersProgram
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
}
