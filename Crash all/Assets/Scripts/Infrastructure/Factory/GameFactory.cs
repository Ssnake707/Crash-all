using Infrastructure.AssetManagement;
using Infrastructure.States;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;

namespace Infrastructure.Factory
{
    public class GameFactory : LevelFactory
    {
        public GameFactory(IPersistentProgressService progressService, 
            ISaveLoadService saveLoadService,
            IStaticDataService staticDataService, 
            IAssetProvider assetProvider, 
            GameStateMachine gameStateMachine) :
        base(progressService, saveLoadService, staticDataService, assetProvider, gameStateMachine)
        {
        }

        public override void Init()
        {
            base.Init();
        }

        protected override void WarmUp()
        {
            base.WarmUp();
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }

        public override void NextState(GameStateMachine stateMachine) =>
            stateMachine.Enter<GameLoopState>();
    }
}