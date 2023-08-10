using System.Collections.Generic;
using Gameplay.BasePlayer;
using Gameplay.BreakdownSystem.Interface;
using StaticData.Entity;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class Entity : MonoBehaviour, IEntity
    {
        [SerializeField] private StaticDataEntity _dataEntity;
        private List<IDestroyedPiece> _destroyedPieces;
        private IEntityFactory _entityFactory;
        private StaticDataEntitySettings _entitySettings;

        public void Construct(IEntityFactory entityFactory, StaticDataEntitySettings entitySettings)
        {
            _entityFactory = entityFactory;
            _entitySettings = entitySettings;
        }

        public void InitDestroyedPieces()
        {
            
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
                {
                    //TODO: Создать новый entity    
                    _entityFactory.CreateEntity(newEntity);
                }
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
            if (_entitySettings.MinImpulseForDestroy > collision.impulse.magnitude) return;
            IDestroyedPiece destroyedPiece = collision.GetContact(0).thisCollider.transform.GetComponent<IDestroyedPiece>();
            if (destroyedPiece == null) return;
            if (collision.rigidbody)
            {
                if (collision.transform.TryGetComponent<PlayerMediator>(out PlayerMediator playerMediator))
                {
                    collision.rigidbody.velocity = Vector3.zero;
                }
            }
            destroyedPiece.Collision(collision);
        }
    }
}