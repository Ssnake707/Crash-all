using System.Collections.Generic;
using StaticData.Entity;

namespace Gameplay.BreakdownSystem.Interface
{
    public interface IEntity
    {
        void Construct(IEntityFactory entityFactory);
        void Construct(IEntityFactory entityFactory, StaticDataEntity staticDataEntity);

        void InitDestroyedPieces();
        void RecalculateEntity();
        void SetDestroyedPieces(List<IDestroyedPiece> destroyedPieces);
    }
}