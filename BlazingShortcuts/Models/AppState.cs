using System;

namespace BlazingShortcuts
{
    public class AppState
    {
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public bool Theme { get; private set; }
        public bool Fullscreen => false;
        //public bool Fullscreen => DisplayState == DisplayState.ShortcutsFull;
        public bool Configure { get; private set; }
        public bool ShowInfo { get; private set; }
        public bool Searching { get; private set; }



        //public bool DisplayKeyEvents { get; private set; }
        //public bool DisplayShortcuts { get; private set; }
        //public bool DisplayInfo { get; private set; }

        public DisplayState DisplayState { get; set; }

        public void SetDisplayState(DisplayState state)
        {
            Console.WriteLine(state);
            DisplayState = state;
            NotifyStateChanged();
        }

        public bool ToggleTheme(bool? theme = null)
        {
            Theme = theme ?? !Theme;
            NotifyStateChanged();
            return Theme;
        }

        //public bool ToggleFullscreen(bool? fullscreen = null)
        //{
        //    Fullscreen = fullscreen ?? !Fullscreen;
        //    NotifyStateChanged();
        //    return Fullscreen;
        //}

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

    public enum DisplayState
    {
        EventsAndInfo = 0,
        EventsAndShortcuts = 1,
        ShortcutsAndInfo = 2,
        ShortcutsFull = 3
    }
}
