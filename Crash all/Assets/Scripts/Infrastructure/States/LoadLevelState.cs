using Infrastructure.SceneLoaders;
using Infrastructure.States.Interface;
using Zenject;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private GameStateMachine _stateMachine;

        [Inject]
        public LoadLevelState(ISceneLoader sceneLoader,
            LoadingCurtain loadingCurtain)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }

        public void Init(GameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            //_gameFactory.CleanUp();
            //_gameFactory.WarmUp(sceneName);
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private void OnLoaded()
        {
            //_gameFactory.Init();
        }
    }
}