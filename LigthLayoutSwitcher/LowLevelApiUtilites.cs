using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LigthLayoutSwitcher
{
    static class LowLevelApiUtilites
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern UInt32 SendInput(UInt32 numberOfInputs, INPUT[] inputs, Int32 sizeOfInputStructure);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetKeyboardLayout(UInt32 WindowsThreadProcessID);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern short VkKeyScanEx(char ch, IntPtr dwhkl);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern short OpenClipboard(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr GetClipboardData(UInt32 uFormat);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool CloseClipboard();
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool EmptyClipboard();
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern IntPtr GetFocus();
        [DllImport("kernel32.dll")]
        internal static extern uint GetCurrentThreadId();
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern int AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        public static extern int SendMessageW([In] System.IntPtr hWnd, uint Msg, int wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern short GetKeyState(int keyCode);

        public struct INPUT
        {
            public UInt32 Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
        }
        public struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        public struct HARDWAREINPUT
        {
            public UInt32 Msg;
            public UInt16 ParamL;
            public UInt16 ParamH;
        }
        public const int HWND_BROADCAST = 0xffff;
        public const int WH_KEYBOARD_LL = 13;
        public const int WH_MOUSE_LL = 14;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int INPUT_KEYBOARD = 1;
        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint WM_INPUTLANGCHANGEREQUEST = 0x50;
        public const uint INPUTLANGCHANGE_FORWARD = 0x02;
        public const uint HKL_NEXT = 1;
        public const uint WM_GETTEXT = 0x0D;
        public const uint WM_GETTEXTLENGTH = 0x0E;
        public const uint EM_GETSEL = 0xB0;
        public const uint EM_SETSEL = 0xB1;
        public const uint EM_REPLACESEL = 0xC2;
        public static INPUT MakeKeyInput(Keys vkCode, bool down)
        {
            return new INPUT
            {
                Type = INPUT_KEYBOARD,
                Data = new MOUSEKEYBDHARDWAREINPUT
                {
                    Keyboard = new KEYBDINPUT
                    {
                        Vk = (UInt16)vkCode,
                        Scan = 0,
                        Flags = down ? 0 : KEYEVENTF_KEYUP,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };
        }


        public static void SendCombinationInput (Keys[] keys)
        {
            INPUT[] downInputs = new INPUT[keys.Length];
            INPUT[] upInputs = new INPUT[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                Console.WriteLine(keys[i].ToString());
                downInputs[i] = MakeKeyInput(keys[i], true);
                upInputs[i] = MakeKeyInput(keys[i], false);
            }
            uint len = Convert.ToUInt32(keys.Length);

            SendInput(len, downInputs, Marshal.SizeOf(typeof(INPUT)));
            SendInput(len, upInputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendKeyPress(Keys vkCode, bool shift = false)
        {
            var down = MakeKeyInput(vkCode, true);
            var up = MakeKeyInput(vkCode, false);

            if (shift)
            {
                var shiftDown = MakeKeyInput(Keys.ShiftKey, true);
                var shiftUp = MakeKeyInput(Keys.ShiftKey, false);
                SendInput(4, new[] { shiftDown, down, up, shiftUp }, Marshal.SizeOf(typeof(INPUT)));
            }
            else
            {
                SendInput(2, new[] { down, up }, Marshal.SizeOf(typeof(INPUT)));
            }
        }

        public static List<Keys> StringToKeys(string str)
        {
            List<Keys> list = new List<Keys>();
            foreach (var symb in str)
            {
                var wndThreadId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
                var layout  = GetKeyboardLayout(wndThreadId);

                short keyNumber = VkKeyScanEx(symb, layout);
                if (keyNumber == -1)
                {
                    list.Add(Keys.None);
                }
                else
                {
                    list.Add((Keys)(((keyNumber & 0xFF00) << 8) | (keyNumber & 0xFF)));
                }
                
            }
            foreach (var key in list)
            {
                Console.WriteLine(key.ToString());
            }
            return list;
        }

        private const uint CF_TEXT = 1;
        public static string GetClipboardText()
        {
            OpenClipboard(GetForegroundWindow());
            string ret = "";
            IntPtr buf;
            if ((buf = GetClipboardData(CF_TEXT)) != IntPtr.Zero)
            {
                ret = Marshal.PtrToStringAnsi(buf);
            }
            CloseClipboard();

            return ret;
        }

        public static bool IsCapsLockOn()
        {
            return (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
        }

        /*
         
        // Gets ALL text from focused textbox, but we need only selected part  
        public static string GetTextFromFocusedControl()
        {
            try
            {
                var wndThreadId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
                uint currentThreadId = GetCurrentThreadId();
                if (wndThreadId != currentThreadId)
                    AttachThreadInput(wndThreadId, currentThreadId, true);

                IntPtr activeCtrlId = GetFocus();


                return GetText(activeCtrlId);
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }

        private static string GetText(IntPtr handle)
        {
            int maxLength = 100;
            IntPtr buffer = Marshal.AllocHGlobal((maxLength + 1) * 2);
            SendMessageW(handle, WM_GETTEXT, maxLength, buffer);
            string w = Marshal.PtrToStringUni(buffer);
            Marshal.FreeHGlobal(buffer);
            return w;
        }
        */
    }

}
