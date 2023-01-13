using System;
using UnityEngine;

namespace PlayerFolder
{
    public interface ICollision
    {
        public event Action<Collision> OnWallCollisionEnterAction;
    }
}