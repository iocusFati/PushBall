using System;
using UI.Windows;

namespace UI.Services
{
    [Serializable]
    public class WindowConfig
    {
        public WindowID WindowID;
        public Window Prefab;
    }
}