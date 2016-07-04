using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CefSharp
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

            var settings = new CefSettings();

            settings.EnableInternalPdfViewerOffScreen();

            settings.CefCommandLineArgs.Add("disable-gpu","1");

            Cef.Initialize(settings, shutdownOnProcessExit: true, performDependencyCheck: true);
        }
    }
}
