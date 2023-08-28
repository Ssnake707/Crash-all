using System.Collections.Generic;
using Gameplay.BasePlayer;
using Gameplay.BreakdownSystem;
using Gameplay.BreakdownSystem.Interface;
using Gameplay.Game.Interfaces;
using StaticData.Entity;
using UI.BasePointerArrow.Interface;
using UnityEngine;

namespace Gameplay.BaseEntitiesController
{
    public class EntitiesController : MonoBehaviour, IEntitiesController
    {
        [SerializeField] private StaticDataEntitySettings _entitySettings;
        [SerializeField] private Entity[] _entities;
        [SerializeField] private DestroyedPiece[] _destroyedPiece;
        private readonly List<ITargetPointerArrow> _targetsPointerArrow = new List<ITargetPointerArrow>();
        private IPointerArrowController _pointerArrowController;
        private PlayerMediator _playerMediator;
        private int _totalPieces;
        private int _totalDestroyedPieces = 0;
        private IGameController _gameController;
        private bool _pointerArrowActivated = false;

        public GameObject GameObject => gameObject;

        public void Construct(IPointerArrowController pointerArrowController, PlayerMediator playerMediator)
        {
            _pointerArrowController = pointerArrowController;
            _playerMediator = playerMediator;
            ActivatePointerArrowToEntity();
        }

        public void CleanUp()
        {
        }

        public void SetGameController(IGameController gameController) =>
            _gameController = gameController;

        private void Awake()
        {
            _totalPieces = _destroyedPiece.Length;
            InitEntities();
        }

        private void InitEntities()
        {
            IEntityFactory entityFactory = new EntityFactory(this);
            foreach (Entity item in _entities)
            {
                _totalPieces += item.transform.childCount;
                item.Construct(entityFactory);
                item.InitDestroyedPieces();

                _targetsPointerArrow.Add(item);
                List<IDestroyedPiece> destroyedPieces = item.GetDestroyedPieces();
                foreach (IDestroyedPiece piece in destroyedPieces)
                    _targetsPointerArrow.Add((ITargetPointerArrow)piece);
            }
        }

        public void TriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDestroyedPiece>(out IDestroyedPiece destroyedPiece))
            {
                _totalDestroyedPieces++;
                destroyedPiece.DestroyPiece();
                _gameController.DestroyPiece(_totalPieces, _totalDestroyedPieces);

                if ((_totalPieces - _totalDestroyedPieces) <= _entitySettings.CountForShowPointerArrow)
                    ActivatePointerArrowToEntity();
            }
        }

        private void ActivatePointerArrowToEntity()
        {
            if (_pointerArrowActivated) return;

            _pointerArrowActivated = true;
            _pointerArrowController.Init(_targetsPointerArrow, _playerMediator.transform);
            _pointerArrowController.Activate(true);
        }
    }
}