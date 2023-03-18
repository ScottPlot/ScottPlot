using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace ScottPlot.OpenGL.GLPrograms;

public abstract class GLProgramBase : IGLProgram
{
    private readonly int Handle;

    protected virtual string? VertexShaderSource => null;
    protected virtual string? GeometryShaderSource => null;
    protected virtual string? FragmentShaderSource => null;

    public GLProgramBase()
    {

        var VertexShader = new GLShader(ShaderType.VertexShader, VertexShaderSource);
        var GeometryShader = new GLShader(ShaderType.GeometryShader, GeometryShaderSource);
        var FragmentShader = new GLShader(ShaderType.FragmentShader, FragmentShaderSource);

        Handle = GL.CreateProgram();

        VertexShader.AttachToProgram(Handle);
        GeometryShader.AttachToProgram(Handle);
        FragmentShader.AttachToProgram(Handle);

        GL.LinkProgram(Handle);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(Handle);
            Debug.WriteLine(infoLog);
        }

        VertexShader.DetachFromProgram(Handle);
        GeometryShader.DetachFromProgram(Handle);
        FragmentShader.DetachFromProgram(Handle);
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(Handle, attribName);
    }

    public int GetUniformLocation(string attribName)
    {
        return GL.GetUniformLocation(Handle, attribName);
    }

    private bool disposedValue = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(Handle);

            disposedValue = true;
        }
    }

    public void GLFinish() => GL.Finish();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
