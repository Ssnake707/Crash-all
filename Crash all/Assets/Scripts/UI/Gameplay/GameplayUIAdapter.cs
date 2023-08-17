using UI.Gameplay.Interface;
using UnityEngine;

namespace UI.Gameplay
{
    public class GameplayUIAdapter : IGameplayUIAdapter
    {
        private IGameplayView _gameplayView;
        private IGameplayUIModel _gameplayUIModel;

        public GameplayUIAdapter(IGameplayView gameplayView, IGameplayUIModel gameplayUIModel)
        {
            _gameplayView = gameplayView;
            _gameplayUIModel = gameplayUIModel;
            _gameplayView.SetAdapter(this);
            _gameplayUIModel.SetGameplayUIAdapter(this);
        }
        
    }
}