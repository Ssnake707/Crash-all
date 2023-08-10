using System.Collections.Generic;
using StaticData.Entity;

namespace Gameplay.BreakdownSystem.Interface
{
    public interface IEntity
    {
        void Construct(IEntityFactory entityFactory, StaticDataEntitySettings entitySettings);

        void InitDestroyedPieces();
        void RecalculateEntity();
        void SetDestroyedPieces(List<IDestroyedPiece> destroyedPieces);
    }
}