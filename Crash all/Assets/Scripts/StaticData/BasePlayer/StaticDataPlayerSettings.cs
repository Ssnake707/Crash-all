using UnityEngine;

namespace StaticData.BasePlayer
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Static data/Player settings", order = 0)]
    public class StaticDataPlayerSettings : ScriptableObject
    {
        public float Speed;
        public float SpeedTurn;
        public float SpeedRotating;
    }
}