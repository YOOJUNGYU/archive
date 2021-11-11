using System;
using System.Windows.Forms;

namespace WinFormWebView2
{
    public partial class MainForm : Form
    {
        private readonly FormTest1 _formTest1 = new FormTest1();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            _formTest1.TopLevel = false;
            _formTest1.FormBorderStyle = FormBorderStyle.None;
            _formTest1.Dock = DockStyle.Fill;
            pnlForm.Controls.Add(_formTest1);
            _formTest1.BringToFront();
            _formTest1.Show();
        }
    }
}
