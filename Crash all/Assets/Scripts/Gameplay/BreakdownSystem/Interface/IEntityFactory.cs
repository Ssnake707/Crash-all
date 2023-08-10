using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.BreakdownSystem.Interface
{
    public interface IEntityFactory
    {
        public void AddEntity(Transform transform);
        void CreateEntity(List<IDestroyedPiece> destroyedPieces);
    }
}