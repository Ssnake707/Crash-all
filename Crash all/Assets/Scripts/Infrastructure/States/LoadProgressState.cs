using Data;
using Infrastructure.States.Interface;
using Services.PersistentProgress;
using Services.SaveLoad;
using Zenject;

namespace Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ICoroutineRunner _coroutineRunner;
        private GameStateMachine _stateMachine;

        [Inject]
        public LoadProgressState(IPersistentProgressService progressService, 
            ISaveLoadService saveLoadService)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Init(GameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter()
        {
            LoadProgressOrInitNew();
            _saveLoadService.SaveProgress();
            _stateMachine.Enter<InitSDKState>();
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew() =>
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();

        private DataGame NewProgress() =>
            new DataGame
            {
                DataLevels = new DataLevels()
                {
                    CurrentLevel = 1,
                    CountFinishLevel = 0
                },
                DataPlayers = new DataPlayers()
                {
                    Coins = 0
                }
            };
    }
}