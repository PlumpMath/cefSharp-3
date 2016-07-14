using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp
{
    class LifeSpanHandler : ILifeSpanHandler
    {
        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            MainWindow.WriteLog(String.Format("{0}", System.Reflection.MethodBase.GetCurrentMethod().Name));
            return true;
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            MainWindow.WriteLog(String.Format("{0}", System.Reflection.MethodBase.GetCurrentMethod().Name));
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
            MainWindow.WriteLog(String.Format("{0}", System.Reflection.MethodBase.GetCurrentMethod().Name));
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            MainWindow.WriteLog(String.Format("{0}", System.Reflection.MethodBase.GetCurrentMethod().Name));
            newBrowser = browserControl;
            return true;
        }
    }
}
