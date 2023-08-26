using Infrastructure.Factory.Interface;
using Infrastructure.States.Interface;

namespace Infrastructure.States
{
    public class MainGameLoopState : IPayloadedState<ILevelFactory>
    {
        private GameStateMachine _stateMachine;
        private ILevelFactory _levelFactory;

        public void Init(GameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter(ILevelFactory levelFactory) => 
            _levelFactory = levelFactory;

        public void Exit() => 
            _levelFactory.Cleanup();
    }
}