using System.Collections.Generic;

namespace Gameplay.BreakdownSystem.Interface
{
    public interface IDestroyedPiece
    {
        bool IsVisited { get; set; }
        public List<IDestroyedPiece> ConnectedTo { get; }
        void Construct();
        void MakeStatic();
    }
}