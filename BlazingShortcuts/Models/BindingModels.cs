using System.Collections.Generic;
using System.Linq;

namespace BlazingShortcuts.Models
{
    public class BindingModel
    {
        public List<Scope> Scope { get; set; } = new List<Scope>();

        public bool IsInitialized => this.Scope.Any();
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
            ShortcutKeys = new ShortcutModel(shortcut);
        }

        public string FullName { get; set; }

        public string DisplayName { get; set; }

        public ShortcutModel ShortcutKeys { get; set; }

        public bool Visible { get; set; } = true;

        public bool IsMatch { get; set; } = false;
    }
}
