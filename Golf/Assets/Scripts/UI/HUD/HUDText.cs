using System;
using TMPro;
using UnityEngine;

namespace UI.HUD
{
    public class HUDText : MonoBehaviour, IHUDText
    {
        [SerializeField] private TextMeshProUGUI _score;

        private int _coinsPicked;
        private int _coinsInScene;

        public void Initialize(int coinsInScene)
        {
            _coinsPicked = 0;
            _coinsInScene = coinsInScene;
            var text = String.Concat(_coinsPicked, "/", _coinsInScene);
            Debug.Log(text);
        }

        public void UpdateText()
        {
            _coinsPicked++;
            _score.text = String.Concat(_coinsPicked, "/", _coinsInScene); 
        }
    }
}