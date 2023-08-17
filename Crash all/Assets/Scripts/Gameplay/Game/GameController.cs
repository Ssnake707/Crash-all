using Gameplay.BaseEntitiesController;
using Gameplay.BasePlayer;
using Gameplay.Game.Interfaces;
using Infrastructure.Factory.Interface;
using Services.PersistentProgress;
using StaticData.Infrastructure;
using UI.Gameplay.Interface;
using UnityEngine;

namespace Gameplay.Game
{
    public class GameController : IGameController, IGameplayUIModel
    {
        private readonly IEntitiesController _entitiesController;
        private readonly IMainGameplayFactory _mainGameplayFactory;
        private readonly PlayerMediator _playerMediator;
        private readonly IPersistentProgressService _progressService;
        private readonly StaticDataLevels _dataLevels;
        private IGameplayUIAdapter _gameplayUIAdapter;

        public GameController(IMainGameplayFactory mainGameplayFactory,
            IPersistentProgressService progressService,
            StaticDataLevels dataLevels,
            IEntitiesController entitiesController,
            PlayerMediator playerMediator)
        {
            _dataLevels = dataLevels;
            _progressService = progressService;
            _playerMediator = playerMediator;
            _entitiesController = entitiesController;
            _mainGameplayFactory = mainGameplayFactory;
        }

        public void DestroyPiece(int totalPieces, int totalDestroyedPieces)
        {
            if (totalDestroyedPieces >= totalPieces)
                LevelComplete();
        }

        public void SetGameplayUIAdapter(IGameplayUIAdapter adapter) => 
            _gameplayUIAdapter = adapter;

        private void LevelComplete()
        {
            _progressService.Progress.DataLevels.CountFinishLevel++;
            
            if (_progressService.Progress.DataLevels.CountFinishLevel >= _dataLevels.TotalLevels)
                _progressService.Progress.DataLevels.CurrentLevel = RandomNextLevel();
            else
                _progressService.Progress.DataLevels.CurrentLevel += 1;

            _mainGameplayFactory.CreateNewLevel();
        }

        private int RandomNextLevel()
        {
            int currentLevel = _progressService.Progress.DataLevels.CurrentLevel;
            int nextLevel;
            do 
                nextLevel = Random.Range(1, _dataLevels.TotalLevels + 1);
            while (currentLevel == nextLevel);

            return nextLevel;
        }
    }
}