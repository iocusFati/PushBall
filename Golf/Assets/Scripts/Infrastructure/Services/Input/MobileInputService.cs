using System;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class MobileInputService : InputService
    {
        private Vector2 _touchPosition1;
        private Vector2 _movement;

        public override Vector2 GetMovement(Action OnMouseButtonUp)
        {
            if (_inputBlocked || UnityEngine.Input.touchCount <= 0)
                return default;

            var touch = UnityEngine.Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) 
                _touchPosition1 = touch.position;

            if (touch.phase is TouchPhase.Moved or TouchPhase.Stationary)
            {
                Vector2 touchPosition2 = touch.position;
                _movement = touchPosition2 - _touchPosition1;

                return _movement;
            }

            if (touch.phase == TouchPhase.Ended) 
                OnMouseButtonUp?.Invoke();

            return default;        }
    }
}