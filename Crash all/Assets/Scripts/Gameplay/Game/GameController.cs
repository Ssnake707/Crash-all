using Data;
using Gameplay.BaseEntitiesController;
using Gameplay.BasePlayer;
using Gameplay.Game.Interfaces;
using Infrastructure.Factory.Interface;
using Services.PersistentProgress;
using StaticData.Infrastructure;
using UnityEngine;

namespace Gameplay.Game
{
    public class GameController : IGameController
    {
        private readonly IEntitiesController _entitiesController;
        private readonly IMainGameplayFactory _mainGameplayFactory;
        private readonly PlayerMediator _playerMediator;
        private readonly IPersistentProgressService _progressService;
        private readonly StaticDataLevels _dataLevels;

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

        private void LevelComplete()
        {
            _progressService.Progress.DataLevels.CountFinishLevel++;
            _progressService.Progress.DataLevels.CurrentLevel = 
                _progressService.Progress.DataLevels.CountFinishLevel >= _dataLevels.TotalLevels
                ? Random.Range(1, _dataLevels.TotalLevels + 1)
                : _progressService.Progress.DataLevels.CurrentLevel + 1;
            _mainGameplayFactory.CreateNewLevel();
        }
    }
}