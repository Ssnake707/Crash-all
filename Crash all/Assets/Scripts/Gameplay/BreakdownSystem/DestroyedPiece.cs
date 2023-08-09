using System.Collections.Generic;
using Gameplay.BreakdownSystem.Interface;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class DestroyedPiece : MonoBehaviour, IDestroyedPiece
    {
        public bool IsVisited { get; set; }
        public List<IDestroyedPiece> ConnectedTo { get; private set; }

        private Vector3 _startPos;
        private Quaternion _startRotating;
        private Vector3 _startScale;
        
        private bool _configured = false;
        private IEntity _entity;

        public void Construct(IEntity entity)
        {
            _entity = entity;
            ConnectedTo = new List<IDestroyedPiece>();

            _startPos = transform.position;
            _startRotating = transform.rotation;
            _startScale = transform.localScale;

            transform.localScale *= 1.02f;
        }

        public void MakeStatic()
        {
            Destroy(GetComponent<Rigidbody>());
            _configured = true;

            transform.localScale = _startScale;
            transform.position = _startPos;
            transform.rotation = _startRotating;
        }
        
        public void Collision(Collision collision)
        {
            if (ConnectedTo.Count == 0) return;
            transform.parent = null;
            ConnectedTo.Clear();
            Rigidbody rigidBody = transform.AddComponent<Rigidbody>();
            rigidBody.AddForce(collision.impulse);
            _entity.RecalculateEntity();
        }

        private void OnCollisionEnter(Collision collision) => 
            AddNeighbour(collision);

        private void AddNeighbour(Collision collision)
        {
            if (_configured) return;
            IDestroyedPiece neighbour = collision.gameObject.GetComponent<IDestroyedPiece>();
            if (neighbour == null) return;

            if (!ConnectedTo.Contains(neighbour))
                ConnectedTo.Add(neighbour);
        }
    }
}