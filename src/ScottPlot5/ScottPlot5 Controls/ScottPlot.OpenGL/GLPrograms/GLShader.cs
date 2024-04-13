using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace ScottPlot.OpenGL.GLPrograms;

public class GLShader
{
    private readonly int Handle;
    private bool active = false;

    public GLShader(ShaderType type, string? sourceCode)
    {
        if (sourceCode is null)
            return;

        Handle = GL.CreateShader(type);
        active = true;
        Compile(sourceCode);
    }

    public void AttachToProgram(int programHandle)
    {
        if (!active)
            return;

        GL.AttachShader(programHandle, Handle);
    }

    public void DetachFromProgram(int programHandle)
    {
        if (!active)
            return;

        GL.DetachShader(programHandle, Handle);
        GL.DeleteShader(Handle);
        active = false;
    }

    private void Compile(string sourceCode)
    {
        GL.ShaderSource(Handle, sourceCode);
        GL.CompileShader(Handle);
        GL.GetShader(Handle, ShaderParameter.CompileStatus, out int successVertex);
        if (successVertex == 0)
        {
            string infoLog = GL.GetShaderInfoLog(Handle);
            Debug.WriteLine(infoLog);
            active = false;
        }
    }
}
