using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp
{
    class JsDialogHandler : IJsDialogHandler
    {
        public void OnDialogClosed(IWebBrowser browserControl, IBrowser browser)
        {
            MainWindow.WriteLog(String.Format("{0}", System.Reflection.MethodBase.GetCurrentMethod().Name));
        }

        public bool OnJSBeforeUnload(IWebBrowser browserControl, IBrowser browser, string message, bool isReload, IJsDialogCallback callback)
        {
            MainWindow.WriteLog(String.Format("{0}", System.Reflection.MethodBase.GetCurrentMethod().Name));
            callback.Continue(true, message);
            return true;
        }

        public bool OnJSDialog(IWebBrowser browserControl, IBrowser browser, string originUrl, string acceptLang, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        {
            MainWindow.WriteLog(String.Format("{0}", System.Reflection.MethodBase.GetCurrentMethod().Name));
            callback.Continue(true, messageText);
            return true;
        }

        public void OnResetDialogState(IWebBrowser browserControl, IBrowser browser)
        {
            MainWindow.WriteLog(String.Format("{0}", System.Reflection.MethodBase.GetCurrentMethod().Name));
        }
    }
}
