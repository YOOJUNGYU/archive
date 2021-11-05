using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private int _oldLocationX;
        private int _oldLocationY;
        private int _oldWidth;
        private int _oldHeight;
        private bool _resizeRight;
        private bool _resizeLeft;
        private bool _resizeTop;
        private bool _resizeBottom;
        #endregion

        public CustomForm()
        {
            InitializeComponent();
            InitializeMouseClick();
            LoadSetting();
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

        private void LoadSetting()
        {
            var savedLocation = new Point(Properties.Settings.Default.FormX, Properties.Settings.Default.FormY);
            Width = Properties.Settings.Default.FormWidth;
            Height = Properties.Settings.Default.FormHeight;
            Location = IsLocationInWorkingArea(savedLocation, Width, Height) ? savedLocation : new Point(0, 0);
            var bound = Screen.FromHandle(Handle).WorkingArea;
            if (Width < bound.Width || Height < bound.Height) return;
            btnWindowRestore.BringToFront();
            _customWindowState = CustomWindowState.Maximized;
        }

        private static bool IsLocationInWorkingArea(Point location, int width, int height)
        {
            const int tolerance = 10;
            var leftX = location.X + tolerance;
            var topY = location.Y + tolerance;
            var rightX = location.X + width - tolerance;
            var bottomY = location.Y + height - tolerance;

            var points = new List<Point>
            {
                new Point(leftX, topY),
                new Point(rightX, topY),
                new Point(leftX, bottomY),
                new Point(rightX,bottomY)
            };

            var isLocationInWorkingArea = true;
            foreach (var screen in Screen.AllScreens)
            {
                isLocationInWorkingArea = true;
                foreach (var point in points)
                {
                    if (!screen.WorkingArea.Contains(point))
                        isLocationInWorkingArea = false;
                }
                if (isLocationInWorkingArea) break;
            }

            return isLocationInWorkingArea;
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
        {
            Properties.Settings.Default.FormX = Location.X;
            Properties.Settings.Default.FormY = Location.Y;
            Properties.Settings.Default.FormHeight = Height;
            Properties.Settings.Default.FormWidth = Width;
            Properties.Settings.Default.Save();
            Close();
        }

        #region Resize
        private void pnlResizeBorder_MouseLeave(object sender, EventArgs e)
            => Cursor = Cursors.Default;
        

        #region ResizeTop
        private void pnlResizeBorderTop_MouseHover(object sender, EventArgs e)
            => Cursor = Cursors.SizeNS;

        private void pnlResizeBorderTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _resizeTop = true;
            _oldLocationY = Location.Y;
            _oldHeight = Height;
        }

        private void pnlResizeBorderTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _resizeTop = false;
            Cursor = Cursors.Default;
        }

        private void pnlResizeBorderTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_resizeTop) return;
            Height = _oldHeight + _oldLocationY - Cursor.Position.Y;
            if (Height <= MinimumSize.Height) return;
            Location = new Point(Location.X, Cursor.Position.Y);
        }
        #endregion

        #region ResizeBottom
        private void pnlResizeBorderBottom_MouseHover(object sender, EventArgs e)
            => Cursor = Cursors.SizeNS;

        private void pnlResizeBorderBottom_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _resizeBottom = true;
        }

        private void pnlResizeBorderBottom_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _resizeBottom = false;
            Cursor = Cursors.Default;
        }

        private void pnlResizeBorderBottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_resizeBottom) return;
            Height = Cursor.Position.Y - Location.Y;
        }
        #endregion

        #region ResizeLeft
        private void pnlResizeBorderLeft_MouseHover(object sender, EventArgs e)
            => Cursor = Cursors.SizeWE;

        private void pnlResizeBorderLeft_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _resizeLeft = true;
            _oldLocationX = Location.X;
            _oldWidth = Width;
        }

        private void pnlResizeBorderLeft_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _resizeLeft = false;
            Cursor = Cursors.Default;
        }

        private void pnlResizeBorderLeft_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_resizeLeft) return;
            Width = _oldWidth + _oldLocationX - Cursor.Position.X;
            if (Width <= MinimumSize.Width) return;
            Location = new Point(Cursor.Position.X, Location.Y);
        }

        #endregion

        #region ResizeRight
        private void pnlResizeBorderRight_MouseHover(object sender, EventArgs e)
            => Cursor = Cursors.SizeWE;

        private void pnlResizeBorderRight_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _resizeRight = true;
        }

        private void pnlResizeBorderRight_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _resizeRight = false;
            Cursor = Cursors.Default;
        }

        private void pnlResizeBorderRight_MouseMove(object sender, MouseEventArgs e)
        {
            if(!_resizeRight) return;
            Width = Cursor.Position.X - Location.X;
        }
        #endregion

        #endregion
    }
}
