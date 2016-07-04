using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CefSharp
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        const string line = "---------------------------------------";

        public MainWindow()
        {
            InitializeComponent();

            //browser.ExecuteScriptAsync("",null);

                
        }

        private void WriteLog(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine(line);
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue == ((ProgressBar)sender).Maximum)
            {
                lbStatus.Content = "Loaded";
            }else if (e.NewValue == ((ProgressBar)sender).Minimum)
            {
                lbStatus.Content = "Waiting...";
            }else
            {
                lbStatus.Content = "Loading...";
            }
        }

        private void browser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            WriteLog(String.Format( "Line: {0}\r\nMessage: {1}", e.Line, e.Message));
        }

        private void browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            WriteLog(String.Format("FrameLoadEnd:\r\nBrowser: {0}\r\nFrame: {1}\r\nStatusCode: {2}\r\nUrl:{3}", e.Browser, e.Frame, e.HttpStatusCode, e.Url));            
        }

        private void browser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            WriteLog(String.Format("FrameLoadStart:\r\nBrowser: {0}\r\nFrame: {1}\r\nUrl:{2}", e.Browser, e.Frame, e.Url));            
        }

        private void browser_Initialized(object sender, EventArgs e)
        {
            WriteLog(String.Format("Initialized: {0}", sender.ToString()));            
        }

        private void browser_Loaded(object sender, RoutedEventArgs e)
        {
            WriteLog(String.Format("Loaded:\r\nHandled: {0}\r\nOriginalSource: {1}\r\nRoutedEvent: {2}\r\nSource:{3}", e.Handled, e.OriginalSource, e.RoutedEvent, e.Source));
        }

        private void browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {

            try
            {
                WriteLog(String.Format("LoadingStateChanged:\r\nBrowser: {0}\r\nCanGoBack. {1}\r\nCanGoForward: {2}\r\nCanReload: {3}\r\nIsLoading: {4}", e.Browser, e.CanGoBack, e.CanGoForward, e.CanReload, e.IsLoading));

                this.Dispatcher.Invoke(
                    () =>
                    {
                    lbStatus.Content = browser.IsLoading ? "loading..." : "loaded";

                if (browser.IsLoaded) browser.ShowDevTools();
            });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        private void browser_StatusMessage(object sender, StatusMessageEventArgs e)
        {
            WriteLog(String.Format("StatusMessage:\r\nBrowser: {0}\r\nValue: {1}", e.Browser, e.Value));            
        }

        private void tbBrowser_KeyUp(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbBrowser.Text)) return;

            if (e.Key.Equals(Key.Enter))
            {
                browser.Address = tbBrowser.Text;
            }
        }
    }
}
