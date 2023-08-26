using Infrastructure.Factory;
using Infrastructure.Factory.Interface;
using Zenject;

namespace Gameplay
{
    public class SceneInstallerMainGameplay : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameFactory();
        }
        
        private void BindGameFactory() => 
            Container.Bind<ILevelFactory>().To<MainGameplayFactory>().AsSingle().NonLazy();
    }
}