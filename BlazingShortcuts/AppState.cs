using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace BlazingShortcuts
{
    public class AppState
    {
        public event Action OnChange;

        public bool Theme { get; private set; }
        public bool Fullscreen { get; private set; }
        public bool Configure { get; private set; }
        public bool ShowInfo { get; private set; }

        public bool ToggleTheme(bool? theme = null)
        {
            Theme = theme ?? !Theme;
            NotifyStateChanged();
            return Theme;
        }

        public bool ToggleFullscreen(bool? fullscreen = null)
        {
            Fullscreen = fullscreen ?? !Fullscreen;
            NotifyStateChanged();
            return Fullscreen;
        }

        public bool ToggleConfigure(bool? configure = null)
        {
            Configure = configure ?? !Configure;
            NotifyStateChanged();
            return Configure;
        }

        public bool ToggleInfo(bool? showInfo = null)
        {
            ShowInfo = showInfo ?? !ShowInfo;
            NotifyStateChanged();
            return ShowInfo;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public string Serialize(BindingsViewModel model)
        {
            string output = JsonSerializer.Serialize(model);
            return output;
        }

        public BindingsViewModel Deserialize(string input)
        {
            var output = JsonSerializer.Deserialize<BindingsViewModel>(input);
            return output;
        }
    }
}
