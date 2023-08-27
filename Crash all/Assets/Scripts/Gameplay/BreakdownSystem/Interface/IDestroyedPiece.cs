using System.Collections.Generic;
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
        void Collision(Collision collision);
        void SetEntity(IEntity entity);
    }
}