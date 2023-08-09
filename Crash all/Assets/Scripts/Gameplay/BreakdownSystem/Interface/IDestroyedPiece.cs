using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.BreakdownSystem.Interface
{
    public interface IDestroyedPiece
    {
        bool IsVisited { get; set; }
        public List<IDestroyedPiece> ConnectedTo { get; }
        void Construct(IEntity entity);
        void MakeStatic();
        void Collision(Collision collision);
    }
}