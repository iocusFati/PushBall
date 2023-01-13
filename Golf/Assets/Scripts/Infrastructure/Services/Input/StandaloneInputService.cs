using System;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class StandaloneInputService : InputService
    {
        private Vector2 _mousePosition1;
        private Vector2 _movement;

        public override Vector2 GetMovement(Action OnMouseButtonUp)
        {
            if (_inputBlocked)
                return default;
            
            if (UnityEngine.Input.GetMouseButtonDown(0)) 
                _mousePosition1 = UnityEngine.Input.mousePosition;

            if (UnityEngine.Input.GetMouseButton(0))
            {
                Vector2 mousePosition2 = UnityEngine.Input.mousePosition;
                _movement = mousePosition2 - _mousePosition1;

                return _movement;
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                OnMouseButtonUp?.Invoke();
            }

            return default;
        }
    }
}