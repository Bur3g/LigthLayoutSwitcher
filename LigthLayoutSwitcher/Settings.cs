using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace LigthLayoutSwitcher
{
    [Serializable]
    public class Settings : ApplicationSettingsBase
    {
        public static Settings Init()
        {
            Settings settings = new Settings();
            settings.Reload();


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
        public KeyInfo SwitchKey
        {
            get { return (KeyInfo) this["SwitchKey"]; }
            set { this["SwitchKey"] = (KeyInfo) value; }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public KeyInfo ConvertKey
        {
            get { return (KeyInfo) this["ConvertKey"]; }
            set { this["ConvertKey"] = (KeyInfo) value; }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public KeyInfo ChangeRegisterKey
        {
            get { return (KeyInfo) this["ChangeRegisterKey"]; }
            set { this["ChangeRegisterKey"] = (KeyInfo) value; }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public bool AutoRun
        {
            get { return (bool) this["AutoRun"]; }
            set { this["AutoRun"] = (bool) value; }
        }

        internal void SaveSettings()
        {
            Save();

            if (AutoRun)
            {
                CreateAutorunShortcut();
            }
            else
            {
                DeleteAutorunShortcut();
            }
        }

        private static string GetAutorunPath()
        {
            return System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                "LightLayoutSwitcher.lnk");
        }
        public static void CreateAutorunShortcut()
        {
            var shortcutLocation = GetAutorunPath();
            if (System.IO.File.Exists(shortcutLocation))
            {
                return;
            }
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);
            shortcut.Description = "Light Layout Switcher";
            shortcut.TargetPath = Assembly.GetExecutingAssembly().Location;
            shortcut.Save();
        }
        public static void DeleteAutorunShortcut()
        {
            System.IO.File.Delete(GetAutorunPath());
        }

        internal void ReloadSettings()
        {
            Reload();
        }
    }
}
