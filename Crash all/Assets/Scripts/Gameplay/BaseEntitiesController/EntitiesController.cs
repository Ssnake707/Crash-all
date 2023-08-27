using System.Collections.Generic;
using Gameplay.BreakdownSystem;
using Gameplay.BreakdownSystem.Interface;
using Gameplay.Game.Interfaces;
using StaticData.Entity;
using UnityEngine;

namespace Gameplay.BaseEntitiesController
{
    public class EntitiesController : MonoBehaviour, IEntitiesController
    {
        [SerializeField] private StaticDataEntitySettings _entitySettings;
        [SerializeField] private Entity[] _entities;
        [SerializeField] private DestroyedPiece[] _destroyedPiece;
        private readonly List<IEntity> _allEntities = new List<IEntity>();
        private IEntityFactory _entityFactory;
        private int _totalPieces;
        private int _totalDestroyedPieces = 0;
        private IGameController _gameController;
        private GameObject _gameObject;

        public GameObject GameObject => gameObject;
        public void CleanUp()
        {
            foreach (IEntity item in _allEntities) 
                Destroy(item.GameObject);
        }

        public void AddEntity(IEntity entity) => 
            _allEntities.Add(entity);

        public void SetGameController(IGameController gameController) =>
            _gameController = gameController;
        
        private void Awake()
        {
            _totalPieces = _destroyedPiece.Length;
            _entityFactory = new EntityFactory(this);
            InitEntities();
        }

        private void InitEntities()
        {
            foreach (Entity item in _entities)
            {
                _allEntities.Add(item);
                _totalPieces += item.transform.childCount;
                item.Construct(_entityFactory);
                item.InitDestroyedPieces();
            }
        }

        public void TriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<IDestroyedPiece>(out IDestroyedPiece destroyedPiece)) return;
            _totalDestroyedPieces++;
            Destroy(other.gameObject);
            _gameController.DestroyPiece(_totalPieces, _totalDestroyedPieces);
            
            if ((_totalPieces - _totalDestroyedPieces) <= _entitySettings.CountForShowPointerArrow)
            {
                // TODO: Активировать PointerArrow и передать ей все объекты
            }
        }
    }
}