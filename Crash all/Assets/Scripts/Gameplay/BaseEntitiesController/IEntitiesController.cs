using Gameplay.BreakdownSystem.Interface;
using Gameplay.Game.Interfaces;
using UnityEngine;

namespace Gameplay.BaseEntitiesController
{
    public interface IEntitiesController
    {
        void AddEntity(IEntity entity);
        void SetGameController(IGameController gameController);
        GameObject GameObject { get; }
        void CleanUp();
    }
}