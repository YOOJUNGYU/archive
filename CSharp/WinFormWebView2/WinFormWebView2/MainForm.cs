using System;
using System.Windows.Forms;
using WinFormWebView2.Test1;
using WinFormWebView2.Test2;

namespace WinFormWebView2
{
    public partial class MainForm : Form
    {
        private readonly FormTest1 _formTest1 = new FormTest1();
        private readonly FormTest2 _formTest2 = new FormTest2();

        public MainForm()
        {
            InitializeComponent();
        }

        private void LoadForm(Form form)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            if(pnlForm.Controls.Find(form.Name, true).Length <= 0)
                pnlForm.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }

        private void btnTest1_Click(object sender, EventArgs e)
            => LoadForm(_formTest1);
        

        private void btnTest2_Click(object sender, EventArgs e)
            => LoadForm(_formTest2);
    }
}
