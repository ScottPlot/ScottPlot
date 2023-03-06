using OpenTK.Graphics.OpenGL;
using ScottPlot.Control;
using ScottPlot.DataSources;
using ScottPlot.WinForms.OpenGL.GLPrograms;
using SkiaSharp;
using System;

namespace ScottPlot.Plottables;

public class ScatterGLCustomWidth : ScatterGL
{
    private IMarkersDrawProgram? joinsProgram;

    public ScatterGLCustomWidth(IScatterSource data, IPlotControl control) : base(data, control)
    {
    }

    protected override void InitializeGL()
    {
        linesProgram = new CustomLinesProgram();
        joinsProgram = new MarkerFillCircleProgram();
        markerProgram = new MarkerFillCircleProgram();

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

    protected override void RenderWithOpenGL(SKSurface surface, GRContext context)
    {
        int height = (int)surface.Canvas.LocalClipBounds.Height;

        context.Flush();
        context.ResetContext();

        if (!GLHasBeenInitialized)
            InitializeGL();


        GL.Viewport(
            x: (int)Axes.DataRect.Left,
            y: (int)(height - Axes.DataRect.Bottom),
            width: (int)Axes.DataRect.Width,
            height: (int)Axes.DataRect.Height);

        if (linesProgram is null)
            throw new NullReferenceException(nameof(linesProgram));

        linesProgram.Use();
        linesProgram.SetTransform(CalcTransform());
        linesProgram.SetColor(LineStyle.Color.ToTkColor());
        linesProgram.SetViewPortSize(Axes.DataRect.Width, Axes.DataRect.Height);
        linesProgram.SetLineWidth(LineStyle.Width);

        GL.BindVertexArray(VertexArrayObject);
        GL.DrawArrays(PrimitiveType.LineStrip, 0, VerticesCount);

        // Draw joins only if they bigger than markers
        if (MarkerStyle.Size < LineStyle.Width 
                || MarkerStyle.Shape == MarkerShape.OpenSquare 
                || MarkerStyle.Shape == MarkerShape.OpenCircle 
                || MarkerStyle.Shape == MarkerShape.None)
        {
            if (joinsProgram is null)
                throw new NullReferenceException(nameof(joinsProgram));

            joinsProgram.Use();
            joinsProgram.SetTransform(CalcTransform());
            joinsProgram.SetFillColor(LineStyle.Color.ToTkColor());
            joinsProgram.SetViewPortSize(Axes.DataRect.Width, Axes.DataRect.Height);
            joinsProgram.SetMarkerSize(LineStyle.Width);
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Points, 0, VerticesCount);
        }
        RenderMarkers();
    }
}
