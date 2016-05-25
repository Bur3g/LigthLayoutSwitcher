using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.UserActivityMonitor;
using System.Runtime.InteropServices;

namespace LigthLayoutSwitcher
{
    public class Switcher
    {
        private String buffer;
        private Settings settings;

        private bool started;
        public Switcher(Settings settings)
        {
            this.settings = settings;
        }

        public void Start()
        {
            if (!started)
            {
                started = true;
                HookManager.KeyDown += HookManagerOnKeyDown;
            }
        }

        public void Stop()
        {
            started = false;
            HookManager.KeyDown -= HookManagerOnKeyDown;
        }
        private void HookManagerOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {

            if (keyEventArgs.KeyData.Equals(settings.ConvertKey.KeyData))
            {
                ConvertSelected();
            }
        }

        private void ConvertSelected()
        {
            ChangeLayout();

            var previousClipboardData = Clipboard.GetDataObject();

            LowLevelApiUtils.SendInput(new Keys[] { Keys.LControlKey, Keys.C });
            string text = Clipboard.GetText(); //Получаем текст из буфера обмена
            if(previousClipboardData != null)
                Clipboard.SetDataObject(previousClipboardData);

            if (String.IsNullOrEmpty(text)) return;

            LowLevelApiUtils.SendInput(new Keys[] { Keys.Delete } );


        }
        private void ChangeLayout()
        {
            LowLevelApiUtils.SendInput(new Keys[] { Keys.LShiftKey, Keys.LMenu });
            //LowLevelApiUtils.SendInput(new Keys[] { Keys.LShiftKey, Keys.LMenu }, LowLevelApiUtils.KeyPosition.Up);
        }

    }
}
