using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace WebView2FixedRuntimeVersion
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeWebView2();
        }

        private async void InitializeWebView2()
        {
            var webView = new WebView2
            {
                CreationProperties = new CoreWebView2CreationProperties
                {
                    BrowserExecutableFolder =
                        $"{Application.StartupPath}\\Microsoft.WebView2.FixedVersionRuntime.112.0.1722.68.x64"
                }
            };
            ((ISupportInitialize) webView).EndInit();
            await webView.EnsureCoreWebView2Async();
            Controls.Add(webView);
            webView.Dock = DockStyle.Fill;
            webView.NavigationCompleted += webView_NavigationCompleted;
            webView.Source = new Uri($"{Application.StartupPath}\\hello.html");
        }

        private void webView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
        }
    }
}
