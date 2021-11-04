using System;
using System.Windows.Forms;

namespace WinFormWebView2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            webView2.Source = new Uri("https://www.naver.com/");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webView2?.CoreWebView2.ExecuteScriptAsync("document.getElementById('search_btn').click()");
        }

        private void webView2_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            webView2?.CoreWebView2.ExecuteScriptAsync("document.getElementById('query').value = 'test'");
        }
    }
}
