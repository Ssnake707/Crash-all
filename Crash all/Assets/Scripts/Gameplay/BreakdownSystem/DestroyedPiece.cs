using System;
using System.Collections.Generic;
using Gameplay.BreakdownSystem.Interface;
using Gameplay.BreakdownSystem.PoolParticleSystem;
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
        public GameObject GameObject => this.gameObject;
        public bool IsVisited { get; set; }
        public bool IsDisconnect { get; set; }
        public List<IDestroyedPiece> ConnectedTo { get; private set; } = new List<IDestroyedPiece>();

        [SerializeField] private int _id;

        private IEntity _entity;
        private PoolParticleSystemHit _poolParticleSystemHit;

        public bool IsActive => ConnectedTo.Count == 0;

        public Vector3 Position => transform.position;

        public void InitDestroyedPieces(IEntity entity, List<IDestroyedPiece> destroyedPieces,
            DestroyedPiecesId destroyedPiecesId)
        {
            SetEntity(entity);
            foreach (int idPiece in destroyedPiecesId.IdPieces)
                ConnectedTo.Add(destroyedPieces[idPiece]);

            if (ConnectedTo.Count == 0)
            {
                transform.parent = null;
                IsDisconnect = true;
                transform.AddComponent<Rigidbody>();
            }
        }

        public void SetPoolParticleSystemHit(PoolParticleSystemHit poolParticleSystemHit) => 
            _poolParticleSystemHit = poolParticleSystemHit;

        public void SetEntity(IEntity entity) =>
            _entity = entity;

        public void DestroyPiece()
        {
            _entity?.RemoveDestroyedPiece(this);
            Destroy(this.gameObject);
        }

        public void Collision(Collision collision, Vector3 velocity)
        {
            if (ConnectedTo == null) return;
            if (ConnectedTo.Count == 0) return;
            DisconnectPiece();
            _entity?.RecalculateEntity();
            Rigidbody rigidBody = transform.AddComponent<Rigidbody>();
            rigidBody.AddForce(velocity, ForceMode.VelocityChange);
            HitEffectPlay(collision);
        }

        private void OnCollisionEnter(Collision collision) => 
            HitEffectPlay(collision);

        private void HitEffectPlay(Collision collision)
        {
            if (collision.impulse.magnitude < 1f) return;
            if (collision.gameObject.TryGetComponent<IDestroyedPiece>(out IDestroyedPiece piece)) return;
            if (collision.gameObject.TryGetComponent<IEntity>(out IEntity entity)) return;
            if (_poolParticleSystemHit == null) return;
            ParticleSystem effect = _poolParticleSystemHit.Pool.Get();
            effect.transform.rotation = Quaternion.Euler(collision.GetContact(0).normal);
            effect.transform.position = collision.GetContact(0).point;
            effect.Play();
        }


        private void DisconnectPiece()
        {
            transform.parent = null;
            ConnectedTo.Clear();
            IsDisconnect = true;
            _entity?.RemoveDestroyedPiece(this);
            _entity = null;
        }
    }
}