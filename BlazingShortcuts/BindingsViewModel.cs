using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BlazingShortcuts
{
    public class BindingsViewModel
    {

        public string Output { get; set; }

        public static int ShortcutCount = 0;

        public BindingsViewModel()
        {
            this.bindings = new List<KeyGroup>();
        }

        public List<KeyGroup> bindings { get; set; }

        public string Save()
        {
            return "";
        }

        public void Load(string input)
        {

        }

        public void Reset()
        {

        }

        public async Task GenerateList(byte[] file)
        {
            ShortcutCount = 0;

            //Output += $"Generating...";
            //this.StateHasChanged();

            XmlDocument doc = new XmlDocument();
            string xml = Encoding.UTF8.GetString(file);
            if (xml.Contains("<?xml"))
            {
                xml = xml.Substring(xml.IndexOf("?>") + 2);
            }

            doc.LoadXml(xml);

            XmlNodeList nodes = doc.DocumentElement.GetElementsByTagName("command");

            //Output += $"nodes found {nodes.Count}";
            //this.StateHasChanged();

            foreach (XmlNode node in nodes)
            {
                var name = node.Attributes["name"].Value;
                var shortcut = node.Attributes["shortcut"].Value;

                //if (string.IsNullOrEmpty(name))
                //    continue;

                Binding prev = new Binding();

                Binding binding = prev.FullName == name ? prev : CreateBinding(this, name, shortcut);

                if (!binding.Shortcuts.Contains(shortcut))
                    prev.Shortcuts.Add(shortcut);

                prev = binding;
                ShortcutCount += 1;
            }

            bindings.First().visible = true;
        }

        private static Binding CreateBinding(BindingsViewModel model, string name, string shortcut)
        {
            int index = name.IndexOf('.');
            string displayName = index > 0 ? name.Substring(name.LastIndexOf('.') + 1) : name;
            string prefix = index > 0 ? CleanName(name.Substring(0, index)) : "Misc";
            Binding binding = new Binding(name, CleanName(displayName), shortcut);

            if (!model.bindings.Any(x => x.name == prefix))
                model.bindings.Add(new KeyGroup() { name = prefix });

            model.bindings.Single(x => x.name == prefix).bindings.Add(binding);
            //dic[prefix].Add(binding);

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
    }

    public class KeyGroup
    {
        public KeyGroup()
        {
            this.bindings = new List<Binding>();
        }

        public string name { get; set; }

        public bool visible { get; set; } = false;

        public List<Binding> bindings { get; set; }
    }

    public class Binding
    {
        public Binding(string name, string displayName, string shortcut)
        {
            FullName = name;
            DisplayName = displayName;
            Shortcuts.Add(shortcut);
        }

        public Binding()
        {
            FullName = string.Empty;
        }

        public string FullName;
        public string DisplayName;
        public List<string> Shortcuts = new List<string>();

        public bool Display { get; set; } = true;
    }
}
