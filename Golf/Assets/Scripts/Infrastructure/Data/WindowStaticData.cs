using System.Collections.Generic;
using UI.Services;
using UnityEngine;

namespace Infrastructure.Data
{
    [CreateAssetMenu(fileName = "WindowStaticData", menuName = "StaticData/WindowStaticData", order = 0)]
    public class WindowStaticData : ScriptableObject
    {
        public List<WindowConfig> Configs;
    }
}