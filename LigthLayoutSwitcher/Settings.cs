using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using File = System.IO.File;

namespace LigthLayoutSwitcher
{
    [Serializable]
    public class Settings : ApplicationSettingsBase
    {
        public static Settings Init()
        {
            // Settings Initialization
            Settings settings = new Settings();

            settings.Reload();

            // Setting default keys
            if (settings.SwitchKey == null)
            {
                settings.SwitchKey = new KeyInfo(Keys.Pause);
            }
            if (settings.ConvertKey == null)
            {
                settings.ConvertKey = new KeyInfo(Keys.Pause | Keys.Shift);
            }
            if (settings.ChangeRegisterKey == null)
            {
                settings.ChangeRegisterKey = new KeyInfo(Keys.Pause | Keys.Alt);
            }
            settings.Save();

            return settings;
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        // HotKey that switches last word
        public KeyInfo SwitchKey
        {
            get { return (KeyInfo) this["SwitchKey"]; }
            set { this["SwitchKey"] = value; }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        // HotKey that switches last selected text
        public KeyInfo ConvertKey
        {
            get { return (KeyInfo) this["ConvertKey"]; }
            set { this["ConvertKey"] = value; }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        // HotKey that switches register
        public KeyInfo ChangeRegisterKey
        {
            get { return (KeyInfo) this["ChangeRegisterKey"]; }
            set { this["ChangeRegisterKey"] = value; }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        // Autorun option
        public bool AutoRun
        {
            get { return (bool) this["AutoRun"]; }
            set { this["AutoRun"] = value; }
        }

        internal void SaveSettings()
        {
            Save();

            // Need to create or delete shortcut in system startup folder
            if (AutoRun)
            {
                CreateAutorunShortcut();
            }
            else
            {
                DeleteAutorunShortcut();
            }
        }

        private static string GetStartUpFolderPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                "LightLayoutSwitcher.lnk");
        }

        public static void CreateAutorunShortcut()
        {
            var shortcutLocation = GetStartUpFolderPath();
            if (File.Exists(shortcutLocation))
            {
                return;
            }
            var shell = new WshShell();
            var shortcut = (IWshShortcut) shell.CreateShortcut(shortcutLocation);
            shortcut.Description = "Light Layout Keyboard Switcher";
            shortcut.TargetPath = Assembly.GetExecutingAssembly().Location;
            shortcut.Save();
        }

        public static void DeleteAutorunShortcut()
        {
            File.Delete(GetStartUpFolderPath());
        }

        internal void ReloadSettings()
        {
            Reload();
        }
    }
}
