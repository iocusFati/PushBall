using System;
using Infrastructure.States.Interfaces;
using UnityEngine;

namespace Infrastructure.States
{
    public class GameLoopState : IState
    {
        public void Enter()
        {
            Debug.Log("GameLOOP");
        }

        public void Exit()
        {
            
        }
    }
}