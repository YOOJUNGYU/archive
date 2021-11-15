using System;
using System.Windows.Forms;

namespace LogExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, System.EventArgs e)
        {
            try
            {
                var test1 = 123;
                var test = 0;
                var result = test1 / test;
            }
            catch (Exception ex)
            {
                LogService.Instance.Error(ex);
            }
        }
    }
}
