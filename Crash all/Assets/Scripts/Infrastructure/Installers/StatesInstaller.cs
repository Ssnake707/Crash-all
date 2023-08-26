using Infrastructure.Factory;
using Infrastructure.Factory.Interface;
using Infrastructure.States;
using Zenject;

namespace Infrastructure.Installers
{
    public class StatesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindBootstrapState();
            BindLoadProgressState();
            BindInitSDKState();
            BindLoadLevelState();
            BindGameLoopState();
            BindGameStateMachine();
        }

        private void BindGameStateMachine() =>
            Container
                .Bind<GameStateMachine>()
                .AsSingle()
                .NonLazy();

        private void BindBootstrapState() =>
            Container
                .Bind<BootstrapState>()
                .AsSingle();

        private void BindInitSDKState() =>
            Container
                .Bind<InitSDKState>()
                .AsSingle();

        private void BindLoadProgressState() =>
            Container
                .Bind<LoadProgressState>()
                .AsSingle();

        private void BindLoadLevelState() =>
            Container
                .Bind<LoadLevelState>()
                .AsSingle();

        private void BindGameLoopState() =>
            Container
                .Bind<MainGameLoopState>()
                .AsSingle();

    }
}