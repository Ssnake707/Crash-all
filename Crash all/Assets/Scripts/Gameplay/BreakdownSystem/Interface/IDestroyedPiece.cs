using System.Collections.Generic;
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
        void Collision(Collision collision);
        void SetEntity(IEntity entity);
    }
}