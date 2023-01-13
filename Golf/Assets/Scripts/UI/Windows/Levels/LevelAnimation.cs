using System;
using System.Collections;
using DefaultNamespace.UI.Data;
using Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Levels
{
    public class LevelAnimation
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Color _unlockColor;
        private readonly float _lerpBy;

        public LevelAnimation(UIStaticData uiStaticData, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _unlockColor = uiStaticData.UnlockColor;
            _lerpBy = uiStaticData.LerpBy;
        }

        public void Unlock(Image levelImage, Action unknown)
        {
            _coroutineRunner.StartCoroutine(UnlockLevel(levelImage));
        }

        private IEnumerator UnlockLevel(Image levelImage)
        {
            Color initialColor = levelImage.color;
            while (levelImage.color != _unlockColor)
            {
                Debug.Log("lerp");
                levelImage.color = Color.Lerp(levelImage.color, _unlockColor, _lerpBy);
                yield return null;
            }
        }
    }
}