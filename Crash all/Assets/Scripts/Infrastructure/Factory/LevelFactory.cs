using System.Collections.Generic;
using Infrastructure.AssetManagement;
using Infrastructure.States;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;

namespace Infrastructure.Factory
{
    public abstract class LevelFactory : ILevel
    {
        protected List<ISavedProgressReader> ProgressReaders => _progressReaders;
        protected List<ISavedProgress> ProgressWriters => _progressWriters;
        protected IPersistentProgressService ProgressService => _progressService;
        protected IStaticDataService StaticDataService => _staticDataService;
        protected IAssetProvider AssetProvider => _assetProvider;
        protected GameStateMachine GameStateMachine => _gameStateMachine;
        
        private readonly IAssetProvider _assetProvider;
        private readonly List<ISavedProgressReader> _progressReaders = new List<ISavedProgressReader>();
        private readonly List<ISavedProgress> _progressWriters = new List<ISavedProgress>();
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly GameStateMachine _gameStateMachine;

        protected LevelFactory(IPersistentProgressService progressService, 
            ISaveLoadService saveLoadService, 
            IStaticDataService staticDataService, 
            IAssetProvider assetProvider, 
            GameStateMachine gameStateMachine)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
        }

        public virtual void Cleanup()
        {
            _saveLoadService.SaveProgress(_progressWriters);
            _progressReaders.Clear();
            _progressWriters.Clear();
            _assetProvider.CleanUp();
        }

        public virtual void Init() => 
            WarmUp();

        public abstract void NextState(GameStateMachine stateMachine);

        protected virtual void WarmUp()
        {
        }

        protected void Register<T>(T registerObject)
        {
            if (registerObject is ISavedProgress progressWriter)
                _progressWriters.Add(progressWriter);
            if (registerObject is ISavedProgressReader progressReader)
                _progressReaders.Add(progressReader);
        }
    }
}