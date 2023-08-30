using Infrastructure.Factory.Interface;
using Zenject;

namespace Gameplay
{
    public class InstallerMainGameplay : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameFactory();
        }
        
        private void BindGameFactory() => 
            Container.Bind<ILevelFactory>().To<MainGameplayFactory>().AsSingle().NonLazy();
    }
}