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
                    var wParamStringBuilder = new StringBuilder();
                    GlobalGetAtomName(wMessage.WParam.ToInt32(), wParamStringBuilder, 1024);
                    GlobalDeleteAtom(wMessage.WParam.ToInt32());

                    var lParamStringBuilder = new StringBuilder();
                    GlobalGetAtomName(wMessage.LParam.ToInt32(), lParamStringBuilder, 1024);
                    GlobalDeleteAtom(wMessage.LParam.ToInt32());

                    txtReceiveMessage.Text += $"[{DateTime.Now}]\r\n[WParam]: {wParamStringBuilder} \r\n[LParam]: {lParamStringBuilder} \r\n============\r\n\r\n";
                    break;
            }
            base.WndProc(ref wMessage);
        }
    }
}
