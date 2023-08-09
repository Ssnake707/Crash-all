using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class DestroyedPiece : MonoBehaviour
    {
        [HideInInspector] public bool _visited = false;
        public List<DestroyedPiece> _connectedTo;

        private Vector3 _startPos;
        private Quaternion _startRotating;
        private Vector3 _startScale;

        private bool _configured = false;

        public void Construct()
        {
            _connectedTo = new List<DestroyedPiece>();

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
            DestroyedPiece neighbour = collision.gameObject.GetComponent<DestroyedPiece>();
            if (!neighbour) return;

            if (!_connectedTo.Contains(neighbour))
                _connectedTo.Add(neighbour);
        }
    }
}