using OpenTK;
using OpenTK.Graphics.OpenGL;
using ScottPlot.DataSources;
using ScottPlot.WinForms.OpenGL;
using SkiaSharp;
using System;
using System.Linq;

namespace ScottPlot.Plottables;

/// <summary>
/// This plot type uses an OpenGL shader for rendering.
/// </summary>
public class ScatterGL : Scatter, IPlottableGL
{
    private readonly GRContext _context;
    private int VertexBufferObject;
    private int VertexArrayObject;
    private GLShader? Shader;
    private double[]? Vertices;
    private readonly int VerticesCount;

    private bool _glInit = false;

    public GRContext GRContext => _context;

    public ScatterGL(IScatterSource data, GRContext context) : base(data)
    {
        _context = context;
        Vertices = data.GetScatterPoints().Select(p =>
        {
            return new double[] { p.X, p.Y };
        }).
        SelectMany(t => t).ToArray();
        VerticesCount = Vertices.Length / 2;
    }

    private void InitGL()
    {
        if (Vertices is null)
            throw new NullReferenceException(nameof(Vertices));

        Shader = new GLShader();
        VertexArrayObject = GL.GenVertexArray();
        VertexBufferObject = GL.GenBuffer();
        GL.BindVertexArray(VertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(double), Vertices, BufferUsageHint.StaticDraw);
        GL.VertexAttribLPointer(0, 2, VertexAttribDoubleType.Double, 0, IntPtr.Zero);
        GL.EnableVertexAttribArray(0);
        Vertices = null;
        _glInit = true;
    }

    private static Matrix4d CreateScale(double x, double y, double z)
    {
        return new Matrix4d(
            x, 0.0, 0.0, 0.0,
            0.0, y, 0.0, 0.0,
            0.0, 0.0, z, 0.0,
            0.0, 0.0, 0.0, 1.0);
    }

    public void Render(SKSurface surface, GRContext context)
    {
        int height = (int)surface.Canvas.LocalClipBounds.Height;

        context.Flush();
        context.ResetContext();

        if (!_glInit)
            InitGL();

        if (Shader is null)
            throw new NullReferenceException(nameof(Shader));

        Shader.Use();

        GL.Viewport(
            x: (int)Axes.DataRect.Left,
            y: (int)(height - Axes.DataRect.Bottom),
            width: (int)Axes.DataRect.Width,
            height: (int)Axes.DataRect.Height);

        var xRange = Axes.XAxis.Range;
        var yRange = Axes.YAxis.Range;

        Matrix4d translateD = Matrix4d.CreateTranslation(
            x: -1.0 * (xRange.Min + xRange.Max) / 2,
            y: -1.0 * (yRange.Min + yRange.Max) / 2,
            z: 0.0);

        Matrix4d scaleD = ScatterGL.CreateScale(
            x: 2.0 / (xRange.Max - xRange.Min),
            y: 2.0 / (yRange.Max - yRange.Min),
            z: 1.0);

        Matrix4d normGLD = translateD * scaleD;

        int location = Shader.GetUniformLocation("transform");
        GL.UniformMatrix4(location, true, ref normGLD);
        int colorLocation = Shader.GetUniformLocation("pathColor");
        GL.Uniform4(colorLocation, LineStyle.Color.ToTkColor());

        GL.BindVertexArray(VertexArrayObject);
        GL.DrawArrays(PrimitiveType.LineStrip, 0, VerticesCount);
    }
}
