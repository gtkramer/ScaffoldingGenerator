using System;
using Gtk;

namespace AdditiveManufacturing.GUI
{
    class ToolWindow : Window
    {
        public ToolWindow() : this(new Builder("ToolWindow.glade")) { }

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
