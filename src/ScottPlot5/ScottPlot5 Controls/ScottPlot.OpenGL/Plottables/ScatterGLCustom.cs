using OpenTK.Graphics.OpenGL;
using ScottPlot.OpenGL.GLPrograms;
using SkiaSharp;
using System;

namespace ScottPlot.Plottables;

/// <summary>
/// This plot type uses an OpenGL shader for rendering.
/// It extends <see cref="ScatterGL"/> to provide additional customizations.
/// </summary>
public class ScatterGLCustom : ScatterGL
{
    private IMarkersDrawProgram? JoinsProgram;

    public ScatterGLCustom(IScatterSource data, IPlotControl control) : base(data, control)
    {
    }

    protected override void InitializeGL()
    {
        base.InitializeGL();

        LinesProgram = new LinesProgramCustom();
        JoinsProgram = new MarkerFillCircleProgram();
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

        if (LinesProgram is null)
            throw new NullReferenceException(nameof(LinesProgram));

        LinesProgram.Use();
        LinesProgram.SetTransform(CalcTransform());
        LinesProgram.SetColor(LineStyle.Color.ToTkColor());
        LinesProgram.SetViewPortSize(Axes.DataRect.Width, Axes.DataRect.Height);
        LinesProgram.SetLineWidth(LineStyle.Width);

        GL.BindVertexArray(VertexArrayObject);
        GL.DrawArrays(PrimitiveType.LineStrip, 0, VerticesCount);

        // skip joins rendering if they are completely overlapped by markers
        if (MarkerStyle.Size < LineStyle.Width
                || MarkerStyle.Shape == MarkerShape.OpenSquare
                || MarkerStyle.Shape == MarkerShape.OpenCircle
                || MarkerStyle.Shape == MarkerShape.None)
        {
            if (JoinsProgram is null)
                throw new NullReferenceException(nameof(JoinsProgram));

            JoinsProgram.Use();
            JoinsProgram.SetTransform(CalcTransform());
            JoinsProgram.SetFillColor(LineStyle.Color.ToTkColor());
            JoinsProgram.SetViewPortSize(Axes.DataRect.Width, Axes.DataRect.Height);
            JoinsProgram.SetMarkerSize(LineStyle.Width);
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Points, 0, VerticesCount);
        }
        RenderMarkers();
    }
}
