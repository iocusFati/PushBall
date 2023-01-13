using UnityEngine;

namespace DefaultNamespace.UI.Data
{
    [CreateAssetMenu(fileName = "UIData", menuName = "StaticData/UIData")]
    public class UIStaticData : ScriptableObject
    {
        [Header("LevelMenu")]
        public Color UnlockColor;
        public float LerpBy;
    }
}