using Gameplay.BasePlayer;
using Gameplay.BreakdownSystem.PoolParticleSystem;
using Gameplay.Game.Interfaces;
using UI.BasePointerArrow.Interface;
using UnityEngine;

namespace Gameplay.BaseEntitiesController
{
    public interface IEntitiesController
    {
        void SetGameController(IGameController gameController);
        GameObject GameObject { get; }
        void CleanUp();
        void Construct(IPointerArrowController pointerArrowController, PlayerMediator playerMediator,
            PoolParticleSystemHit poolParticleSystemHit);
    }
}