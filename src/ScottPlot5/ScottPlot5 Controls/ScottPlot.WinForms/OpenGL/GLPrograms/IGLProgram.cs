using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.WinForms.OpenGL.GLPrograms
{
    public interface IGLProgram : IDisposable
    {
        void Use();
        int GetUniformLocation(string name);
        int GetAttribLocation(string name);
        void GLFinish();
    }
}
