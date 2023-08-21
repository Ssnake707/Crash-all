using UnityEngine;

namespace StaticData.Infrastructure
{
    [CreateAssetMenu(fileName = "DataLevels", menuName = "Static data/Infrastructure/Data levels", order = 0)]
    public class StaticDataLevels : ScriptableObject
    {
#if UNITY_EDITOR
        public int AlwaysLoadLevel = -1;
#endif
        public int TotalLevels;
        public DataLevels[] DataLevels;
    }
}