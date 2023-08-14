using Infrastructure.AssetManagement;
using Infrastructure.States;
using Services.PersistentProgress;
using Services.SaveLoad;
using Zenject;

namespace Infrastructure.Factory
{
    public class MainGameplayFactory : AbstractLevelFactory
    {
        [Inject]
        public MainGameplayFactory(IPersistentProgressService progressService, ISaveLoadService saveLoadService,
            IAssetProvider assetProvider,
            GameStateMachine stateMachine) : base(progressService, saveLoadService,
            assetProvider, stateMachine)
        {
        }

        public override void Init()
        {
            StateMachine.Enter<GameLoopState>();
        }
    }
}