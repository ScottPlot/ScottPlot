using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ScottPlot.OpenGL.GLPrograms;

public class MarkerOpenCircleProgram : MarkerFillCircleProgram
{
    protected override string FragmentShaderSource =>
    @"#version 430 core
        in noperspective vec2 g_uv;
        uniform vec4 pathColor;
        uniform float openFactor;
        out vec4 FragColor;

        void main()
        {
            float distance = length(g_uv);
            if (distance <= 1 && distance >= openFactor)
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
