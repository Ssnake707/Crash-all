using System.Collections.Generic;
using Gameplay.BreakdownSystem.PoolParticleSystem;
using StaticData.Entity;
using UI.BasePointerArrow.Interface;
using UnityEngine;

namespace Gameplay.BreakdownSystem.Interface
{
    public interface IDestroyedPiece
    {
        int Id { get; }
        Transform Transform { get; }
        bool IsVisited { get; set; }
        public List<IDestroyedPiece> ConnectedTo { get; }
        bool IsDisconnect { get; }
        void InitDestroyedPieces(IEntity entity, List<IDestroyedPiece> destroyedPieces,
            DestroyedPiecesId destroyedPiecesId);
        void Collision(Collision collision, Vector3 velocity);
        void SetEntity(IEntity entity);
        void DestroyPiece();
        void SetPoolParticleSystemHit(PoolParticleSystemHit poolParticleSystemHit);
    }
}