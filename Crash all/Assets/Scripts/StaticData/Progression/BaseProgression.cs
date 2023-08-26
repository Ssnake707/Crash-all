using UnityEngine;

namespace StaticData.Progression
{
    public abstract class BaseProgression : ScriptableObject
    {
        public int MaxLevel; // если -1, то максимальный уровень не ограничен
        public abstract float GetValue(int level);
    }
}