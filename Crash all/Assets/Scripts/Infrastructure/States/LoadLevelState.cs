using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.SceneLoaders;
using Infrastructure.States.Interface;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ISaveLoadService _saveLoadService;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentProgressService _progress;
        private readonly IAssetProvider _assetProvider;
        private GameStateMachine _stateMachine;
        private ILevel _level;

        [Inject]
        public LoadLevelState(ISceneLoader sceneLoader, ISaveLoadService saveLoadService,
            LoadingCurtain loadingCurtain,
            IStaticDataService staticDataService,
            IPersistentProgressService progress,
            IAssetProvider assetProvider)
        {
            _sceneLoader = sceneLoader;
            _saveLoadService = saveLoadService;
            _loadingCurtain = loadingCurtain;
            _staticDataService = staticDataService;
            _progress = progress;
            _assetProvider = assetProvider;
        }

        public void Init(GameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _level?.Cleanup();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() =>
            _loadingCurtain.Hide();

        private void OnLoaded()
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case SceneNames.Main:
                    _level = new GameFactory(_progress, _saveLoadService, _staticDataService, _assetProvider, _stateMachine);
                    break;
            }

            _level.Init();
            _level.NextState(_stateMachine);
        }
    }
}