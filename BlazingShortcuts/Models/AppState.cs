using System;

namespace BlazingShortcuts
{
    public class AppState
    {
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public bool Theme { get; private set; }
        public bool Fullscreen { get; private set; }
        public bool Configure { get; private set; }
        public bool ShowInfo { get; private set; }
        public bool Searching { get; private set; }

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

        public bool IsSearching(bool? searching = null)
        {
            Searching = searching ?? !Searching;
            NotifyStateChanged();
            return Searching;
        }
    }
}
