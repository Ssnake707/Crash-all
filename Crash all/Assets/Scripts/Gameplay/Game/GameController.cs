using Cinemachine;
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
        private readonly IMainGameplayFactory _mainGameplayFactory;
        private readonly PlayerMediator _playerMediator;
        private readonly IPersistentProgressService _progressService;
        private readonly StaticDataLevels _dataLevels;
        private readonly float _totalCoinsOnLevel;
        private IGameplayUIAdapter _gameplayUIAdapter;
        private CinemachineVirtualCamera _playerCamera;
        private CinemachineVirtualCamera _playerCameraWin;

        public GameController(IMainGameplayFactory mainGameplayFactory,
            IPersistentProgressService progressService,
            StaticDataLevels dataLevels,
            PlayerMediator playerMediator,
            float totalCoinsOnLevel,
            CinemachineVirtualCamera playerCamera,
            CinemachineVirtualCamera playerCameraWin)
        {
            _playerCameraWin = playerCameraWin;
            _playerCamera = playerCamera;
            _totalCoinsOnLevel = totalCoinsOnLevel;
            _dataLevels = dataLevels;
            _progressService = progressService;
            _playerMediator = playerMediator;
            _mainGameplayFactory = mainGameplayFactory;
        }

        public void DestroyPiece(int totalPieces, int totalDestroyedPieces)
        {
            AddCoins(totalPieces);
            _gameplayUIAdapter.DestroyPiece(totalPieces, totalDestroyedPieces);
            if (totalDestroyedPieces >= totalPieces)
                LevelComplete();
        }

        public void StartGame() => 
            _playerMediator.PlayerStartGame();

        public void StopGame() => 
            _playerMediator.PlayerWin();

        public void ActivateCameraPlayer()
        {
            _playerCameraWin.gameObject.SetActive(false);
            _playerCamera.gameObject.SetActive(true);
        }

        public void ActivateCameraWin()
        {
            _playerCamera.gameObject.SetActive(false);
            _playerCameraWin.gameObject.SetActive(true);
        }

        public void NextLevel()
        {
            _playerMediator.PlayerResetAnimation();
            _mainGameplayFactory.CreateNewLevel();
        }

        public void SetGameplayUIAdapter(IGameplayUIAdapter adapter) =>
            _gameplayUIAdapter = adapter;

        private void AddCoins(int totalPieces) => 
            _progressService.Progress.DataPlayers.AddCoins(_totalCoinsOnLevel / totalPieces);

        private void LevelComplete()
        {
            _progressService.Progress.DataLevels.CountFinishLevel++;

            if (_progressService.Progress.DataLevels.CountFinishLevel >= _dataLevels.TotalLevels)
                _progressService.Progress.DataLevels.CurrentLevel = RandomNextLevel();
            else
                _progressService.Progress.DataLevels.CurrentLevel += 1;

            _gameplayUIAdapter.LevelComplete();
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