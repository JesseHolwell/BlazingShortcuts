using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BlazingShortcuts.Models
{
    public class VSShortcutsViewModel
    {
        public VSShortcutsViewModel()
        {
            this.Reset();
        }

        public event Action OnBindingsChange;
        public event Action OnKeyboardChange;
        public event Action OnShortcutChange;

        private void NotifyBindingsChanged() => OnBindingsChange?.Invoke();
        private void NotifyKeyboardChanged() => OnKeyboardChange?.Invoke();
        private void NotifyShortcutChanged() => OnShortcutChange?.Invoke();

        public BindingModel Bindings { get; set; }
        public KeyboardModel Keyboard { get; set; }
        public ShortcutModel Shortcut { get; set; }

        public void Reset()
        {
            this.Bindings = new BindingModel();
            this.Keyboard = new KeyboardModel();
            this.Shortcut = new ShortcutModel();
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

        private static Binding CreateBinding(VSShortcutsViewModel model, string name, string shortcut)
        {
            int index = name.IndexOf('.');
            string displayName = index > 0 ? name.Substring(name.LastIndexOf('.') + 1) : name;
            string prefix = index > 0 ? CleanName(name.Substring(0, index)) : "Misc";
            Binding binding = new Binding(name, CleanName(displayName), shortcut);

            if (!model.Bindings.Scope.Any(x => x.Name == prefix))
                model.Bindings.Scope.Add(new Scope() { Name = prefix });

            model.Bindings.Scope.Single(x => x.Name == prefix).Bindings.Add(binding);

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

        public async Task UpdateViewModel(ShortcutModel shortcut)
        {
            Console.WriteLine($"UpdateViewModel - {shortcut.ToString()}");

            //AppState.IsSearching(!string.IsNullOrEmpty(shortcut.ToString()));

            this.Keyboard.ResetAvailable();

            foreach (var v in this.Bindings.Scope.SelectMany(x => x.Bindings))
            {
                //split up the shortcut string into parts



                if (v.DisplayName.ToLower() == "inserttab" || v.DisplayName.ToLower() == "insert tab")
                    Console.WriteLine(shortcut.ToString() + " | " + v.ShortcutKeys.ToString());

                var ctrlMatch = v.ShortcutKeys.Keys1.Control == shortcut.Keys1.Control;
                var altMatch = v.ShortcutKeys.Keys1.Alt == shortcut.Keys1.Alt;
                var shiftMatch = v.ShortcutKeys.Keys1.Shift == shortcut.Keys1.Shift;
                var keyMatch = v.ShortcutKeys.Keys1.Key == shortcut.Keys1.Key || string.IsNullOrEmpty(shortcut.Keys1.Key);

                v.IsMatch = ctrlMatch && altMatch && shiftMatch && keyMatch;
                    //(string.IsNullOrEmpty(shortcut.ToString()) || v.ShortcutKeys.ToString().ToLower().StartsWith(shortcut.ToString().ToLower()));

                //if control is pressed
                //matches = anything with ctrl in it
                //available = shift, alt, anything thats a direct ctrl combo

                if (v.IsMatch)
                {
                    var hey = v.ShortcutKeys.ToString().Substring(shortcut.ToString().Length);

                    var you = hey.Split(',');

                    var are = (you.FirstOrDefault().Length == 0) ? you.LastOrDefault() : you.FirstOrDefault();

                    var lazy = are.Trim().Split('+').FirstOrDefault().Trim();

                    if (!string.IsNullOrWhiteSpace(lazy))
                    {
                        var keyEnum = GetKeyFromString(lazy);
                        if (keyEnum.HasValue)
                            this.Keyboard.Keys[keyEnum.Value].IsAvailable = true;
                    }
                }
            }
        }

        public Key? GetKeyFromString(string key)
        {
            Console.WriteLine($"GetKeyFromString - {key}");

            key = key.ToLower();
            Key? keyEnum = null;

            if (key == "esc") keyEnum = Key.Escape;

            else if (key == "`") keyEnum = Key.Backtick;
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
            else if (key == "-") keyEnum = Key.Minus;
            else if (key == "=") keyEnum = Key.Equals;

            else if (key == "bkspce") keyEnum = Key.Backspace;

            else if (key == "[") keyEnum = Key.SquareOpen;
            else if (key == "]") keyEnum = Key.SquareClose;
            else if (key == "\\") keyEnum = Key.Backslash;

            else if (key == ";") keyEnum = Key.Semicolon;
            else if (key == "'") keyEnum = Key.Apostrophe;

            else if (key == "shift") keyEnum = Key.Shift;
            else if (key == ",") keyEnum = Key.Comma;
            else if (key == ".") keyEnum = Key.Fullstop;
            else if (key == "/") keyEnum = Key.Slash;

            else if (key == "control" || key == "ctrl") keyEnum = Key.Ctrl;
            else if (key == "alt") keyEnum = Key.Alt;

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
}
