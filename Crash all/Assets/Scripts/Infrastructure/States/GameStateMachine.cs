using System;
using System.Collections.Generic;
using Infrastructure.States.Interface;
using Zenject;

namespace Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        [Inject]
        public GameStateMachine(BootstrapState bootstrapState,
            LoadProgressState loadProgressState,
            LoadLevelState loadLevelState,
            GameLoopState gameLoopState,
            InitSDKState initSDKState)
        {
            bootstrapState.Init(this);
            loadProgressState.Init(this);
            loadLevelState.Init(this);
            gameLoopState.Init(this);
            initSDKState.Init(this);
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = bootstrapState,
                [typeof(LoadProgressState)] = loadProgressState,
                [typeof(LoadLevelState)] = loadLevelState,
                [typeof(GameLoopState)] = gameLoopState,
                [typeof(InitSDKState)] = initSDKState
            };

            Enter<BootstrapState>();
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPaylaod>(TPaylaod paylaod) where TState : class, IPayloadedState<TPaylaod>
        {
            TState state = ChangeState<TState>();
            state.Enter(paylaod);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}