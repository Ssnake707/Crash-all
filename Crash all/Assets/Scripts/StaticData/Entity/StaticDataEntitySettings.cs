using UnityEngine;

namespace StaticData.Entity
{
    [CreateAssetMenu(fileName = "EntitySettings", menuName = "Static data/Entity/Entity settings", order = 0)]
    public class StaticDataEntitySettings : ScriptableObject
    {
        public float MinImpulseForDestroy;
    }
}