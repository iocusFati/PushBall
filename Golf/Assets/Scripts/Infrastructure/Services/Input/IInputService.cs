using System;
using UnityEngine;

namespace Infrastructure.Services.Input
{
    public interface IInputService : IService
    {
        public Vector2 GetMovement(Action OnMouseButtonUp);
        public void BlockInput();
        public void UnlockInput();
    }
}