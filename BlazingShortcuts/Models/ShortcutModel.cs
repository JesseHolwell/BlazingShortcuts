using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingShortcuts.Models
{
    public class ShortcutModel
    {
        public ShortcutModel()
        {
            while (ShortcutKeys.Count() < 2)
            {
                ShortcutKeys.Add(new Keys());
            }
        }

        public ShortcutModel(Keys k1, Keys k2)
        {
            ShortcutKeys.Add(k1);
            ShortcutKeys.Add(k2);
        }

        public List<Keys> ShortcutKeys { get; set; } = new List<Keys>();

        public Keys Keys1 => ShortcutKeys.Count() >= 1 ? ShortcutKeys[0] : null;
        public Keys Keys2 => ShortcutKeys.Count() >= 2 ? ShortcutKeys[1] : null;

        public bool IsCtrlDown { get; set; }
        public bool IsShiftDown { get; set; }
        public bool IsAltDown { get; set; }

        public bool currentState { get; set; }
        public int current => Convert.ToInt32(currentState);

        public bool IsFull => Keys1?.IsFull == true && Keys2?.IsFull == true;

        public void Reset()
        {
            ShortcutKeys[0] = new Keys();
            ShortcutKeys[1] = new Keys();
        }

        public ShortcutModel(string shortcut)
        {
            //Console.WriteLine($"input:\t{shortcut}");
            foreach (var v in shortcut.Split(','))
            {
                //Console.WriteLine($"\npart:\t{v}");

                var keys = new Keys();
                foreach (var z in v.Split('+'))
                {
                    //Console.WriteLine($"\n\nkey:\t{z}");

                    if (z == "Ctrl")
                        keys.Control = true;
                    else if (z == "Alt")
                        keys.Alt = true;
                    else if (z == "Shift")
                        keys.Shift = true;
                    else
                        keys.Key = z;
                }

                //Console.WriteLine($"\nkeys:\t{keys.ToString()}");
                ShortcutKeys.Add(keys);
            }

            //Console.WriteLine($"\ndone:\t{this.ToString()}");

        }

        public override string ToString()
        {
            var output = string.Empty;
            var key1 = ShortcutKeys.Count() >= 1 ? ShortcutKeys[0].ToString() : "";
            var key2 = ShortcutKeys.Count() >= 2 ? ShortcutKeys[1].ToString() : "";
            output += !string.IsNullOrWhiteSpace(key1) ? key1 : "";
            output += !string.IsNullOrWhiteSpace(key2) ? (", " + key2) : "";
            return output;
        }
    }

    public class Keys
    {
        public string Key { get; set; }

        public bool Control { get; set; }
        public bool Alt { get; set; }
        public bool Shift { get; set; }

        public bool IsFull => !string.IsNullOrEmpty(Key);

        public override string ToString()
        {
            return (Control ? "Ctrl + " : "")
                + (Alt ? "Alt + " : "")
                + (Shift ? "Shift + " : "")
                + (!string.IsNullOrEmpty(Key) ? Key : "");
        }

    }
}
