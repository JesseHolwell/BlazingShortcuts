using BlazingShortcuts.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BlazingShortcuts.Models
{
    public class KeyboardModel
    {
        public KeyboardModel()
        {
            foreach (Key key in (Key[])Enum.GetValues(typeof(Key)))
            {
                AddKey(key);
            }
        }

        public Dictionary<Key, KeyboardKeyModel> Keys { get; set; }
            = new Dictionary<Key, KeyboardKeyModel>();

        public void AddKey(Key key)
        {
            Keys.Add(key, new KeyboardKeyModel(key));
        }

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
        public KeyboardKeyModel(Key key)
        {
            Key = key;
            KeyFriendly = key.GetKeyDescription();
        }

        public Key Key { get; set; }

        public string KeyFriendly { get; set; }

        public bool IsPressed { get; set; }

        public bool IsAvailable { get; set; }

        public void Press()
        {
            IsPressed = !IsPressed;
        }
    }

    public enum Key
    {
        [Description("Esc")]
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

        [Description("~<br/>`")]
        Backtick,
        [Description("1")]
        Num1,
        [Description("2")]
        Num2,
        [Description("3")]
        Num3,
        [Description("4")]
        Num4,
        [Description("5")]
        Num5,
        [Description("6")]
        Num6,
        [Description("7")]
        Num7,
        [Description("8")]
        Num8,
        [Description("9")]
        Num9,
        [Description("0")]
        Num0,
        [Description("_<br/>-")]
        Minus,
        [Description("+<br/>=")]
        Equals,
        [Description("Bckspce")]
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
        [Description("{<br/>[")]
        SquareOpen,
        [Description("}<br/>]")]
        SquareClose,
        [Description("|<br/>\\")]
        Backslash,

        [Description("Caps<br/>Lock")]
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
        [Description(":<br/>;")]
        Semicolon,
        [Description("\"<br/>'")]
        Apostrophe,
        Enter,

        Shift,
        Z,
        X,
        C,
        V,
        B,
        N,
        M,
        [Description("&lt;<br/>,")]
        Comma,
        [Description("&gt;<br/>.")]
        Fullstop,
        [Description("?<br/>/")]
        Slash,

        Ctrl,
        Win,
        Alt,
        [Description("")]
        Space,
        Func,

        [Description("Prt<br/>Scr")]
        PrintScreen,
        [Description("Scroll<br/>Lock")]
        ScrollLock,
        [Description("Pause<br/>Break")]
        PauseBreak,
        [Description("Ins")]
        Insert,
        Home,
        [Description("Page<br/>Up")]
        PageUp,
        [Description("Del")]
        Delete,
        End,
        [Description("Page<br/>Dn")]
        PageDown,

        [Description("^")]
        Up,
        [Description("<")]
        Left,
        [Description("v")]
        Down,
        [Description(">")]
        Right,

        Hidden,
    }
}
