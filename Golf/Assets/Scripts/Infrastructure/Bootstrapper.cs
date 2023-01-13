using System;
using Infrastructure.Services;
using Infrastructure.States;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner, IUpdatableLoop, ILateUpdatableLoop
    {
        public event Action OnUpdate;
        public event Action OnLateUpdate;

        private void Awake()
        {
            SceneLoader sceneLoader = new SceneLoader(this);
            GameStateMachine gameStateMachine = new GameStateMachine(
                sceneLoader, AllServices.Container, this, this, this);

            gameStateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }

        public void Update() => 
            OnUpdate.Invoke();

        public void LateUpdate()
        {
            OnLateUpdate.Invoke();
        }
    }

    public interface ILateUpdatableLoop
    {
        public event Action OnLateUpdate;
        public void LateUpdate();
    }
}
