using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace HackerBot
{
    public class Capture:IDisposable
    {
        private Bitmap _bmp;
       
        IntPtr hwnd = IntPtr.Zero;
        IntPtr hwndChild = IntPtr.Zero;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_COMMAND = 0x111;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_RBUTTONDBLCLK = 0x206;

        private bool disposed = false;
        public Capture()
        {
            hwnd = FindWindow(null, "BlueStacks");

            hwndChild = FindWindowEx(hwnd, IntPtr.Zero, "WindowsForms10.Window.8.app.0.2fc056_r6_ad1", "BlueStacks Android PluginAndroid");

            RECT rct;

            if (!GetWindowRect(hwndChild, out rct))
            {
                return;
            }
            Rectangle myRect = new Rectangle();
            myRect.X = rct.Left;
            myRect.Y = rct.Top;
            myRect.Width = rct.Right - rct.Left;
            myRect.Height = rct.Bottom - rct.Top;


            if(rct.Top<0)
            {
                ShowWindow(hwnd, 1);
            }
            MoveWindow(hwnd, 0, 0, 1560, 885, true);

            _bmp = new Bitmap(1560, 885, PixelFormat.Format32bppArgb);

            Graphics gfxBmp = Graphics.FromImage(_bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(hwnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetCursorPos(int x,int y);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetCursor(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        public Bitmap CaptureScreen(Point f,Size size)
        {
            Rectangle section = new Rectangle(f, size);

            Bitmap CroppedImage = CropImage(_bmp, section);

            return CroppedImage;
        }
        private Bitmap CropImage(Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
                return bitmap;
            }
        }

        public Color GetPixel(Bitmap source)
        {
            Color c = source.GetPixel(0, 0);

            return c;
        }



        public void LeftClick(Point point)
        {
            SendMessage(hwndChild, WM_LBUTTONDOWN, IntPtr.Zero, MakeLParam(point.X, point.Y));
            SendMessage(hwndChild, WM_LBUTTONUP, IntPtr.Zero, MakeLParam(point.X, point.Y));
            
        }


        public void ExecSendMessage()
        {
            //SendMessage(hwndChild, WM_LBUTTONDOWN, 0x0001, MAKELPARAM(x,y));
          
            //SetCursor(window.);
            //SetCursorPos(100, 100);
            //SendMessage(window, WM_LBUTTONDOWN, IntPtr.Zero, IntPtr.Zero);
            //SendMessage(window)
        }
        IntPtr MakeLParam(int x, int y) => (IntPtr)((y << 16) | (x & 0xFFFF));


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                    _bmp.Dispose();
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here. 
                // If disposing is false, 
                // only the following code is executed.
                hwnd = IntPtr.Zero;
                hwndChild = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;

            }
        }
        ~Capture()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
    }
}
