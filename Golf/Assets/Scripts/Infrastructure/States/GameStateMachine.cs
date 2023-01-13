using System;
using System.Collections.Generic;
using Infrastructure.Factories;
using Infrastructure.Services;
using Infrastructure.Services.Input;
using Infrastructure.Services.Spawners;
using Infrastructure.States.Interfaces;
using UI.Services;

namespace Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitState> _states;
        private IExitState _currentState;

        public GameStateMachine(SceneLoader sceneLoader, AllServices services, IUpdatableLoop updatableLoop,
            ICoroutineRunner coroutineRunner, ILateUpdatableLoop lateUpdatableLoop)
        {
            _states = new Dictionary<Type, IExitState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(
                    this, sceneLoader, services, coroutineRunner, updatableLoop, lateUpdatableLoop),
                
                [typeof(LoadLevelState)] = new LoadLevelState(
                    this , sceneLoader, services.Single<IGameFactory>(), services.Single<ICoinSpawner>(), 
                        services.Single<IInputService>(), services.Single<IUIFactory>()),
                
                [typeof(LoadMenuState)] = new LoadMenuState(
                    sceneLoader, services.Single<IWindowService>(), services.Single<IUIFactory>()),
                
                [typeof(GameLoopState)] = new GameLoopState(),
                
                [typeof(WinState)] = new WinState(services.Single<IWindowService>(), services.Single<IInputService>())
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitState
        {
            _currentState?.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitState => 
            _states[typeof(TState)] as TState;
    }
}