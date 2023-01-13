using System;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        protected bool _inputBlocked { get; set; }

        public virtual Vector2 GetMovement(Action OnMouseButtonUp)
        {
            return default;
        }

        public void BlockInput() => 
            _inputBlocked = true;

        public void UnlockInput() => 
            _inputBlocked = false;
    }
}