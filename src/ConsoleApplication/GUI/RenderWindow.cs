using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Mathematics;

namespace AdditiveManufacturing.GUI {
    public class RenderWindow : GameWindow {
        public RenderWindow() : base(GameWindowSettings.Default, NativeWindowSettings.Default) {
            // GameWindowSettings
            RenderFrequency = 60;
            UpdateFrequency = 60;
            // NativeWindowSettings
            IsEventDriven = true;
            Title = "Render Window";
            Size = new Vector2i(640, 640);
        }
    }
}
