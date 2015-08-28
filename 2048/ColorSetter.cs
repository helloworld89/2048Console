using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace _2048
{
    class ColorSetter
    {
        public ColorSetter()
        {
            //BColor = Color.LightBlue;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X;
            internal short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SMALL_RECT
        {
            internal short Left;
            internal short Top;
            internal short Right;
            internal short Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct COLORREF
        {
            internal uint ColorDWORD;

            internal COLORREF(Color color)
            {
                ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
            }

            internal COLORREF(uint r, uint g, uint b)
            {
                ColorDWORD = r + (g << 8) + (b << 16);
            }

            internal Color GetColor()
            {
                return Color.FromArgb((int)(0x000000FFU & ColorDWORD),
                                      (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
            }

            internal void SetColor(Color color)
            {
                ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct CONSOLE_SCREEN_BUFFER_INFO_EX
        {
            internal int cbSize;
            internal COORD dwSize;
            internal COORD dwCursorPosition;
            internal ushort wAttributes;
            internal SMALL_RECT srWindow;
            internal COORD dwMaximumWindowSize;
            internal ushort wPopupAttributes;
            internal bool bFullscreenSupported;
            internal COLORREF black;
            internal COLORREF darkBlue;
            internal COLORREF darkGreen;
            internal COLORREF darkCyan;
            internal COLORREF darkRed;
            internal COLORREF darkMagenta;
            internal COLORREF darkYellow;
            internal COLORREF gray;
            internal COLORREF darkGray;
            internal COLORREF blue;
            internal COLORREF green;
            internal COLORREF cyan;
            internal COLORREF red;
            internal COLORREF magenta;
            internal COLORREF yellow;
            internal COLORREF white;
        }

        const int STD_OUTPUT_HANDLE = -11;                                        // per WinBase.h
        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);    // per WinBase.h

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX currentScreen);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX currentScreen);

        // Set a specific console color to an RGB color
        // The default console colors used are gray (foreground) and black (background)
        public static int SetColor(ConsoleColor consoleColor, Color targetColor)
        {
            return SetColor(consoleColor, targetColor.R, targetColor.G, targetColor.B);
        }

        public static int SetColor(ConsoleColor color, uint r, uint g, uint b)
        {
            CONSOLE_SCREEN_BUFFER_INFO_EX currentScreen = new CONSOLE_SCREEN_BUFFER_INFO_EX();
            currentScreen.cbSize = (int)Marshal.SizeOf(currentScreen);                    // 96 = 0x60
            IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);    // 7
            if (handle == INVALID_HANDLE_VALUE)
            {
                return Marshal.GetLastWin32Error();
            }
            bool brc = GetConsoleScreenBufferInfoEx(handle, ref currentScreen);
            if (!brc)
            {
                return Marshal.GetLastWin32Error();
            }

            //var a = currentScreen.GetType().GetRuntimeFields()
            //    .Single(x => { return x.Name.ToLower() == color.ToString().ToLower(); });
            // a.SetValue(currentScreen, new COLORREF(r, g, b));

            switch (color)
            {
                case ConsoleColor.Black:
                    currentScreen.black = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkBlue:
                    currentScreen.darkBlue = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkGreen:
                    currentScreen.darkGreen = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkCyan:
                    currentScreen.darkCyan = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkRed:
                    currentScreen.darkRed = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkMagenta:
                    currentScreen.darkMagenta = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkYellow:
                    currentScreen.darkYellow = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Gray:
                    currentScreen.gray = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkGray:
                    currentScreen.darkGray = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Blue:
                    currentScreen.blue = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Green:
                    currentScreen.green = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Cyan:
                    currentScreen.cyan = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Red:
                    currentScreen.red = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Magenta:
                    currentScreen.magenta = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Yellow:
                    currentScreen.yellow = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.White:
                    currentScreen.white = new COLORREF(r, g, b);
                    break;
            }

            ++currentScreen.srWindow.Bottom;
            ++currentScreen.srWindow.Right;
            brc = SetConsoleScreenBufferInfoEx(handle, ref currentScreen);
            if (!brc)
            {
                return Marshal.GetLastWin32Error();
            }
            return 0;
        }

        public static Color BackgroundColor { set { Console.BackgroundColor = GetColor(value); } }
        public static Color ForegroundColor { set { Console.ForegroundColor = GetColor(value); } }

        static Dictionary<Color, ConsoleColor> _colorDic = new Dictionary<Color, ConsoleColor>();
        public static ConsoleColor GetColor(Color color)
        {
            if (!_colorDic.Keys.Contains(color))
            {
                var consoleColor = GetConsoleColor();
                SetColor(consoleColor, color);
                _colorDic.Add(color, consoleColor);
            }
            return _colorDic[color];
        }

        static int _currentColor = 1;
        private static ConsoleColor GetConsoleColor()
        {
            _currentColor = _currentColor == 16 ? 1 : _currentColor;
            return (ConsoleColor)_currentColor++;
        }

    }
}
