using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.BreakdownSystem.HelperForEditor
{
    public class DestroyedPieceCreator : MonoBehaviour
    {
        public List<DestroyedPieceCreator> ConnectedTo;
        public int Id;
        private Vector3 _startPos;
        private Quaternion _startRotating;
        private Vector3 _startScale;
        private bool _configured;
        
        public void SetDefaultValue()
        {
            ConnectedTo = new List<DestroyedPieceCreator>();
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

        private void AddNeighbour(Collision collision)
        {
            if (_configured) return;
            DestroyedPieceCreator neighbour = collision.gameObject.GetComponent<DestroyedPieceCreator>();
            if (neighbour == null) return;
            if (neighbour.transform.parent != transform.parent.transform) return;
            if (!ConnectedTo.Contains(neighbour))
                ConnectedTo.Add(neighbour);
        }

        private void OnCollisionEnter(Collision collision) => 
            AddNeighbour(collision);
    }
}