using System;
using System.Windows.Forms;

namespace WinFormWebView2.Test3
{
    public partial class FormTest3 : Form
    {
        public FormTest3()
        {
            InitializeComponent();
            webView.Source = new Uri($@"{Application.StartupPath}\Test3\Html\Test3.html");
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = "pdf",
                AddExtension = true,
                FileName = "PDF출력테스트"
            };
            if (dialog.ShowDialog() != DialogResult.OK) return;
            var printSettings = webView.CoreWebView2.Environment.CreatePrintSettings();
            printSettings.ShouldPrintBackgrounds = true;
            printSettings.ScaleFactor = 1;
            printSettings.FooterUri = "꼬리말입력";
            printSettings.ShouldPrintHeaderAndFooter = true;
            printSettings.MarginTop = printSettings.MarginBottom = printSettings.MarginLeft = printSettings.MarginRight = 1;
            webView.CoreWebView2?.PrintToPdfAsync(dialog.FileName, printSettings);
        }

        private async void webView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            await webView.EnsureCoreWebView2Async();
        }
    }
}
