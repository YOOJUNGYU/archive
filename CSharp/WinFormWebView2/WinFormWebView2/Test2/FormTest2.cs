using System;
using System.Windows.Forms;

namespace WinFormWebView2.Test2
{
    public partial class FormTest2 : Form
    {
        public FormTest2()
        {
            InitializeComponent();
            webView.Source = new Uri($@"{Application.StartupPath}\Test2\Html\Test2.html");
        }

        private void webView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            webView.CoreWebView2.AddHostObjectToScript("bridge", new Bridge());
        }
    }
}
