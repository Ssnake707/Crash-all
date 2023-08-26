using UnityEngine;
using UnityEngine.AddressableAssets;

namespace StaticData.Weapon
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Static data/Weapons/New weapon", order = 0)]
    public class StaticDataWeapon : ScriptableObject
    {
        public int Id;
        public AssetReference AssetWeapon;
        public ColliderData MinColliderData;
        public ColliderData MaxColliderData;
    }
}