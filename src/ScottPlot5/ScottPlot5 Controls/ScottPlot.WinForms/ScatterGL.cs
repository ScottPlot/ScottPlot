using OpenTK;
using OpenTK.Graphics.OpenGL;
using ScottPlot.DataSources;
using ScottPlot.Plottables;
using SkiaSharp;
using System.Linq;

namespace ScottPlot.WinForms
{
    public class ScatterGL : Scatter, IPlottableGL
    {
        private readonly GRContext _context;
        private int VertexBufferObject;
        private int VertexArrayObject;
        private GLShader shader;
        private float[] vertices;
        private int verticesCount;

        private bool _glInit = false;

        public ScatterGL(IScatterSource data, GRContext context) : base(data)
        {
            _context = context;
            vertices = data.GetScatterPoints().Select(p =>
            {
                return new float[] { (float)p.X, (float)p.Y, 0 };
            }).
            SelectMany(t => t).ToArray();
            verticesCount = vertices.Length / 3;
        }

        private void InitGL()
        {
            shader = new GLShader();
            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            vertices = null;
            _glInit = true;
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

            float xTranslation = (float)(-1.0 * (xRange.Min + xRange.Max) / 2);
            float yTranslation = (float)(-1.0 * (yRange.Min + yRange.Max) / 2);
            float xScaling = (float)(2.0 / (xRange.Max - xRange.Min));
            float yScaling = (float)(2.0 / (yRange.Max - yRange.Min));
            Matrix4 translate = Matrix4.CreateTranslation(xTranslation, yTranslation, 0);
            Matrix4 scale = Matrix4.CreateScale(xScaling, yScaling, 1.0f);
            Matrix4 normGL = translate * scale;

            int location = shader.GetUniformLocation("transform");
            GL.UniformMatrix4(location, true, ref normGL);
            int colorLocation = shader.GetUniformLocation("pathColor");
            GL.Uniform4(colorLocation, OpenTK.Graphics.Color4.Blue);

            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.LineStrip, 0, verticesCount);
        }
    }
}
