using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CustomForm
{
    public partial class CustomForm : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private DateTime _lastClick;
        private bool _inDoubleClick;
        private Rectangle _doubleClickArea;
        private TimeSpan _doubleClickMaxTime;
        private Action _doubleClickAction;
        private Timer _clickTimer;
        private Point _windowRestorePoint;
        private int _windowRestoreHeight;
        private int _windowRestoreWidth;

        public CustomForm()
        {
            InitializeComponent();
            InitializeMouseClick();
        }

        private void InitializeMouseClick()
        {
            _doubleClickMaxTime = TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime);

            _clickTimer = new Timer { Interval = SystemInformation.DoubleClickTime };
            _clickTimer.Tick += ClickTimer_Tick;

            _doubleClickAction = FormMaximize;
        }

        private void FormMaximize()
        {
            _windowRestorePoint = Location;
            _windowRestoreHeight = Height;
            _windowRestoreWidth = Width;

            var bounds = Screen.FromHandle(Handle).WorkingArea;
            var xOffset = SystemInformation.HorizontalResizeBorderThickness + SystemInformation.FixedFrameBorderSize.Width;
            var yOffset = SystemInformation.VerticalResizeBorderThickness + SystemInformation.FixedFrameBorderSize.Height;
            bounds.X -= xOffset;
            bounds.Width += (xOffset * 2);
            bounds.Height += yOffset;
            Location = bounds.Location;
            Width = bounds.Width;
            Height = bounds.Height;
            btnWindowRestore.BringToFront();
        }

        private void ClickTimer_Tick(object sender, EventArgs e)
        {
            _inDoubleClick = false;
            _clickTimer.Stop();
        }

        private void pnlTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (_inDoubleClick)
            {
                _inDoubleClick = false;

                var length = DateTime.Now - _lastClick;

                // If double click is valid, respond
                if (!_doubleClickArea.Contains(e.Location) || length >= _doubleClickMaxTime) return;
                _clickTimer.Stop();
                _doubleClickAction();

                return;
            }

            // Double click was invalid, restart 
            _clickTimer.Stop();
            _clickTimer.Start();
            _lastClick = DateTime.Now;
            _inDoubleClick = true;
            _doubleClickArea = new Rectangle(e.Location, SystemInformation.DoubleClickSize);

            btnWindowMaximize.BringToFront();
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void btnWindowMinimize_Click(object sender, EventArgs e)
            => WindowState = FormWindowState.Minimized;
        

        private void btnWindowRestore_Click(object sender, EventArgs e)
        {
            Location = _windowRestorePoint;
            Height = _windowRestoreHeight;
            Width = _windowRestoreWidth;
            btnWindowMaximize.BringToFront();
        }

        private void btnWindowMaximize_Click(object sender, EventArgs e)
            => FormMaximize();

        private void btnWindowClose_Click(object sender, EventArgs e)
            => Close();
    }
}
