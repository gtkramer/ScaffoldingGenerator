using System;
using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Graphics.OpenGL4;

namespace AdditiveManufacturing.GUI
{
    public class RenderWindow : GameWindow
    {
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

        private RenderWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {}

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
    }
}
