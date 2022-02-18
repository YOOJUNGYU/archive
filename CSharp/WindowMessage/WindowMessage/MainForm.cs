using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SendWindowMessage
{
    public partial class MainForm : Form
    {
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int GlobalAddAtom(string lpString);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private const int WM_USER = 1024;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                // 프로세스 명칭으로 프로세스를 가져온다.
                var processes = Process.GetProcessesByName("ReceiveWindowMessage");
                if (processes.Length > 0)
                {
                    SendMessage(processes[0].MainWindowHandle, WM_USER, 0, GlobalAddAtom(txtSendMessage.Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
