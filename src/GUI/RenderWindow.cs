using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace ScaffoldingGenerator.GUI
{
    public class RenderWindow : GameWindow
    {
        public List<int> VertexBufferObjects;
        private Shader Shader;

        public static RenderWindow CreateInstance() {
            GameWindowSettings gameWindowSettings = new GameWindowSettings();
            gameWindowSettings.RenderFrequency = 60;
            gameWindowSettings.UpdateFrequency = 60;
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings();
            nativeWindowSettings.APIVersion = new Version(4, 1);
            nativeWindowSettings.IsEventDriven = true;
            nativeWindowSettings.Title = "Render Window";
            nativeWindowSettings.Size = new Vector2i(640, 640);
            return new RenderWindow(gameWindowSettings, nativeWindowSettings);
        }

        private RenderWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            VertexBufferObjects = new List<int>();
            Shader = new Shader("", "");
        }

        public void AddVbo(float[] vertices) {
            int vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            VertexBufferObjects.Add(vertexBufferObject);
        }

        protected override void OnLoad() {
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            SwapBuffers();
            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs args) {
            GL.Viewport(0, 0, Size.X, Size.Y);
            base.OnResize(args);
        }

        protected override void OnUnload() {
            foreach (int vertexBufferObject in VertexBufferObjects) {
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.DeleteBuffer(vertexBufferObject);
            }
            //Shader.Dispose();
            base.OnUnload();
        }
    }
}
