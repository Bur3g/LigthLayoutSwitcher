using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LigthLayoutSwitcher
{
    [Serializable]
    public class KeyInfo: KeyEventArgs, ISerializable
    {
        private readonly StreamingContext _context;

        public KeyInfo(Keys keyData) : base(keyData)
        {
        }
        public KeyInfo(SerializationInfo info, StreamingContext context)
            : base((Keys)info.GetValue("keyData", typeof(Keys)))
        {
            _context = context;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("keyData", KeyData, typeof(Keys));
        }

        public override string ToString()
        {
            return ((Shift ? Keys.Shift.ToString() + " + " : "") +
                   (Alt ? Keys.Alt.ToString() + " + " : "") +
                   (Control ? Keys.Control.ToString() + " + " : "") +
                   KeyCode.ToString())
                   .Replace("Cancel", "Pause");
        }
    }
}
