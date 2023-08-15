using UnityEngine;

namespace StaticData.Infrastructure
{
    [CreateAssetMenu(fileName = "DataLevels", menuName = "Static data/Infrastructure/Data levels", order = 0)]
    public class StaticDataLevels : ScriptableObject
    {
        public int TotalLevels;
        public Vector3[] SpawnPositionsOnLevel;
    }
}