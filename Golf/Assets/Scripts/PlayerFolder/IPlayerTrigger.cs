using System;
using UnityEngine;

namespace PlayerFolder
{
    public interface IPlayerTrigger
    {
        public event Action<Collider> OnCoinTriggerEnter;
    }
}