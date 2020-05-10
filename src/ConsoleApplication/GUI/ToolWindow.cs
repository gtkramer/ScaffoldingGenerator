using System;
using Gtk;

namespace AdditiveManufacturing.GUI
{
    class ToolWindow : Window
    {
        public static ToolWindow CreateInstance() {
            Builder builder = new Builder("ToolWindow.glade");
            ToolWindow toolWindow = new ToolWindow(builder);
            toolWindow.Show();
            return toolWindow;
        }

        private ToolWindow(Builder builder) : base(builder.GetObject("ToolWindow").Handle)
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
    }
}
