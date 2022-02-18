using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ReceiveWindowMessage
{
    public partial class MainForm : Form
    {
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern uint GlobalGetAtomName(int nAtom, StringBuilder lpBuffer, int nSize);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int GlobalDeleteAtom(int atom);

        private const int WM_USER = 1024;

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message wMessage)
        {
            switch (wMessage.Msg)
            {
                case WM_USER:
                    var sb = new StringBuilder();
                    GlobalGetAtomName(wMessage.LParam.ToInt32(), sb, 1024);
                    GlobalDeleteAtom(wMessage.LParam.ToInt32());
                    txtReceiveMessage.Text += $"[{DateTime.Now}]{sb} \r\n";
                    break;
            }
            base.WndProc(ref wMessage);
        }
    }
}
