using System.Collections.Generic;
using StaticData.Entity;

namespace Gameplay.BreakdownSystem.Interface
{
    public interface IEntity
    {
        void Construct(IEntityFactory entityFactory, StaticDataEntity staticDataEntity);
        void RecalculateEntity();
        void SetDestroyedPieces(List<IDestroyedPiece> destroyedPieces);
        List<IDestroyedPiece> GetDestroyedPieces();
        void RemoveDestroyedPiece(DestroyedPiece destroyedPiece);
    }
}