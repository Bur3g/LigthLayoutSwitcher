using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Gma.UserActivityMonitor;

namespace LigthLayoutSwitcher
{
    public partial class LightLayoutSwitcherForm : Form
    {
        private Settings settings;
        private Switcher switcher;
        private bool keyboardHookedByForm = false;

        public LightLayoutSwitcherForm(Settings settings, Switcher switcher)
        {
            this.settings = settings;
            this.switcher = switcher;

            InitializeComponent();
            SetFormPosition();
            SetTextBoxActions();
            ReadSettings();

        }

        private void ReadSettings()
        {
            SwitchKeyText.Text = settings.SwitchKey.ToString();
            ConvertKeyText.Text = settings.ConvertKey.ToString();
            SwitchRegisterText.Text = settings.ChangeRegisterKey.ToString();
            Startup_checkBox.Checked = settings.AutoRun;
        }


        private void SetFormPosition()
        {
            // Seeting up position of form
            Screen rightmost = Screen.AllScreens[0];
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                {
                    rightmost = screen;
                }
            }
            this.Location = new Point(rightmost.WorkingArea.Right - this.Width,
                rightmost.WorkingArea.Bottom - this.Height);
        }

        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowForm();
            }
            else
            {
                WindowState = FormWindowState.Minimized;
                HideForm();
            }
        }
        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                this.Activate();
            }
        }
        private void LightLayoutSwitcherForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                HideForm();
            }
        }
        private void LightLayoutSwitcherForm_Activated(object sender, EventArgs e)
        {
            HookKeyboard();
        }
        private void LightLayoutSwitcherForm_Deactivate(object sender, EventArgs e)
        {
            FreeKeyboard();
        }


        private void ShowForm()
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            HookKeyboard();
        }

        private void HideForm()
        {
            this.Hide();
            TrayIcon.Visible = true;
            FreeKeyboard();
        }
        private void HookKeyboard()
        {
            switcher.Stop();
            if (!keyboardHookedByForm)
            {
                keyboardHookedByForm = true;
                HookManager.KeyDown += SetKey;
            }

        }
        private void FreeKeyboard()
        {
            switcher.Start();
            keyboardHookedByForm = false;
            HookManager.KeyDown -= SetKey;
        }


        private enum KeyType
        {
            None,
            Switch,
            Convert,
            Register
        }

        private KeyType _currenKeyType = KeyType.None;
        private KeyInfo _currentKey;

        private void SetTextBoxActions()
        {
            SwitchKeyText.Enter += (sender, args) => _currenKeyType = KeyType.Switch;
            SwitchKeyText.Leave += (sender, args) => UpdateSettings();
            ConvertKeyText.Enter += (sender, args) => _currenKeyType = KeyType.Convert;
            ConvertKeyText.Leave += (sender, args) => UpdateSettings();
            SwitchRegisterText.Enter += (sender, args) => _currenKeyType = KeyType.Register;
            SwitchRegisterText.Leave += (sender, args) => UpdateSettings();
        }

        private void SetKey(object sender, KeyEventArgs e)
        {
            KeyInfo keyInfo = new KeyInfo(e.KeyData);
            var vk = e.KeyCode;
            if (vk == Keys.Escape)
            {
                e.Handled = true;
                ResetCurrentHotkey();
                return;
            }

            if (vk != Keys.LMenu && vk != Keys.RMenu
                && vk != Keys.LWin && vk != Keys.RWin
                && vk != Keys.LShiftKey && vk != Keys.RShiftKey
                && vk != Keys.LControlKey && vk != Keys.RControlKey)
            {
                e.Handled = true;
            }

            switch (_currenKeyType)
            {
                case KeyType.Switch:
                    SwitchKeyText.Text = keyInfo.ToString();
                    break;
                case KeyType.Convert:
                    ConvertKeyText.Text = keyInfo.ToString();
                    break;
                case KeyType.Register:
                    SwitchRegisterText.Text = keyInfo.ToString();
                    break;
            }
            _currentKey = keyInfo;
        }

        private void ResetCurrentHotkey()
        {

            switch (_currenKeyType)
            {
                case KeyType.Switch:
                    _currentKey = settings.SwitchKey;
                    break;
                case KeyType.Convert:
                    _currentKey = settings.ConvertKey;
                    break;
                case KeyType.Register:
                    _currentKey = settings.ChangeRegisterKey;
                    break;
            }
            ReadSettings();
        }

        private void UpdateSettings()
        {
            if (_currentKey == null)
                ResetCurrentHotkey();

            switch (_currenKeyType)
            {
                case KeyType.Switch:
                    settings.SwitchKey = _currentKey;
                    break;
                case KeyType.Convert:
                    settings.ConvertKey = _currentKey;
                    break;
                case KeyType.Register:
                    settings.ChangeRegisterKey = _currentKey;
                    break;
            }
        }

        private void Ok_button_Click(object sender, EventArgs e)
        {
            settings.AutoRun = Startup_checkBox.Checked;
            settings.SaveSettings();
        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            settings.ReloadSettings();
            ReadSettings();
        }


    }
}
