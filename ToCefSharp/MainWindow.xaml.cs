using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace CefSharp
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void WriteLog(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("---------------------------------------");
        }

        public MainWindow()
        {
            InitializeComponent();
            //browser.RequestHandler = new RequestHandler();                  
            var obj = this.CreateNetObject();
            //el registro se debe efectuar antes de inicializar le cefSharp. No se puede hacer en el win_loaded
            this.RegisterJsObject(obj);            
        }

        private Object CreateNetObject()
        {
            return new MyNetObject("Voro", 31,"yo");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WriteLog(String.Format("{0}", System.Reflection.MethodBase.GetCurrentMethod().Name));
        }       

        private void ExecuteJavaScript(string methodName,string jsonData)
        {
            //TODO 13/07/2016: pasar un array de objetos como argumento en vez de un json. De todas formas, si se va a pasar
            //un objeto, igual vale la pena serializarlo y pasarselo a la web como json
            try
            {
                browser.ExecuteScriptAsync(String.Format(methodName+ "({0})",jsonData));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void RegisterJsObject(dynamic myNetObject)
        {
            try
            {
                browser.RegisterAsyncJsObject(myNetObject.WebName, myNetObject, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        #region BROWSER EVENTS

        private void browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            WriteLog(String.Format("{3}:\r\n\t{2}: {1} --> {0}", e.NewValue,e.OldValue,e.Property, System.Reflection.MethodBase.GetCurrentMethod().Name));            
        }

        private void browser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            WriteLog(String.Format( "{2}\r\n\tLine: {0}\r\n\tMessage: {1}", e.Line, e.Message,System.Reflection.MethodBase.GetCurrentMethod().Name));
        }

        private void browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            WriteLog(String.Format("{4}:\r\nBrowser: {0}\r\nFrame: {1}\r\nStatusCode: {2}\r\nUrl:{3}", e.Browser, e.Frame, e.HttpStatusCode, e.Url, System.Reflection.MethodBase.GetCurrentMethod().Name));
            //hasta que no este cargado el DOM no se pueden ejecutar funciones javaScrip            
            this.ExecuteJavaScript("showFromCS", "{\"Name\":\"Salva\"}");
        }       

        private void browser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            WriteLog(String.Format("{3}:\r\nBrowser: {0}\r\nFrame: {1}\r\nUrl:{2}", e.Browser, e.Frame, e.Url,System.Reflection.MethodBase.GetCurrentMethod().Name));            
        }

        private void browser_Initialized(object sender, EventArgs e)
        {
            WriteLog(String.Format("Initialized: {0}", sender.ToString()));            
        }

        private void browser_Loaded(object sender, RoutedEventArgs e)
        {
            WriteLog(String.Format("{4}:\r\nHandled: {0}\r\nOriginalSource: {1}\r\nRoutedEvent: {2}\r\nSource:{3}", e.Handled, e.OriginalSource, e.RoutedEvent, e.Source, System.Reflection.MethodBase.GetCurrentMethod().Name));           
        }        

        private void browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            try
            {
                WriteLog(String.Format("{5}:\r\nBrowser: {0}\r\nCanGoBack. {1}\r\nCanGoForward: {2}\r\nCanReload: {3}\r\nIsLoading: {4}", e.Browser, e.CanGoBack, e.CanGoForward, e.CanReload, e.IsLoading, System.Reflection.MethodBase.GetCurrentMethod().Name));

                this.Dispatcher.Invoke(
                    () =>
                    {
                    lbStatus.Content = browser.IsLoading ? "loading..." : "loaded";                
            });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        private void browser_StatusMessage(object sender, StatusMessageEventArgs e)
        {
            WriteLog(String.Format("{2}:\r\nBrowser: {0}\r\nValue: {1}", e.Browser, e.Value, System.Reflection.MethodBase.GetCurrentMethod().Name));            
        }

        private void browser_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            WriteLog(String.Format("{0}:\r\n\t{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.ToString()));
        }

        private void browser_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            WriteLog(String.Format("{0}:\r\n\t{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.ToString()));
        }

        #endregion

        #region MENU EVENTS

        private void tbBrowser_KeyUp(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbBrowser.Text)) return;

            if (e.Key.Equals(Key.Enter))
            {
                browser.Address = tbBrowser.Text;
            }
        }

        private void DevTools_Click(object sender, RoutedEventArgs e)
        {
            this.ShowDevTools();
        }

        private void ShowDevTools()
        {
            WriteLog(String.Format("{0}...", System.Reflection.MethodBase.GetCurrentMethod().Name));
            if (browser.IsLoaded) browser.ShowDevTools();
        }        

        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
            this.StartHtmlTest();
        }

        private void StartHtmlTest()
        {
            WriteLog(String.Format("{0}", System.Reflection.MethodBase.GetCurrentMethod().Name));
            string myHtml = File.ReadAllText("HtmlTest.html");
            browser.LoadHtml(myHtml, "http://www.example.com/");
            //browser.Address = "http://wwww.texyon.com";
            
        }



        #endregion

        #region MyCommands

        private void GoForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {            
            e.CanExecute = (browser!=null) && browser.CanGoForward;
        }

        private void GoBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (browser != null) && browser.CanGoBack;
        }

        private void GoForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            browser.Forward();
        }

        private void GoBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            browser.Back();
        }

        #endregion
    }

    public class MyNetObject
    {
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string WebName { get; private set; }

        public MyNetObject(string name, int age, string webName)
        {
            this.Name = name;
            this.Age = age;
            this.WebName = webName;
        }
        public void showData()
        {
            MessageBox.Show(String.Format("{0}\r\n\tName: {1}\r\n\tAge: {2}\r\n",this.WebName, this.Name, this.Age));
        }

        public void loginTexyon(string email, string password)
        {
            MessageBox.Show(String.Format("Email: {0}\r\nPassword: {1}", email,password));
        }
    }
    
    public static class Commands
    {
        public static readonly RoutedUICommand GoForward = new RoutedUICommand(
            "fasdfa",
            "asdfadsf",
            typeof(Commands));

        public static readonly RoutedUICommand GoBack = new RoutedUICommand(
           "asdf",
           "asdfa",
           typeof(Commands));
    }   
}
