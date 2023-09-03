using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.BasePlayer;
using Gameplay.BreakdownSystem.Interface;
using StaticData.Entity;
using UI.BasePointerArrow.Interface;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class Entity : MonoBehaviour, IEntity, ITargetPointerArrow
    {
        public GameObject GameObject => this.gameObject;
        public bool IsActive => _destroyedPieces.Count > 0;
        public Vector3 Position => transform.position;
        
        [SerializeField] private StaticDataEntity _dataEntity;
        [SerializeField] private Transform _centerOfMass;
        private List<IDestroyedPiece> _destroyedPieces;
        private IEntityFactory _entityFactory;
        private Rigidbody _rigidbody;

        private void Awake() => 
            _rigidbody = GetComponent<Rigidbody>();

        public void Construct(IEntityFactory entityFactory) => 
            _entityFactory = entityFactory;

        public void Construct(IEntityFactory entityFactory, StaticDataEntity staticDataEntity)
        {
            _entityFactory = entityFactory;
            _dataEntity = staticDataEntity;
        }

        public void InitDestroyedPieces()
        {
            if (_centerOfMass != null) 
                transform.GetComponent<Rigidbody>().centerOfMass = _centerOfMass.localPosition;
            
            _destroyedPieces = new List<IDestroyedPiece>();
            foreach (IDestroyedPiece destroyedPiece in transform.GetComponentsInChildren<IDestroyedPiece>())
                _destroyedPieces.Add(destroyedPiece);
            
            _destroyedPieces.OrderBy(x => x.Id);
            
            foreach (IDestroyedPiece destroyedPiece in _destroyedPieces)
                destroyedPiece.InitDestroyedPieces(this, _destroyedPieces,
                    _dataEntity.DestroyedPiecesIds[destroyedPiece.Id]);
        }

        public void SetDestroyedPieces(List<IDestroyedPiece> destroyedPieces)
        {
            _destroyedPieces = destroyedPieces;
            transform.position = destroyedPieces[0].Transform.position;
            foreach (IDestroyedPiece piece in destroyedPieces)
            {
                piece.Transform.parent = transform;
                piece.SetEntity(this);
            }
        }

        public List<IDestroyedPiece> GetDestroyedPieces() =>
            _destroyedPieces;

        public void RemoveDestroyedPiece(DestroyedPiece destroyedPiece)
        {
            _destroyedPieces.Remove(destroyedPiece);
            if (_destroyedPieces.Count == 0)
                Destroy(this.gameObject);
        }

        public void RecalculateEntity()
        {
            foreach (IDestroyedPiece destroyedPiece in _destroyedPieces)
                destroyedPiece.IsVisited = false;

            List<IDestroyedPiece> entity = BreadthFistSearch(_destroyedPieces[0]);
            bool isNextIteration;
            do
            {
                isNextIteration = false;
                List<IDestroyedPiece> newEntity = null;
                foreach (IDestroyedPiece destroyedPiece in _destroyedPieces)
                {
                    if (destroyedPiece.IsVisited) continue;
                    isNextIteration = true;
                    newEntity = BreadthFistSearch(destroyedPiece);
                    break;
                }

                if (newEntity != null && newEntity.Count != 1)
                    _entityFactory.CreateEntity(newEntity, _dataEntity);
            } while (isNextIteration);

            _destroyedPieces = entity;
        }

        private List<IDestroyedPiece> BreadthFistSearch(IDestroyedPiece startDestroyedPiece)
        {
            Queue<IDestroyedPiece> queue = new Queue<IDestroyedPiece>();
            startDestroyedPiece.IsVisited = true;
            queue.Enqueue(startDestroyedPiece);
            List<IDestroyedPiece> result = new List<IDestroyedPiece>();
            result.Add(startDestroyedPiece);
            while (queue.Count > 0)
            {
                IDestroyedPiece destroyedPiece = queue.Dequeue();
                foreach (IDestroyedPiece piece in destroyedPiece.ConnectedTo)
                {
                    if (piece.IsVisited) continue;
                    if (piece.IsDisconnect) continue;
                    piece.IsVisited = true;
                    result.Add(piece);
                    queue.Enqueue(piece);
                }
            }

            return result;
        }

        private void OnCollisionEnter(Collision collision)
        {
            IDestroyedPiece destroyedPiece =
                collision.GetContact(0).thisCollider.transform.GetComponent<IDestroyedPiece>();
            if (destroyedPiece == null) return;

            if (collision.rigidbody &&
                collision.transform.TryGetComponent<PlayerMediator>(out PlayerMediator playerMediator))
            {
                collision.rigidbody.velocity = Vector3.zero;
                if (_dataEntity.MinImpulsePlayerForDestroy > collision.impulse.magnitude) return;
                destroyedPiece.Collision(collision, _rigidbody.velocity);
            }
            else
            {
                if (_dataEntity.MinImpulseOtherForDestroy > collision.impulse.magnitude) return;
                destroyedPiece.Collision(collision, _rigidbody.velocity);
            }
        }
    }
}