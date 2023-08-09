using System.Collections.Generic;
using Gameplay.BreakdownSystem.Interface;
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

        public void Construct()
        {
            ConnectedTo = new List<IDestroyedPiece>();

            _startPos = transform.position;
            _startRotating = transform.rotation;
            _startScale = transform.localScale;

            transform.localScale *= 1.02f;
        }

        public void Damage(Vector3 force)
        {
        }

        public void Drop()
        {
        }

        public void MakeStatic()
        {
            Destroy(GetComponent<Rigidbody>());
            _configured = true;

            transform.localScale = _startScale;
            transform.position = _startPos;
            transform.rotation = _startRotating;
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