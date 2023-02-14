using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace ScottPlot.WinForms
{
    public class GLShader : IDisposable
    {
        private int Handle;

        string _vertexShaderSource =
        @"# version 410 core
        layout(location = 0) in dvec2 aPosition;
        uniform dmat4 transform;

        void main()
        {
            dvec4 posd = dvec4(aPosition, 0.0, 1.0);
            dvec4 transformedD = posd * transform;
            gl_Position = vec4(transformedD);
        }";

        string _fragmentShaderSource =
        @"#version 330 core
        out vec4 FragColor;
        uniform vec4 pathColor;

        void main()
        {
            FragColor = pathColor;
        }";

        public GLShader()
        {
            var VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, _vertexShaderSource);

            var FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, _fragmentShaderSource);

            GL.CompileShader(VertexShader);

            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int successVertex);
            if (successVertex == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Debug.WriteLine(infoLog);
            }

            GL.CompileShader(FragmentShader);

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int successFragment);
            if (successFragment == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Debug.WriteLine(infoLog);
            }

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Debug.WriteLine(infoLog);
            }

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
