using System.Threading.Tasks;
using Infrastructure.AssetManagement;
using Infrastructure.SceneLoaders;
using Infrastructure.States.Interface;
using Services.StaticData;
using Zenject;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataService _staticDataService;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IAssetProvider _assetProvider;
        private GameStateMachine _stateMachine;

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

        public async void Enter()
        {
            _loadingCurtain.Show();
            await InitServices();
            _sceneLoader.Load(_staticDataService.Scenes.InitialScene, EnterLoadLevel);
        }

        public void Exit() =>
            _loadingCurtain.Hide();

        private void EnterLoadLevel() => 
            _stateMachine.Enter<LoadProgressState>();

        private async Task InitServices()
        {
            _assetProvider.InitializeAddressables();
            await _staticDataService.LoadAsync();
        }
    }
}