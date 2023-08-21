using System.Threading.Tasks;
using Cinemachine;
using Gameplay.BaseEntitiesController;
using Gameplay.BasePlayer;
using Gameplay.Game;
using Gameplay.Game.Interfaces;
using Infrastructure.AssetManagement;
using Infrastructure.Factory.Interface;
using Infrastructure.States;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;
using UI.Gameplay;
using UI.Gameplay.Interface;
using UI.GeneralMenu.Interface;
using UI.WindowController;
using UI.WindowController.Interface;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factory
{
    public class MainGameplayFactory : AbstractLevelFactory, IMainGameplayFactory
    {
        private readonly IStaticDataService _staticDataService;
        
        private IEntitiesController _entitiesController;
        private PlayerMediator _playerMediator;
        private CinemachineVirtualCamera _cameraPlayer;
        private CinemachineVirtualCamera _cameraPlayerWin;
        private IGameController _gameController;
        private GameObject _mainCanvas;

        [Inject]
        public MainGameplayFactory(IPersistentProgressService progressService, ISaveLoadService saveLoadService,
            IAssetProvider assetProvider,
            GameStateMachine stateMachine,
            IStaticDataService staticDataService) : base(progressService, saveLoadService,
            assetProvider, stateMachine)
        {
            _staticDataService = staticDataService;
        }

        public override async void Init()
        {
            await CreateLevel();
            await CreatePlayer();
            await CreateCanvas();
            await CreateVirtualCameraPlayer();
            CreateGameController();

            StateMachine.Enter<GameLoopState>();
        }

        public async void CreateNewLevel()
        {
            Object.Destroy(_entitiesController.GameObject);
            await CreateLevel();
            SetPositionPlayer();
            CreateGameController();
        }

        private async Task CreateCanvas()
        {
            GameObject mainCanvasPrefab = await AssetProvider.Load<GameObject>(AssetAddress.MainCanvas);
            _mainCanvas = Object.Instantiate(mainCanvasPrefab);
            _mainCanvas.GetComponent<IGeneralMenuView>().Construct(ProgressService);
            IWindowsController windowsController = _mainCanvas.GetComponent<IWindowsController>();
            windowsController.ShowWindow(WindowType.MainMenu);
        }
        private void SetPositionPlayer() =>
            _playerMediator.SetPosition(
                _staticDataService.DataLevels.DataLevels[
                    ProgressService.Progress.DataLevels.CurrentLevel - 1].SpawnPosition);

        private void CreateGameController()
        {
            _gameController = new GameController(this,
                ProgressService,
                _staticDataService.DataLevels,
                _playerMediator, 
                _staticDataService.DataLevels.DataLevels[ProgressService.Progress.DataLevels.CurrentLevel - 1].TotalCoins,
                _cameraPlayer,
                _cameraPlayerWin);
            _entitiesController.SetGameController(_gameController);
            CreateGameplayUI();
        }
        
        private void CreateGameplayUI() => 
            new GameplayUIAdapter(_mainCanvas.GetComponent<IGameplayView>(), (IGameplayUIModel)_gameController);

        private async Task CreateVirtualCameraPlayer()
        {
            GameObject cameraPlayerWinPrefab = await AssetProvider.Load<GameObject>(AssetAddress.CameraPlayerWin);
            _cameraPlayerWin = Object.Instantiate(cameraPlayerWinPrefab).GetComponent<CinemachineVirtualCamera>();
            _cameraPlayerWin.Follow = _playerMediator.transform;
            _cameraPlayerWin.LookAt = _playerMediator.transform;
            _cameraPlayerWin.gameObject.SetActive(false);
            
            GameObject cameraPlayerPrefab = await AssetProvider.Load<GameObject>(AssetAddress.CameraPlayer);
            _cameraPlayer = Object.Instantiate(cameraPlayerPrefab).GetComponent<CinemachineVirtualCamera>();
            _cameraPlayer.Follow = _playerMediator.transform;
            _cameraPlayer.LookAt = _playerMediator.transform;
        }

        private async Task CreatePlayer()
        {
            GameObject playerPrefab = await AssetProvider.Load<GameObject>(AssetAddress.Player);
            _playerMediator = Object.Instantiate(playerPrefab,
                _staticDataService.DataLevels.DataLevels[ProgressService.Progress.DataLevels.CurrentLevel - 1].SpawnPosition,
                Quaternion.identity).GetComponent<PlayerMediator>();
        }

        private async Task CreateLevel()
        {
#if UNITY_EDITOR
            if (_staticDataService.DataLevels.AlwaysLoadLevel != -1)
                ProgressService.Progress.DataLevels.CurrentLevel = _staticDataService.DataLevels.AlwaysLoadLevel;
#endif
            GameObject levelPrefab =
                await AssetProvider.Load<GameObject>(
                    $"{AssetAddress.Level}{ProgressService.Progress.DataLevels.CurrentLevel}");
            _entitiesController = Object.Instantiate(levelPrefab).GetComponent<IEntitiesController>();
        }
    }
}