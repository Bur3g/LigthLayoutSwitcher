using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Gma.UserActivityMonitor;
using System.Threading;

namespace LigthLayoutSwitcher
{
    public class Switcher
    {
        private readonly Settings settings;
        private readonly List<KeyEventArgs> currentWord = new List<KeyEventArgs>();
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

            if (keyEventArgs.KeyData.Equals(settings.SwitchKey.KeyData))
            {
                keyEventArgs.Handled = true;
                ConvertLast();
                return;
            }
            if (keyEventArgs.KeyData.Equals(settings.ConvertKey.KeyData))
            {
                keyEventArgs.Handled = true;
                ConvertSelected();
                return;
            }
            if (keyEventArgs.KeyData.Equals(settings.ChangeRegisterKey.KeyData))
            {
                keyEventArgs.Handled = true;
                ConvertRegister();
                return;
            }

            var vkCode = keyEventArgs.KeyCode;
            if (vkCode == Keys.Space ) { AddToCurrentWord(new KeyInfo(keyEventArgs.KeyData)); return; }
            if (vkCode == Keys.Back ) { RemoveLastKey(); return; }
            if (IsPrintable(keyEventArgs))
            {
                if (GetPreviousVkCode() == Keys.Space) { BeginNewWord(); }
                AddToCurrentWord(new KeyInfo(keyEventArgs.KeyData));
                return;
            }

            BeginNewWord();
        }
        public static bool IsPrintable(KeyEventArgs evtData)
        {
            if (evtData.Alt || evtData.Control) { return false; }
            var keyCode = evtData.KeyCode;
            if (keyCode >= Keys.D0 && keyCode <= Keys.Z) { return true; }
            if (keyCode >= Keys.Oem1 && keyCode <= Keys.OemBackslash) { return true; }
            if (keyCode >= Keys.NumPad0 && keyCode <= Keys.NumPad9) { return true; }
            if (keyCode == Keys.Decimal) { return true; }
            return false;
        }
        private Keys GetPreviousVkCode()
        {
            if (currentWord.Count == 0) { return Keys.None; }
            return currentWord[currentWord.Count - 1].KeyCode;
        }
        private void BeginNewWord() { currentWord.Clear(); }

        private void AddToCurrentWord(KeyEventArgs data)
        {
            currentWord.Add(data);
        }
        private void RemoveLastKey()
        {
            if (currentWord.Count == 0) { return; }
            currentWord.RemoveAt(currentWord.Count - 1);
        }
        private void ConvertSelected()
        {
            // TODO clipboard problems
            // Save previous Clipboard data (Strange behavior of Clipboard.GetText, Clipboard.SetText)
            // If i try to copy text, that printed in another layout (not current), the date from clipboard is corrupted

            // Copy selected text
            LowLevelApiUtilites.SendCombinationInput(new [] {Keys.LControlKey, Keys.Insert });

            //In cae of strange behavior of Clipboard
            Thread.Sleep(200);

            string text = LowLevelApiUtilites.GetClipboardText();
            if (String.IsNullOrEmpty(text)) return;
            ChangeLayout();

            LowLevelApiUtilites.SendCombinationInput(new[] {Keys.Delete});
            
            // Send input of each key
            foreach (var key in LowLevelApiUtilites.StringToKeys(text))
            {
                LowLevelApiUtilites.SendKeyPress(key, (key & Keys.Shift) != Keys.None);
            }
        }

        private void ConvertRegister()
        {

            // TODO clipboard problems
            // Same problems whith clipboard and different layouts like in ConvertSelected()

            LowLevelApiUtilites.SendCombinationInput(new[] {Keys.LControlKey, Keys.Insert});
            Thread.Sleep(200);

            var text = LowLevelApiUtilites.GetClipboardText();
            Console.WriteLine(text);

            if (string.IsNullOrEmpty(text)) return;

            var convertedText = "";
            if (!LowLevelApiUtilites.IsCapsLockOn())
            {
                foreach (var c in text)
                {
                    if (!char.IsUpper(c))
                    {
                        convertedText += char.ToUpper(c);
                    }
                    else
                    {
                        convertedText += char.ToLower(c);
                    }
                }
            }
            else
            {
                convertedText = text;
            }

            Console.WriteLine(convertedText);

            LowLevelApiUtilites.SendKeyPress(Keys.Delete);
            //bool isCapsLockOn = !LowLevelApiUtilites.IsCapsLockOn();
            foreach (var key in LowLevelApiUtilites.StringToKeys(convertedText))
            {
                LowLevelApiUtilites.SendKeyPress(key, ((key & Keys.Shift) != Keys.None) );
            }
        }

        private void ChangeLayout()
        {
            // TODO determine current global hotkey for change layout
            LowLevelApiUtilites.SendCombinationInput(new[] { Keys.LShiftKey, Keys.LMenu });  // most common shortcuts for now
            LowLevelApiUtilites.SendCombinationInput(new[] { Keys.LShiftKey, Keys.LControlKey });  // most common shortcuts for now
        }

        private void ConvertLast()
        {
            var word = currentWord.ToList();
            var backspaces = Enumerable.Repeat(Keys.Back, word.Count);
            foreach (var vkCode in backspaces) { LowLevelApiUtilites.SendKeyPress(vkCode); }
            ChangeLayout();
            foreach (var data in word)
            {
                LowLevelApiUtilites.SendKeyPress(data.KeyCode, data.Shift);
            }
        }
    }
}
