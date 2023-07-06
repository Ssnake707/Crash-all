using Infrastructure.States.Interface;
using Services.StaticData;
using Zenject;

namespace Infrastructure.States
{
    public class InitSDKState : IState
    {
        private readonly IStaticDataService _staticDataService;
        private GameStateMachine _stateMachine;

        [Inject]
        public InitSDKState(IStaticDataService staticDataService) => 
            _staticDataService = staticDataService;

        public void Init(GameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter() => 
            _stateMachine.Enter<LoadLevelState, string>(_staticDataService.Scenes.MainScene);

        public void Exit()
        {
        }
    }
}