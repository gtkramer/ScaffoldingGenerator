using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace ScaffoldingGenerator.GUI {
    public class Shader : IDisposable
    {
        public int Handle;
        private bool DisposedValue = false;

        public Shader(string vertexPath, string fragmentPath)
        {
            int vertexShader = CompileShader(vertexPath, ShaderType.VertexShader);
            int fragmentShader = CompileShader(fragmentPath, ShaderType.FragmentShader);
            int Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        private int CompileShader(string shaderPath, ShaderType shaderType) {
            int shader = GL.CreateShader(shaderType);
            using (StreamReader reader = new StreamReader(shaderPath, Encoding.UTF8))
            {
                GL.ShaderSource(shader, reader.ReadToEnd());
            }
            GL.CompileShader(shader);
            string infoLog = GL.GetShaderInfoLog(shader);
            if (infoLog != System.String.Empty) {
                System.Console.WriteLine(infoLog);
            }
            return shader;
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!DisposedValue)
            {
                GL.DeleteProgram(Handle);
                DisposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
