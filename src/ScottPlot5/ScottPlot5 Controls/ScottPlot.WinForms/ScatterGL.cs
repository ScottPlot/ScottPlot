using OpenTK;
using OpenTK.Graphics.OpenGL;
using ScottPlot.DataSources;
using ScottPlot.Plottables;
using SkiaSharp;
using System;
using System.Linq;

namespace ScottPlot.WinForms
{
    public class ScatterGL : Scatter, IPlottableGL
    {
        private readonly GRContext _context;
        private int VertexBufferObject;
        private int VertexArrayObject;
        private GLShader shader;
        private double[] vertices;
        private int verticesCount;

        private bool _glInit = false;

        public ScatterGL(IScatterSource data, GRContext context) : base(data)
        {
            _context = context;
            vertices = data.GetScatterPoints().Select(p =>
            {
                return new double[] { p.X, p.Y };
            }).
            SelectMany(t => t).ToArray();
            verticesCount = vertices.Length / 2;
        }

        private void InitGL()
        {
            shader = new GLShader();
            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(double), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribLPointer(0, 2, VertexAttribDoubleType.Double, 0, IntPtr.Zero);
            GL.EnableVertexAttribArray(0);
            vertices = null;
            _glInit = true;
        }

        private Matrix4d CreateScale(double x, double y, double z)
        {
            return new Matrix4d(
                x, 0.0, 0.0, 0.0,
                0.0, y, 0.0, 0.0,
                0.0, 0.0, z, 0.0,
                0.0, 0.0, 0.0, 1.0);
        }

        public override void Render(SKSurface surface)
        {
            int deviceHeight = (int)surface.Canvas.LocalClipBounds.Height;
            _context.Flush();
            _context.ResetContext();

            if (!_glInit)
                InitGL();

            shader.Use();

            GL.Viewport((int)Axes.DataRect.Left, (int)(deviceHeight - Axes.DataRect.Bottom), (int)Axes.DataRect.Width, (int)Axes.DataRect.Height);

            var xRange = Axes.XAxis.Range;
            var yRange = Axes.YAxis.Range;

            Matrix4d translateD = Matrix4d.CreateTranslation(-1.0 * (xRange.Min + xRange.Max) / 2, -1.0 * (yRange.Min + yRange.Max) / 2, 0.0);
            Matrix4d scaleD = CreateScale(2.0 / (xRange.Max - xRange.Min), 2.0 / (yRange.Max - yRange.Min), 1.0);

            Matrix4d normGLD = translateD * scaleD;

            int location = shader.GetUniformLocation("transform");
            GL.UniformMatrix4(location, true, ref normGLD);
            int colorLocation = shader.GetUniformLocation("pathColor");
            GL.Uniform4(colorLocation, OpenTK.Graphics.Color4.Blue);

            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.LineStrip, 0, verticesCount);
        }
    }
}
