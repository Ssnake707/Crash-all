using Infrastructure.States.Interface;
using Services.StaticData;
using Zenject;

namespace Infrastructure.States
{
    public class InitSDKState : IState
    {
        private GameStateMachine _stateMachine;
        private IStaticDataService _staticDataService;

        [Inject]
        public InitSDKState(IStaticDataService staticDataService) => 
            _staticDataService = staticDataService;

        public void Init(GameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter() => 
            _stateMachine.Enter<LoadLevelState, string>(SceneNames.Main);

        public void Exit()
        {
        }
    }
}