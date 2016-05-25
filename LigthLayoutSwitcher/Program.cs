using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LigthLayoutSwitcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Settings settings = Settings.Init();

            Switcher switcher = new Switcher(settings);
            switcher.Start();

            Application.Run(new LightLayoutSwitcherForm(settings, switcher));
        }
    }
}
