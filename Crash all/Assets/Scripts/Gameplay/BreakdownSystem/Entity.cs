using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.BreakdownSystem.Interface;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class Entity : MonoBehaviour, IEntity
    {
        private List<IDestroyedPiece> _destroyedPieces;
        private IEntityFactory _entityFactory;

        public void Construct(IEntityFactory entityFactory)
        {
            _entityFactory = entityFactory;
        }

        private void Awake()
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
                destroyedPiece.Construct();
                _destroyedPieces.Add(destroyedPiece);
            }
        }

        private void RecalculateEntity()
        {
            foreach (IDestroyedPiece destroyedPiece in _destroyedPieces)
                destroyedPiece.IsVisited = false;
        }

        private void BreadthFistSearch(DestroyedPiece startDestroyedPiece)
        {
            Queue<IDestroyedPiece> queue = new Queue<IDestroyedPiece>();
            startDestroyedPiece.IsVisited = true;
            queue.Enqueue(startDestroyedPiece);

            while (queue.Count > 0)
            {
                IDestroyedPiece destroyedPiece = queue.Dequeue();
                foreach (IDestroyedPiece piece in destroyedPiece.ConnectedTo)
                {
                    if (piece.IsVisited) continue;
                    piece.IsVisited = true;
                    queue.Enqueue(piece);
                }
            }
        }

        private IEnumerator RunPhysicsSteps(int stepCount)
        {
            for (int i = 0; i < stepCount; i++)
                yield return new WaitForFixedUpdate();

            foreach (IDestroyedPiece piece in _destroyedPieces)
                piece.MakeStatic();

            transform.AddComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            IDestroyedPiece destroyedPiece = other.GetContact(0).thisCollider.transform.GetComponent<IDestroyedPiece>();
            if (destroyedPiece == null) return;
        }
    }
}