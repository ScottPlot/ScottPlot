using OpenTK;
using OpenTK.Graphics.OpenGL;
using ScottPlot.OpenGL;
using ScottPlot.OpenGL.GLPrograms;
using SkiaSharp;
using System;
#if NETCOREAPP || NET
using OpenTK.Mathematics;
#endif

namespace ScottPlot.Plottables;

/// <summary>
/// This plot type uses an OpenGL shader for rendering.
/// </summary>
public class ScatterGL : Scatter, IPlottableGL, IDisposable
{
    public IPlotControl PlotControl { get; }
    protected int VertexBufferObject;
    protected int VertexArrayObject;
    protected ILinesDrawProgram? LinesProgram;
    protected IMarkersDrawProgram? MarkerProgram;
    protected double[] Vertices;
    protected int VerticesCount;

    protected bool GLHasBeenInitialized = false;

    public GLFallbackRenderStrategy Fallback { get; set; } = GLFallbackRenderStrategy.Software;

    public ScatterGL(IScatterSource data, IPlotControl control) : base(data)
    {
        PlotControl = control;
        var dataPoints = data.GetScatterPoints();
        Vertices = new double[dataPoints.Count * 2];
        for (int i = 0; i < dataPoints.Count; i++)
        {
            Vertices[i * 2] = dataPoints[i].X;
            Vertices[i * 2 + 1] = dataPoints[i].Y;
        }
        VerticesCount = Vertices.Length / 2;
    }

    protected virtual void InitializeGL()
    {
        LinesProgram = new LinesProgram();
        MarkerProgram = new MarkerFillCircleProgram();

        VertexArrayObject = GL.GenVertexArray();
        VertexBufferObject = GL.GenBuffer();
        GL.BindVertexArray(VertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(double), Vertices, BufferUsageHint.StaticDraw);
        GL.VertexAttribLPointer(0, 2, VertexAttribDoubleType.Double, 0, IntPtr.Zero);
        GL.EnableVertexAttribArray(0);
        Vertices = Array.Empty<double>();
        GLHasBeenInitialized = true;
    }

    protected Matrix4d CalcTransform()
    {
        var xRange = Axes.XAxis.Range;
        var yRange = Axes.YAxis.Range;

        Matrix4d translate = Matrix4d.CreateTranslation(
            x: -1.0 * (xRange.Min + xRange.Max) / 2,
            y: -1.0 * (yRange.Min + yRange.Max) / 2,
            z: 0.0);

        Matrix4d scale = Matrix4d.Scale(
            x: 2.0 / (xRange.Max - xRange.Min),
            y: 2.0 / (yRange.Max - yRange.Min),
            z: 1.0);

        return translate * scale;
    }


    public override void Render(RenderPack rp)
    {
        if (PlotControl.GRContext is not null)
        {
            RenderWithOpenGL(rp.Canvas, PlotControl.GRContext);
            return;
        }

        if (Fallback == GLFallbackRenderStrategy.Software)
        {
            rp.Canvas.ClipRect(Axes.DataRect.ToSKRect());
            PixelSize figureSize = new(rp.Canvas.LocalClipBounds.Width, rp.Canvas.LocalClipBounds.Height);
            PixelRect rect = new(0, figureSize.Width, figureSize.Height, 0);
            RenderPack rp1 = new(PlotControl.Plot, rect, rp.Canvas);
            base.Render(rp1);
        }
    }

    protected virtual void RenderWithOpenGL(SKCanvas canvas, GRContext context)
    {
        int height = (int)canvas.LocalClipBounds.Height;

        if (!GLHasBeenInitialized)
            InitializeGL();

        GL.Viewport(
            x: (int)Axes.DataRect.Left,
            y: (int)(height - Axes.DataRect.Bottom),
            width: (int)Axes.DataRect.Width,
            height: (int)Axes.DataRect.Height);

        if (LinesProgram is null)
            throw new NullReferenceException(nameof(LinesProgram));

        LinesProgram.Use();
        LinesProgram.SetTransform(CalcTransform());
        LinesProgram.SetColor(LineStyle.Color.ToTkColor());
        GL.BindVertexArray(VertexArrayObject);
        GL.DrawArrays(PrimitiveType.LineStrip, 0, VerticesCount);

        RenderMarkers();
    }

    protected void RenderMarkers()
    {
        if (MarkerStyle.Shape == MarkerShape.None || MarkerStyle.Size == 0 || MarkerStyle.IsVisible == false)
            return;

        IMarkersDrawProgram? newProgram = MarkerStyle.Shape switch
        {
            MarkerShape.FilledSquare => MarkerProgram is MarkerFillSquareProgram ? null : new MarkerFillSquareProgram(),
            MarkerShape.FilledCircle => MarkerProgram is MarkerFillCircleProgram ? null : new MarkerFillCircleProgram(),
            MarkerShape.OpenCircle => MarkerProgram is MarkerOpenCircleProgram ? null : new MarkerOpenCircleProgram(),
            MarkerShape.OpenSquare => MarkerProgram is MarkerOpenSquareProgram ? null : new MarkerOpenSquareProgram(),
            _ => throw new NotSupportedException($"Marker shape `{MarkerStyle.Shape}` is not supported by GLPlottables"),
        };

        if (newProgram is not null)
        {
            MarkerProgram?.Dispose();
            MarkerProgram = newProgram;
        }

        if (MarkerProgram is null)
            throw new NullReferenceException(nameof(MarkerProgram));

        MarkerProgram.Use();
        MarkerProgram.SetTransform(CalcTransform());
        MarkerProgram.SetMarkerSize(MarkerStyle.Size);
        MarkerProgram.SetFillColor(MarkerStyle.FillColor.ToTkColor());
        MarkerProgram.SetViewPortSize(Axes.DataRect.Width, Axes.DataRect.Height);
        MarkerProgram.SetOutlineColor(MarkerStyle.LineColor.ToTkColor());
        MarkerProgram.SetOpenFactor(1.0f - (float)MarkerStyle.LineWidth * 2 / MarkerStyle.Size);
        GL.BindVertexArray(VertexArrayObject);
        GL.DrawArrays(PrimitiveType.Points, 0, VerticesCount);
    }

    public void GLFinish() => LinesProgram?.GLFinish();
    public void StoreGLState()
    {
        if (PlotControl.GRContext is not null)
            GL.PushAttrib(AttribMask.AllAttribBits);
    }
    public void RestoreGLState()
    {
        if (PlotControl.GRContext is not null)
        {
            PlotControl.GRContext.Flush();
            PlotControl.GRContext.ResetContext();
            GL.PopAttrib();
        }
    }

    #region DISPOSAL

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                GL.Finish();
                LinesProgram?.Dispose();
                LinesProgram = null;
                MarkerProgram?.Dispose();
                MarkerProgram = null;
            }

            CleanupGPUMemory();

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void CleanupGPUMemory()
    {
        if (GLHasBeenInitialized)
        {
            GL.DeleteVertexArray(VertexArrayObject);
            GL.DeleteBuffer(VertexBufferObject);

            GLHasBeenInitialized = false;
        }
    }

    #endregion
}
