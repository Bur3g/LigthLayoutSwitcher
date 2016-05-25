using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LigthLayoutSwitcher
{
    static class LowLevelApiUtils
    {

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
            public Int32 X;
            public Int32 Y;
            public UInt32 MouseData;
            public UInt32 Flags;
            public UInt32 Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public UInt16 Vk;
            public UInt16 Scan;
            public UInt32 Flags;
            public UInt32 Time;
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


        public enum KeyPosition
        {
            Down,
            Up
        };

        [DllImport("user32.dll", SetLastError = true)]
        private static extern UInt32 SendInput(UInt32 numberOfInputs, INPUT[] inputs, Int32 sizeOfInputStructure);

        public static void SendInput (Keys[] keys)
        {
            INPUT[] downInputs = new INPUT[keys.Length];
            INPUT[] upInputs = new INPUT[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                downInputs[i] = MakeKeyInput(keys[i], true);
                upInputs[i] = MakeKeyInput(keys[i], false);
            }
            uint len = Convert.ToUInt32(keys.Length);

            SendInput(len, downInputs, Marshal.SizeOf(typeof(INPUT)));
            SendInput(len, upInputs, Marshal.SizeOf(typeof(INPUT)));
        }


        /*
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern short VkKeyScanEx(char ch, IntPtr dwhkl);
        public static Keys ToKey(char ch)
        {
            var layout = GetCurrentLayout();

            short keyNumber = VkKeyScanEx(ch, layout);
            if (keyNumber == -1)
            {
                return System.Windows.Forms.Keys.None;
            }
            return (System.Windows.Forms.Keys)(((keyNumber & 0xFF00) << 8) | (keyNumber & 0xFF));
        }
        */
    }

}
