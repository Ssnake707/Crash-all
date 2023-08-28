using System.Collections.Generic;
using Gameplay.BaseEntitiesController;
using Gameplay.BreakdownSystem.Interface;
using StaticData.Entity;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class EntityFactory : IEntityFactory
    {
        private EntitiesController _entitiesController;

        public EntityFactory(EntitiesController entitiesController) => 
            _entitiesController = entitiesController;

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