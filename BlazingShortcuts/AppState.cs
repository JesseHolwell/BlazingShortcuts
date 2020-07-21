using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BlazingShortcuts
{
    public class AppState
    {
        public bool Theme { get; private set; }

        public event Action OnChange;

        public bool ToggleTheme(bool? theme = null)
        {
            Theme = theme ?? !Theme;
            NotifyStateChanged();
            return Theme;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
