using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class Entity : MonoBehaviour
    {
        private List<DestroyedPiece> _destroyedPieces;
        private void Awake()
        {
            FillDestroyedPieces();
            StartCoroutine(RunPhysicsSteps(10));
        }

        private void FillDestroyedPieces()
        {
            _destroyedPieces = new List<DestroyedPiece>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                DestroyedPiece destroyedPiece = child.gameObject.AddComponent<DestroyedPiece>();
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
            foreach (DestroyedPiece destroyedPiece in _destroyedPieces) 
                destroyedPiece._visited = false;
        }

        private void BreadthFistSearch(DestroyedPiece startDestroyedPiece)
        {
            Queue<DestroyedPiece> queue = new Queue<DestroyedPiece>();
            startDestroyedPiece._visited = true;
            queue.Enqueue(startDestroyedPiece);

            while (queue.Count > 0)
            {
                DestroyedPiece destroyedPiece = queue.Dequeue();
                foreach (DestroyedPiece piece in destroyedPiece._connectedTo)
                {
                    if (piece._visited) continue;
                    piece._visited = true;
                    queue.Enqueue(piece);
                }
            }
        }

        private IEnumerator RunPhysicsSteps(int stepCount)
        {
            for (int i = 0; i < stepCount; i++)
                yield return new WaitForFixedUpdate();
        
            foreach(DestroyedPiece piece in _destroyedPieces) 
                piece.MakeStatic();

            transform.AddComponent<Rigidbody>();
        }
    }
}