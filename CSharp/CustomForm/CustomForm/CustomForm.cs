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

        private enum CustomWindowState
        {
            Restored,
            Maximized
        }

        #region titleBar doubleClick
        private DateTime _lastClick;
        private bool _inDoubleClick;
        private Rectangle _doubleClickArea;
        private TimeSpan _doubleClickMaxTime;
        private Action _doubleClickMaximize;
        private Action _doubleClickRestore;
        private Timer _clickTimer;
        #endregion

        #region window State
        private CustomWindowState _customWindowState;
        private Point _windowRestorePoint;
        private int _windowRestoreHeight;
        private int _windowRestoreWidth;
        #endregion

        #region resize
        private const int BorderThickness = 5;
        private bool _onMinimumSize;
        private bool _onBorderRight;
        private bool _onBorderLeft;
        private bool _onBorderTop;
        private bool _onBorderBottom;
        private bool _onCornerTopRight;
        private bool _onCornerTopLeft;
        private bool _onCornerBottomRight;
        private bool _onCornerBottomLeft;
        private bool _movingRight;
        private bool _movingLeft;
        private bool _movingTop;
        private bool _movingBottom;
        private bool _movingCornerTopRight;
        private bool _movingCornerTopLeft;
        private bool _movingCornerBottomRight;
        private bool _movingCornerBottomLeft;
        #endregion

        public CustomForm()
        {
            InitializeComponent();
            InitializeMouseClick();
        }

        private void InitializeMouseClick()
        {
            _customWindowState = CustomWindowState.Restored;
            _doubleClickMaxTime = TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime);

            _clickTimer = new Timer { Interval = SystemInformation.DoubleClickTime };
            _clickTimer.Tick += ClickTimer_Tick;

            _doubleClickMaximize = FormMaximize;
            _doubleClickRestore = FormRestore;
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
            _customWindowState = CustomWindowState.Maximized;
        }

        private void FormRestore()
        {
            Location = _windowRestorePoint;
            Height = _windowRestoreHeight;
            Width = _windowRestoreWidth;
            btnWindowMaximize.BringToFront();
            _customWindowState = CustomWindowState.Restored;
        }

        private void StartResize()
        {
            if (_movingCornerTopRight)
            {
                Width = Cursor.Position.X - Location.X;
                Height = Location.Y - Cursor.Position.Y + Height;
                Location = new Point(Location.X, Cursor.Position.Y);
            }
            else if (_movingCornerTopLeft)
            {
                Width = Width + Location.X - Cursor.Position.X;
                Location = new Point(Cursor.Position.X, Location.Y);
                Height = Height + Location.Y - Cursor.Position.Y;
                Location = new Point(Location.X, Cursor.Position.Y);
            }
            else if (_movingCornerBottomRight)
            {
                Width = Cursor.Position.X - Location.X;
                Height = Cursor.Position.Y - Location.Y;
            }
            else if (_movingCornerBottomLeft)
            {
                Width = Width + Location.X - Cursor.Position.X;
                Height = Cursor.Position.Y - Location.Y;
                Location = new Point(Cursor.Position.X, Location.Y);
            }
            else if (_movingRight)
            {
                Width = Cursor.Position.X - Location.X;
            }
            else if (_movingLeft)
            {
                Width = Width + Location.X - Cursor.Position.X;
                Location = new Point(Cursor.Position.X, Location.Y);
            }
            else if (_movingTop)
            {
                Height = Height + Location.Y - Cursor.Position.Y;
                Location = new Point(Location.X, Cursor.Position.Y);
            }
            else if (_movingBottom)
            {
                Height = Cursor.Position.Y - Location.Y;
            }
        }

        private void StopResize()
        {
            _movingRight = false;
            _movingLeft = false;
            _movingTop = false;
            _movingBottom = false;
            _movingCornerTopRight = false;
            _movingCornerTopLeft = false;
            _movingCornerBottomRight = false;
            _movingCornerBottomLeft = false;
            Cursor = Cursors.Default;
            System.Threading.Thread.Sleep(300);
            _onMinimumSize = false;
        }

        private void OnResizeOff()
        {
            _onBorderRight = false;
            _onBorderLeft = false;
            _onBorderTop = false;
            _onBorderBottom = false;
            _onCornerTopRight = false;
            _onCornerTopLeft = false;
            _onCornerBottomRight = false;
            _onCornerBottomLeft = false;
            Cursor = Cursors.Default;
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
                if (!_doubleClickArea.Contains(e.Location) || length >= _doubleClickMaxTime) return;
                _clickTimer.Stop();
                switch (_customWindowState)
                {
                    case CustomWindowState.Restored:
                        _doubleClickMaximize();
                        break;
                    case CustomWindowState.Maximized:
                        _doubleClickRestore();
                        break;
                }
                return;
            }
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
            => FormRestore();

        private void btnWindowMaximize_Click(object sender, EventArgs e)
            => FormMaximize();

        private void btnWindowClose_Click(object sender, EventArgs e)
            => Close();

        private void CustomForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _movingRight = _onBorderRight;
            _movingLeft = _onBorderLeft;
            _movingTop = _onBorderTop;
            _movingBottom = _onBorderBottom;
            _movingCornerTopRight = _onCornerTopRight;
            _movingCornerTopLeft = _onCornerTopLeft;
            _movingCornerBottomRight = _onCornerBottomRight;
            _movingCornerBottomLeft = _onCornerBottomLeft;
        }

        private void CustomForm_MouseUp(object sender, MouseEventArgs e)
            => StopResize();

        private void CustomForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_customWindowState == CustomWindowState.Maximized) { return; }

            if (Width <= MinimumSize.Width) { Width = MinimumSize.Width + 5; _onMinimumSize = true; }
            if (Height <= MinimumSize.Height) { Height = MinimumSize.Height + 5; _onMinimumSize = true; }
            if (_onMinimumSize) { StopResize(); } else { StartResize(); }

            if ((Cursor.Position.X >= Location.X + Width - BorderThickness)
                & (Cursor.Position.Y >= Location.Y + BorderThickness)
                & (Cursor.Position.Y <= Location.Y + Height - BorderThickness))
            { Cursor = Cursors.SizeWE; _onBorderRight = true; }

            else if ((Cursor.Position.X <= Location.X + BorderThickness)
                & (Cursor.Position.Y >= Location.Y + BorderThickness)
                & (Cursor.Position.Y <= Location.Y + Height - BorderThickness))
            { Cursor = Cursors.SizeWE; _onBorderLeft = true; }

            else if ((Cursor.Position.Y <= Location.Y + BorderThickness)
                & (Cursor.Position.X >= Location.X + BorderThickness)
                & (Cursor.Position.X <= Location.X + Width - BorderThickness))
            { Cursor = Cursors.SizeNS; _onBorderTop = true; }

            else if ((Cursor.Position.Y >= Location.Y + Height - BorderThickness)
                & (Cursor.Position.X >= Location.X + BorderThickness)
                & (Cursor.Position.X <= Location.X + Width - BorderThickness))
            { Cursor = Cursors.SizeNS; _onBorderBottom = true; }

            else if (Cursor.Position.X >= Location.X + Width - BorderThickness & Cursor.Position.X <= Location.X + Width
                & Cursor.Position.Y >= Location.Y & Cursor.Position.Y <= Location.Y + BorderThickness)
            { Cursor = Cursors.SizeNESW; _onCornerTopRight = true; }

            else if (Cursor.Position.X >= Location.X & Cursor.Position.X <= Location.X + BorderThickness
                     & Cursor.Position.Y >= Location.Y & Cursor.Position.Y <= Location.Y + BorderThickness)
            { Cursor = Cursors.SizeNWSE; _onCornerTopLeft = true; }

            else if (Cursor.Position.X >= Location.X + Width - BorderThickness & Cursor.Position.X <= Location.X + Width 
                     & Cursor.Position.Y >= Location.Y + Height - 5 & Cursor.Position.Y <= Location.Y + Height)
            { Cursor = Cursors.SizeNWSE; _onCornerBottomRight = true; }

            else if ((Cursor.Position.X == Location.X)
                & (Cursor.Position.Y == Location.Y + Height - 1))
            { Cursor = Cursors.SizeNESW; _onCornerBottomLeft = true; }

            else
                OnResizeOff();
        }

        private void pnlMain_MouseMove(object sender, MouseEventArgs e)
            => OnResizeOff();

        private void pnlTitleBar_MouseMove(object sender, MouseEventArgs e)
            => OnResizeOff();
    }
}
