using Gameplay.BreakdownSystem.Interface;
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
    }
}