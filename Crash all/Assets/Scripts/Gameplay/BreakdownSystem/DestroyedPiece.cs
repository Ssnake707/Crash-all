using System.Collections.Generic;
using Gameplay.BreakdownSystem.Interface;
using StaticData.Entity;
using UI.BasePointerArrow.Interface;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.BreakdownSystem
{
    public class DestroyedPiece : MonoBehaviour, IDestroyedPiece, ITargetPointerArrow
    {
        public int Id => _id;
        public Transform Transform => transform;
        public bool IsVisited { get; set; }
        public bool IsDisconnect { get; set; }
        public List<IDestroyedPiece> ConnectedTo { get; private set; }

        [SerializeField] private int _id;

        private IEntity _entity;

        public bool IsActive => true;
        public Vector3 Position => transform.position;

        public void InitDestroyedPieces(IEntity entity, List<IDestroyedPiece> destroyedPieces,
            DestroyedPiecesId destroyedPiecesId)
        {
            _entity = entity;
            ConnectedTo = new List<IDestroyedPiece>();
            foreach (int idPiece in destroyedPiecesId.IdPieces)
                ConnectedTo.Add(destroyedPieces[idPiece]);

            if (ConnectedTo.Count == 0)
            {
                transform.parent = null;
                IsDisconnect = true;
                transform.AddComponent<Rigidbody>();
            }
        }

        public void SetEntity(IEntity entity) =>
            _entity = entity;

        public void Collision(Collision collision)
        {
            if (ConnectedTo == null) return;
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