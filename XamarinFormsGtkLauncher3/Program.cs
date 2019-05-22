using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;
using CrossLibrary;

namespace XamarinFormsGtkLauncher3
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Gtk.Application.Init();
            Forms.Init();
            var app = new App();
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("XamarinFormsGtkLauncher");
            window.Show();
            Gtk.Application.Run();
        }
    }
}
