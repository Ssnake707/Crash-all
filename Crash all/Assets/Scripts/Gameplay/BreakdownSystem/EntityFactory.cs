using System.Collections.Generic;
using Gameplay.BreakdownSystem.Interface;
using StaticData.Entity;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class EntityFactory : IEntityFactory
    {
        private StaticDataEntitySettings _entitySettings;

        public EntityFactory(StaticDataEntitySettings staticDataEntitySettings) => 
            _entitySettings = staticDataEntitySettings;

        public void AddEntity(Transform transform)
        {
            IEntity entity = transform.AddComponent<Entity>();
            entity.Construct(this, _entitySettings);
        }

        public void CreateEntity(List<IDestroyedPiece> destroyedPieces)
        {
            GameObject gameObjectEntity = new GameObject();
            IEntity entity = gameObjectEntity.AddComponent<Entity>();
            entity.Construct(this, _entitySettings);
            entity.SetDestroyedPieces(destroyedPieces);
            gameObjectEntity.AddComponent<Rigidbody>();
        }
    }
}