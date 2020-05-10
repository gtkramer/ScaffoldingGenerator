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
    }
}
