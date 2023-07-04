using System.Collections.Generic;
using Data;
using Infrastructure.States.Interface;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;
using Zenject;

namespace Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly List<ISavedProgressReader> _progressReaders;
        private readonly List<ISavedProgress> _progressWriters;
        private GameStateMachine _stateMachine;

        [Inject]
        public LoadProgressState(IPersistentProgressService progressService, 
            ISaveLoadService saveLoadService,
            ICoroutineRunner coroutineRunner)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _coroutineRunner = coroutineRunner;
            _coroutineRunner.OnDestroyEvent += OnDestroyHandler;
            _progressReaders = new List<ISavedProgressReader>();
            _progressWriters = new List<ISavedProgress>();
        }

        public void Init(GameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter()
        {
            LoadProgressOrInitNew();
            InformServices();
            _saveLoadService.SaveProgress(_progressWriters);
            _stateMachine.Enter<InitSDKState>();
        }

        public void Exit()
        {
        }

        private void Register<T>(T registerObject)
        {
            if (registerObject is ISavedProgress progressWriter)
                _progressWriters.Add(progressWriter);
            if (registerObject is ISavedProgressReader progressReader)
                _progressReaders.Add(progressReader);
        }

        private void OnDestroyHandler()
        {
            _coroutineRunner.OnDestroyEvent -= OnDestroyHandler;
            if (_progressService.Progress == null) return;
            _saveLoadService.SaveProgress(_progressWriters);
                
        }

        private void InformServices()
        {
            foreach (ISavedProgressReader item in _progressReaders)
                item.LoadProgress(_progressService.Progress);
        }

        private void LoadProgressOrInitNew() =>
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();

        private DataGame NewProgress() =>
            new DataGame
            {
                
            };
    }
}