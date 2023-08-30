using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.AssetManagement;
using Infrastructure.Factory.Interface;
using Infrastructure.States;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factory
{
    public abstract class AbstractLevelFactory : ILevelFactory
    {
        protected IPersistentProgressService ProgressService { get; }
        protected IAssetProvider AssetProvider { get; }
        protected IStaticDataService StaticDataService { get; }
        protected GameStateMachine StateMachine { get; }
        private List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        private List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        
        protected readonly DiContainer DiContainer;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ICoroutineRunnerWithDestroyEvent _coroutineRunnerWithDestroyEvent;

        [Inject]
        protected AbstractLevelFactory(IPersistentProgressService progressService,
            ISaveLoadService saveLoadService,
            IStaticDataService staticDataService,
            IAssetProvider assetProvider,
            GameStateMachine stateMachine, 
            DiContainer diContainer,
            ICoroutineRunnerWithDestroyEvent coroutineRunnerWithDestroyEvent)
        {
            _coroutineRunnerWithDestroyEvent = coroutineRunnerWithDestroyEvent;
            DiContainer = diContainer;
            StaticDataService = staticDataService;
            ProgressService = progressService;
            _saveLoadService = saveLoadService;
            AssetProvider = assetProvider;
            StateMachine = stateMachine;
            _coroutineRunnerWithDestroyEvent.OnDestroyEvent += OnDestroyHandler;
        }

        public abstract void Init();

        public virtual void WarmUp()
        {
        }

        public virtual void Cleanup()
        {
            _saveLoadService.SaveProgress(ProgressWriters);
            ProgressReaders.Clear();
            ProgressWriters.Clear();
            AssetProvider.CleanUp();
        }

        public virtual void SaveProgress() =>
            _saveLoadService.SaveProgress(ProgressWriters);
        
        public void IncreaseLevelOrRandomLevel()
        {
            ProgressService.Progress.DataLevels.CountFinishLevel++;

            if (ProgressService.Progress.DataLevels.CountFinishLevel >= StaticDataService.DataLevels.TotalLevels)
                ProgressService.Progress.DataLevels.CurrentLevel = RandomNextLevel();
            else
                ProgressService.Progress.DataLevels.CurrentLevel += 1;
        }
        
        private int RandomNextLevel()
        {
            int currentLevel = ProgressService.Progress.DataLevels.CurrentLevel;
            int nextLevel;
            do
                nextLevel = Random.Range(1, StaticDataService.DataLevels.TotalLevels + 1);
            while (currentLevel == nextLevel);

            return nextLevel;
        }

        private void OnDestroyHandler()
        {
            _coroutineRunnerWithDestroyEvent.OnDestroyEvent -= OnDestroyHandler;
            SaveProgress();
        }
        
        protected void Register<T>(T registerObject)
        {
            if (registerObject is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);
            if (registerObject is ISavedProgressReader progressReader)
                ProgressReaders.Add(progressReader);
        }
    }
}