using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.AssetManagement;
using Infrastructure.Factory.Interface;
using Infrastructure.States;
using Services.PersistentProgress;
using Services.SaveLoad;
using Zenject;

namespace Infrastructure.Factory
{
    public abstract class AbstractLevelFactory : ILevelFactory
    {
        protected IPersistentProgressService ProgressService { get; }
        protected IAssetProvider AssetProvider { get; }
        protected GameStateMachine StateMachine { get; }
        private List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        private List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        
        protected readonly DiContainer DiContainer;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ICoroutineRunnerWithDestroyEvent _coroutineRunnerWithDestroyEvent;

        [Inject]
        protected AbstractLevelFactory(IPersistentProgressService progressService,
            ISaveLoadService saveLoadService,
            IAssetProvider assetProvider,
            GameStateMachine stateMachine, 
            DiContainer diContainer,
            ICoroutineRunnerWithDestroyEvent coroutineRunnerWithDestroyEvent)
        {
            _coroutineRunnerWithDestroyEvent = coroutineRunnerWithDestroyEvent;
            DiContainer = diContainer;
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