using System.Collections.Generic;
using Gameplay.BreakdownSystem.Interface;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class DestroyedPiece : MonoBehaviour, IDestroyedPiece
    {
        public int Id => _id;
        public Transform Transform => transform;
        public bool IsVisited { get; set; }
        public bool IsDisconnect { get; set; }
        public List<IDestroyedPiece> ConnectedTo { get; private set; }

        [SerializeField] private int _id;
        private IEntity _entity;

        public void SetDefaultValue(IEntity entity)
        {
            SetEntity(entity);
            ConnectedTo = new List<IDestroyedPiece>();
        }

        public void SetEntity(IEntity entity) =>
            _entity = entity;

        public void Collision(Collision collision)
        {
            if (ConnectedTo.Count == 0) return;
            DisconnectPiece(collision);
            _entity.RecalculateEntity();
        }

        private void DisconnectPiece(Collision collision)
        {
            transform.parent = null;
            ConnectedTo.Clear();
            IsDisconnect = true;
            Rigidbody rigidBody = transform.AddComponent<Rigidbody>();
            rigidBody.AddForce(collision.impulse);
        }
    }
}