using UnityEngine;

namespace StaticData.Player
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Static data/Player settings", order = 0)]
    public class StaticDataPlayerSettings : ScriptableObject
    {
        [Header("Movement settings")] 
        public float Speed;
        public float SpeedTurn;
        public float DefaultMaxAngularVelocity;
        public float MaxAngularVelocity;
        public float DefaultSizeWeapon;
        public float MaxSizeWeapon;
        public float ForceRotating;

        [Header("Animation"), Space(15)] 
        public string AnimParameterMove;
        public string AnimParameterSpeed;
        public string AnimParameterIsDancing;
    }
}