using UnityEngine;

namespace StaticData.Entity
{
    [CreateAssetMenu(fileName = "Entity", menuName = "Static data/Entity/Entity", order = 0)]
    public class StaticDataEntity : ScriptableObject
    {
        public DestroyedPiecesId[] DestroyedPiecesIds;
    }
}