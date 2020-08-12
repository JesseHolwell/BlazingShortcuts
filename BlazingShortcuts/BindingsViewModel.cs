using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace BlazingShortcuts
{
    public class BindingsViewModel
    {
        public BindingsViewModel()
        {
            this.Scope = new List<Scope>();
            this.Keyboard = new Keyboard();
            this.Shortcut = new Shortcut();
        }

        public List<Scope> Scope { get; set; }
        public Keyboard Keyboard { get; set; }

        public Shortcut Shortcut { get; set; }

        public void Reset()
        {
            this.Scope = new List<Scope>();
            //this.Keys = new List<KeyboardKeys>();
        }

        public async Task GenerateList(byte[] file)
        {
            Reset();

            XmlDocument doc = new XmlDocument();
            string xml = Encoding.UTF8.GetString(file);
            if (xml.Contains("<?xml"))
            {
                xml = xml.Substring(xml.IndexOf("?>") + 2);
            }

            doc.LoadXml(xml);

            XmlNodeList nodes = doc.DocumentElement.GetElementsByTagName("command");

            foreach (XmlNode node in nodes)
            {
                var name = node.Attributes["name"].Value;
                var shortcut = node.Attributes["shortcut"].Value;

                CreateBinding(this, name, shortcut);

                //Binding prev = new Binding();

                //Binding binding = prev.FullName == name ? prev : CreateBinding(this, name, shortcut);
                //could  set this up to put matching shortcuts under the same name

            }

        }

        private static Binding CreateBinding(BindingsViewModel model, string name, string shortcut)
        {
            int index = name.IndexOf('.');
            string displayName = index > 0 ? name.Substring(name.LastIndexOf('.') + 1) : name;
            string prefix = index > 0 ? CleanName(name.Substring(0, index)) : "Misc";
            Binding binding = new Binding(name, CleanName(displayName), shortcut);

            if (!model.Scope.Any(x => x.Name == prefix))
                model.Scope.Add(new Scope() { Name = prefix });

            model.Scope.Single(x => x.Name == prefix).Bindings.Add(binding);

            return binding;
        }

        public static string CleanName(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name[0]);

            for (int i = 1; i < name.Length; i++)
            {
                char c = name[i];
                if (char.IsUpper(c) && !char.IsUpper(name[i - 1]))
                {
                    sb.Append(" ");
                }
                sb.Append(c);
            }

            return sb.ToString();
        }

        public async Task UpdateViewModel(Shortcut shortcut)
        {
            //AppState.IsSearching(!string.IsNullOrEmpty(shortcut.ToString()));

            this.Keyboard.ResetAvailable();

            foreach (var v in this.Scope.SelectMany(x => x.Bindings))
            {
                if (v.DisplayName.ToLower() == "inserttab" || v.DisplayName.ToLower() == "insert tab")
                    Console.WriteLine(shortcut.ToString() + " | " + v.ShortcutKeys.ToString());

                v.IsMatch = (string.IsNullOrEmpty(shortcut.ToString()) || v.ShortcutKeys.ToString().ToLower().StartsWith(shortcut.ToString().ToLower()));

                if (v.IsMatch)
                {
                    var hey = v.ShortcutKeys.ToString().Substring(shortcut.ToString().Length);

                    var you = hey.Split(',');

                    var lazy = (you.FirstOrDefault().Length == 0) ? you.LastOrDefault() : you.FirstOrDefault();

                    var are = lazy.Trim().Split('+').FirstOrDefault().Trim();

                    var keyEnum = GetKeyFromString(are);
                    if (keyEnum.HasValue)
                        this.Keyboard.Keys[keyEnum.Value].IsAvailable = true;
                }
            }
        }

        public Key? GetKeyFromString(string key)
        {
            key = key.ToLower();
            Key? keyEnum = null;

            if (key == "esc") keyEnum = Key.Escape;

            else if (key == "`") keyEnum = Key.Tilde;
            else if (key == "1") keyEnum = Key.Num1;
            else if (key == "2") keyEnum = Key.Num2;
            else if (key == "3") keyEnum = Key.Num3;
            else if (key == "4") keyEnum = Key.Num4;
            else if (key == "5") keyEnum = Key.Num5;
            else if (key == "6") keyEnum = Key.Num6;
            else if (key == "7") keyEnum = Key.Num7;
            else if (key == "8") keyEnum = Key.Num8;
            else if (key == "9") keyEnum = Key.Num9;
            else if (key == "0") keyEnum = Key.Num0;
            else if (key == "-") keyEnum = Key.Underscore;
            else if (key == "=") keyEnum = Key.Plus;

            else if (key == "bkspce") keyEnum = Key.Backspace;

            else if (key == "[") keyEnum = Key.CurlyOpen;
            else if (key == "]") keyEnum = Key.CurlyClose;
            else if (key == "\\") keyEnum = Key.Pipe;

            else if (key == ";") keyEnum = Key.Colon;
            else if (key == "'") keyEnum = Key.Quote;

            else if (key == "shift") keyEnum = Key.LShift;
            else if (key == ",") keyEnum = Key.Comma;
            else if (key == ".") keyEnum = Key.Dot;
            else if (key == "/") keyEnum = Key.Slash;

            else if (key == "control" || key == "ctrl") keyEnum = Key.LCtrl;
            else if (key == "alt") keyEnum = Key.LAlt;

            else if (key == "up arrow") keyEnum = Key.Up;
            else if (key == "left arrow") keyEnum = Key.Left;
            else if (key == "down arrow") keyEnum = Key.Down;
            else if (key == "right arrow") keyEnum = Key.Right;

            else if (key == "break") keyEnum = Key.PauseBreak;

            else if (key == "ins") keyEnum = Key.Insert;
            else if (key == "pgup") keyEnum = Key.PageUp;
            else if (key == "del") keyEnum = Key.Delete;
            else if (key == "pgdn") keyEnum = Key.PageDown;


            else if (key == " ") keyEnum = Key.Space;

            else
                keyEnum = (Key)Enum.Parse(typeof(Key), key, true);

            return keyEnum;
        }
    }

    public class Scope
    {
        public string Name { get; set; }

        public List<Binding> Bindings { get; set; } = new List<Binding>();

        public bool Visible { get; set; } = true;

        public bool HasMatch => Bindings.Any(x => x.IsMatch);
    }

    public class Binding
    {
        public Binding() { }
        public Binding(string name, string displayName, string shortcut)
        {
            FullName = name;
            DisplayName = displayName;
            //Shortcut = shortcut;
            ShortcutKeys = new Shortcut(shortcut);
        }

        public string FullName { get; set; }
        public string DisplayName { get; set; }
        //public string Shortcut { get; set; } //= new List<string>(); //TODO: change from list to single shortcut, add multiple bindings for multiple shortcuts
        public Shortcut ShortcutKeys { get; set; }

        public bool Visible { get; set; } = true;
        public bool IsMatch { get; set; } = false;
    }

    public class Shortcut
    {
        public Shortcut()
        {
            while (ShortcutKeys.Count() < 2)
            {
                ShortcutKeys.Add(new Keys());
            }
        }

        public void Reset()
        {
            ShortcutKeys[0] = new Keys();
            ShortcutKeys[1] = new Keys();
        }

        public Shortcut(Keys k1, Keys k2)
        {
            ShortcutKeys.Add(k1);
            ShortcutKeys.Add(k2);
        }

        public Shortcut(string shortcut)
        {
            foreach (var v in shortcut.Split(','))
            {
                var keys = new Keys();
                foreach (var z in v.Split('+'))
                {
                    if (z == "Ctrl")
                        keys.Control = true;
                    else if (z == "Alt")
                        keys.Alt = true;
                    else if (z == "Shift")
                        keys.Shift = true;
                    else
                        keys.Key = z;
                }

                ShortcutKeys.Add(keys);
            }
        }

        public override string ToString()
        {
            var output = string.Empty;
            var key1 = ShortcutKeys.FirstOrDefault()?.ToString() ?? "";
            var key2 = ShortcutKeys.LastOrDefault()?.ToString() ?? "";
            output += !string.IsNullOrWhiteSpace(key1) ? key1 : "";
            output += !string.IsNullOrWhiteSpace(key2) ? (", " + key2) : "";
            return output;
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

    }

    public class Keys
    {
        public string Key { get; set; }

        public bool Control { get; set; }
        public bool Alt { get; set; }
        public bool Shift { get; set; }

        public override string ToString()
        {
            return (Control ? "Ctrl + " : "")
                + (Alt ? "Alt + " : "")
                + (Shift ? "Shift + " : "")
                + (!string.IsNullOrEmpty(Key) ? Key : "");
        }

        public bool IsFull => !string.IsNullOrEmpty(Key);

    }

    public class Keyboard
    {
        public Keyboard()
        {
            foreach (Key key in (Key[])Enum.GetValues(typeof(Key)))
            {
                AddKey(key);
            }
        }

        public void AddKey(Key key)
        {
            Keys.Add(key, new KeyboardKeyModel(key.ToString()));
        }

        public Dictionary<Key, KeyboardKeyModel> Keys { get; set; }
            = new Dictionary<Key, KeyboardKeyModel>();

        public void ResetPressed()
        {
            foreach (var v in Keys)
            {
                v.Value.IsPressed = false;
            }
        }

        public void ResetAvailable()
        {
            foreach (var v in Keys)
            {
                v.Value.IsAvailable = false;
            }
        }

    }

    public class KeyboardKeyModel
    {
        public KeyboardKeyModel(string key)
        {
            Key = key;
        }
        public string Key { get; set; }

        public bool IsPressed { get; set; }
        public bool IsAvailable { get; set; }

        public void Press()
        {
            IsPressed = !IsPressed;
        }
    }

    public enum Key
    {
        Escape,
        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,

        Tilde,
        Num1,
        Num2,
        Num3,
        Num4,
        Num5,
        Num6,
        Num7,
        Num8,
        Num9,
        Num0,
        Underscore,
        Plus,
        Backspace,

        Tab,
        Q,
        W,
        E,
        R,
        T,
        Y,
        U,
        I,
        O,
        P,
        CurlyOpen,
        CurlyClose,
        Pipe,

        Caps,
        A,
        S,
        D,
        F,
        G,
        H,
        J,
        K,
        L,
        Colon,
        Quote,
        Enter,

        LShift,
        Z,
        X,
        C,
        V,
        B,
        N,
        M,
        Comma,
        Dot,
        Slash,
        RShift,

        LCtrl,
        LWin,
        LAlt,
        Space,
        RAlt,
        RWin,
        Func,
        RCtrl,

        PrintScreen,
        ScrollLock,
        PauseBreak,
        Insert,
        Home,
        PageUp,
        Delete,
        End,
        PageDown,

        Up,
        Left,
        Down,
        Right,

        Hidden,
    }
}
