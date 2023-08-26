using UnityEngine;

namespace StaticData.Player
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Static data/Player settings", order = 0)]
    public class StaticDataPlayerSettings : ScriptableObject
    {
        [Header("Movement settings")] 
        public float Speed;
        public float SpeedTurn;
        public float ForceRotating;
        
        [Space(10)]
        [Header("Upgrade")]
        public float DefaultMaxAngularVelocity;
        public float MaxAngularVelocity;
        public float DefaultSizeWeapon;
        public float MaxSizeWeapon;

        [Space(10)]
        [Header("Animation")] 
        public float DurationAnimSize;
        public string AnimParameterMove;
        public string AnimParameterSpeed;
        public string AnimParameterIsDancing;
    }
}