using Infrastructure.Factory.Interface;
using Services.StaticData;
using Zenject;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly DiContainer _diContainer;
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineRunner _coroutineRunner;
        private ILevelFactory _currentFactory;

        [Inject]
        public GameFactory(DiContainer diContainer, IStaticDataService staticDataService,
            ICoroutineRunner coroutineRunner)
        {
            _staticDataService = staticDataService;
            _diContainer = diContainer;
            _coroutineRunner = coroutineRunner;
            _coroutineRunner.OnDestroyEvent += OnDestroyHandler;
        }

        public void WarmUp(string nameScene)
        {
            if (_staticDataService.Scenes.MainScene.Equals(nameScene))
                _currentFactory = _diContainer.Instantiate<MainGameplayFactory>();
            if (_currentFactory == null) return;
            _currentFactory.WarmUp();
        }

        public void Init()
        {
            if (_currentFactory == null) return;
            _currentFactory.Init();
        }

        public void CleanUp()
        {
            if (_currentFactory == null) return;
            _currentFactory.Cleanup();
        }

        private void OnDestroyHandler()
        {
            _coroutineRunner.OnDestroyEvent -= OnDestroyHandler;
            if (_currentFactory == null) return;
            _currentFactory.SaveProgress();
        }
    }
}