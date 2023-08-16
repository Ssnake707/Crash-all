using System.Collections.Generic;
using StaticData.Entity;
using UnityEngine;

namespace Gameplay.BreakdownSystem.Interface
{
    public interface IEntityFactory
    {
        public void AddEntity(Transform transform);
        void CreateEntity(List<IDestroyedPiece> destroyedPieces, StaticDataEntity staticDataEntity);
    }
}