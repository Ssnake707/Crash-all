using Gameplay.BreakdownSystem.Interface;
using StaticData.Entity;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class EntitiesController : MonoBehaviour
    {
        [SerializeField] private StaticDataEntitySettings _entitySettings;
        [SerializeField] private Entity[] _entities;
        private IEntityFactory _entityFactory;
        private int _totalDestroyObjects;

        private void Awake()
        {
            _totalDestroyObjects = 0;
            _entityFactory = new EntityFactory(_entitySettings);
            InitEntities();
        }

        private void InitEntities()
        {
            foreach (Entity item in _entities)
            {
                _totalDestroyObjects += item.transform.childCount;
                item.Construct(_entityFactory, _entitySettings);
            }
        }
    }
}