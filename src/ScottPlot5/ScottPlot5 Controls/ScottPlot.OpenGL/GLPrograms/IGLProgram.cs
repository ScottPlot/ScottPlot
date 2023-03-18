using System;

namespace ScottPlot.OpenGL.GLPrograms;

public interface IGLProgram : IDisposable
{
    void Use();
    int GetUniformLocation(string name);
    int GetAttribLocation(string name);
    void GLFinish();
}
