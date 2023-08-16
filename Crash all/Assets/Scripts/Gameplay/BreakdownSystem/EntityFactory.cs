using System.Collections.Generic;
using Gameplay.BreakdownSystem.Interface;
using StaticData.Entity;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class EntityFactory : IEntityFactory
    {
        public void AddEntity(Transform transform)
        {
            IEntity entity = transform.AddComponent<Entity>();
            entity.Construct(this);
        }

        public void CreateEntity(List<IDestroyedPiece> destroyedPieces, StaticDataEntity staticDataEntity)
        {
            GameObject gameObjectEntity = new GameObject();
            IEntity entity = gameObjectEntity.AddComponent<Entity>();
            entity.Construct(this, staticDataEntity);
            entity.SetDestroyedPieces(destroyedPieces);
            gameObjectEntity.AddComponent<Rigidbody>();
        }
    }
}