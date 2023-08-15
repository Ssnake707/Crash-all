using Data;
using Gameplay.BaseEntitiesController;
using Gameplay.BasePlayer;
using Gameplay.Game.Interfaces;
using Infrastructure.Factory.Interface;
using Services.PersistentProgress;

namespace Gameplay.Game
{
    public class GameController : IGameController, ISavedProgress
    {
        private readonly IEntitiesController _entitiesController;
        private readonly IMainGameplayFactory _mainGameplayFactory;
        private readonly PlayerMediator _playerMediator;

        public GameController(IMainGameplayFactory mainGameplayFactory, IEntitiesController entitiesController,
            PlayerMediator playerMediator)
        {
            _playerMediator = playerMediator;
            _entitiesController = entitiesController;
            _mainGameplayFactory = mainGameplayFactory;
        }

        public void LoadProgress(DataGame progress)
        {
        }

        public void UpdateProgress(DataGame progress)
        {
        }
    }
}