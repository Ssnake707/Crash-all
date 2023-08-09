using System.Collections;
using System.Collections.Generic;
using Gameplay.BreakdownSystem.Interface;
using StaticData.Entity;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class Entity : MonoBehaviour, IEntity
    {
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
            FillDestroyedPieces();
            StartCoroutine(RunPhysicsSteps(10));
        }

        private void FillDestroyedPieces()
        {
            _destroyedPieces = new List<IDestroyedPiece>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                IDestroyedPiece destroyedPiece = child.gameObject.AddComponent<DestroyedPiece>();
                Rigidbody rigidBody = child.gameObject.AddComponent<Rigidbody>();
                rigidBody.isKinematic = false;
                rigidBody.useGravity = false;
                MeshCollider meshCollider = child.AddComponent<MeshCollider>();
                meshCollider.convex = true;
                destroyedPiece.Construct(this);
                _destroyedPieces.Add(destroyedPiece);
            }
        }

        public void RecalculateEntity()
        {
            foreach (IDestroyedPiece destroyedPiece in _destroyedPieces)
                destroyedPiece.IsVisited = false;

            bool isNextIteration = false;
            do
            {
                List<IDestroyedPiece> newEntity = BreadthFistSearch(_destroyedPieces[0]);
                if (newEntity.Count != 1)
                {
                    //TODO: Создать новый entity    
                }
                
                foreach (IDestroyedPiece destroyedPiece in _destroyedPieces)
                {
                    if (destroyedPiece.IsVisited) continue;
                    isNextIteration = true;
                }
            } while (isNextIteration);
            
            
        }

        private List<IDestroyedPiece> BreadthFistSearch(IDestroyedPiece startDestroyedPiece)
        {
            Queue<IDestroyedPiece> queue = new Queue<IDestroyedPiece>();
            startDestroyedPiece.IsVisited = true;
            queue.Enqueue(startDestroyedPiece);
            List<IDestroyedPiece> result = new List<IDestroyedPiece>();
            _destroyedPieces.Remove(startDestroyedPiece);
            result.Add(startDestroyedPiece);
            while (queue.Count > 0)
            {
                IDestroyedPiece destroyedPiece = queue.Dequeue();
                foreach (IDestroyedPiece piece in destroyedPiece.ConnectedTo)
                {
                    if (piece.IsVisited) continue;
                    piece.IsVisited = true;
                    _destroyedPieces.Remove(piece);
                    result.Add(piece);
                    queue.Enqueue(piece);
                }
            }

            return result;
        }

        private IEnumerator RunPhysicsSteps(int stepCount)
        {
            for (int i = 0; i < stepCount; i++)
                yield return new WaitForFixedUpdate();

            foreach (IDestroyedPiece piece in _destroyedPieces)
                piece.MakeStatic();

            transform.AddComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_entitySettings.MinImpulseForDestroy > collision.impulse.magnitude) return;
            IDestroyedPiece destroyedPiece = collision.GetContact(0).thisCollider.transform.GetComponent<IDestroyedPiece>();
            if (destroyedPiece == null) return;
            destroyedPiece.Collision(collision);
        }
    }
}