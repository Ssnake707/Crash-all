using Gameplay.Game.Interfaces;
using UnityEngine;

namespace Gameplay.BaseEntitiesController
{
    public interface IEntitiesController
    {
        void SetGameController(IGameController gameController);
        GameObject GameObject { get; }
    }
}