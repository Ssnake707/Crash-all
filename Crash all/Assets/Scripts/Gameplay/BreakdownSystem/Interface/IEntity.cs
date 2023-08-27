using System.Collections.Generic;
using StaticData.Entity;
using UnityEngine;

namespace Gameplay.BreakdownSystem.Interface
{
    public interface IEntity
    {
        GameObject GameObject { get; }
        void Construct(IEntityFactory entityFactory, StaticDataEntity staticDataEntity);
        void RecalculateEntity();
        void SetDestroyedPieces(List<IDestroyedPiece> destroyedPieces);
    }
}