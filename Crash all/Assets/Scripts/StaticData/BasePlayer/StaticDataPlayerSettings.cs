using UnityEngine;

namespace StaticData.BasePlayer
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Static data/Player settings", order = 0)]
    public class StaticDataPlayerSettings : ScriptableObject
    {
        [Header("Speed")] 
        public float Speed;
        public float SpeedTurn;
        public float SpeedRotating;

        [Header("Animation"), Space(15)] 
        public string AnimParameterMove;
        public string AnimParameterSpeed;
    }
}