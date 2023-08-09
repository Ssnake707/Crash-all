using Gameplay.BreakdownSystem.Interface;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class EntitiesController : MonoBehaviour
    {
        [SerializeField] private Transform[] _environmentsDestroy;
        private IEntityFactory _entityFactory;
        private int _totalDestroyObjects;

        private void Awake()
        {
            _totalDestroyObjects = 0;
            _entityFactory = new EntityFactory();
            InitEntities();
        }

        private void InitEntities()
        {
            foreach (Transform item in _environmentsDestroy)
            {
                _totalDestroyObjects += item.childCount;
                _entityFactory.AddEntity(item);
            }
        }
    }
}