using System;
using System.Windows.Forms;

namespace WinFormWebView2.Test2
{
    public partial class FormTest2 : Form
    {
        public FormTest2()
        {
            InitializeComponent();

            // Test2.html 불러오기
            webView.Source = new Uri($@"{Application.StartupPath}\Test2\Test\index.html");
        }

        private async void webView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            await webView.EnsureCoreWebView2Async();
            // Test2.html과 bridge 연결
            webView.CoreWebView2.AddHostObjectToScript("bridge", new Bridge());
            webView.CoreWebView2?.ExecuteScriptAsync($"document.getElementById('test-input').value = '{"TEST!!!!"}'");
        }
    }
}
