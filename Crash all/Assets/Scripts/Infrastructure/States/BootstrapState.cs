using Infrastructure.AssetManagement;
using Infrastructure.SceneLoaders;
using Infrastructure.States.Interface;
using Services.StaticData;
using Zenject;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string InitialScene = "Boot";
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataService _staticDataService;
        private readonly LoadingCurtain _loadingCurtain;
        private GameStateMachine _stateMachine;
        private IAssetProvider _assetProvider;

        [Inject]
        public BootstrapState(ISceneLoader sceneLoader,
            IStaticDataService staticDataService,
            IAssetProvider assetProvider,
            LoadingCurtain loadingCurtain)
        {
            _assetProvider = assetProvider;
            _sceneLoader = sceneLoader;
            _staticDataService = staticDataService;
            _loadingCurtain = loadingCurtain;
        }

        public void Init(GameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter()
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(InitialScene, EnterLoadLevel);
        }

        public void Exit() =>
            _loadingCurtain.Hide();

        private async void EnterLoadLevel()
        {
            _assetProvider.InitializeAddressables();
            await _staticDataService.LoadAsync();
            _stateMachine.Enter<LoadProgressState>();
        }
    }
}