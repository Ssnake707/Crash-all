using System;
using UnityEngine;

namespace StaticData.Entity
{
    [CreateAssetMenu(fileName = "Entity", menuName = "Static data/Entity/Entity", order = 0)]
    public class StaticDataEntity : ScriptableObject
    {
        public int Id;
        public DestroyedPiecesId[] DestroyedPiecesIds;
    }

    [Serializable]
    public class DestroyedPiecesId
    {
        public int Id;
        public int[] IdPieces;
    }
}