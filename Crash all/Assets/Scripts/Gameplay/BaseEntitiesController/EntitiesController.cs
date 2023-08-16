using Gameplay.BreakdownSystem;
using Gameplay.BreakdownSystem.Interface;
using Gameplay.Game.Interfaces;
using UnityEngine;

namespace Gameplay.BaseEntitiesController
{
    public class EntitiesController : MonoBehaviour, IEntitiesController
    {
        [SerializeField] private Entity[] _entities;
        private IEntityFactory _entityFactory;
        private int _totalPieces;
        private int _totalDestroyedPieces = 0;
        private IGameController _gameController;
        private GameObject _gameObject;

        public GameObject GameObject => gameObject;

        public void SetGameController(IGameController gameController) =>
            _gameController = gameController;
        
        private void Awake()
        {
            _totalPieces = 0;
            _entityFactory = new EntityFactory();
            InitEntities();
        }

        private void InitEntities()
        {
            foreach (Entity item in _entities)
            {
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
        }
    }
}