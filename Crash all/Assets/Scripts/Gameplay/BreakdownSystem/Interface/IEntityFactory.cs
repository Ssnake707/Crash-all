using System.Collections.Generic;
using StaticData.Entity;
using UnityEngine;

namespace Gameplay.BreakdownSystem.Interface
{
    public interface IEntityFactory
    {
        void CreateEntity(List<IDestroyedPiece> destroyedPieces, StaticDataEntity staticDataEntity);
    }
}