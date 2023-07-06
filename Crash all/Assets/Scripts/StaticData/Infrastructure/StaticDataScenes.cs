using UnityEngine;

namespace StaticData.Infrastructure
{
    [CreateAssetMenu(fileName = "Scenes", menuName = "Static data/Infrastructure/Scenes", order = 0)]
    public class StaticDataScenes : ScriptableObject
    {
        public string InitialScene;
        public string MainScene;
    }
}