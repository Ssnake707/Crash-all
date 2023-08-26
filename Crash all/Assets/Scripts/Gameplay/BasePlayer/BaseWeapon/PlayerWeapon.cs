using System.Collections;
using System.Threading.Tasks;
using Infrastructure.AssetManagement;
using StaticData.Weapon;
using UnityEngine;
using Zenject;

namespace Gameplay.BasePlayer.BaseWeapon
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private StaticDataWeapon _dataWeapon;
        [SerializeField] private Transform _handForWeapon;
        [SerializeField] private float _durationAnimSize;
        private IAssetProvider _assetProvider;
        private Weapon _weapon;
        private Coroutine _coroutineAnimationSizeWeapon = null;

        [Inject]
        public void Construct(IAssetProvider assetProvider) => 
            _assetProvider = assetProvider;

        public async Task CreateWeapon()
        {
            GameObject weaponPrefab = await _assetProvider.Load<GameObject>(_dataWeapon.AssetWeapon);
            GameObject weapon = Instantiate(weaponPrefab, _handForWeapon);
            _weapon = weapon.GetComponent<Weapon>();
        }

        public void SetSize(int levelSizeWeapon, int maxLevelSizeWeapon, float defaultSizeWeapon, float maxSizeWeapon)
        {
            float interpolate = (float)levelSizeWeapon / maxLevelSizeWeapon;
            _weapon.SkinnedMeshRenderer.SetBlendShapeWeight(0, 
                Mathf.Lerp(defaultSizeWeapon, maxSizeWeapon, interpolate));
            _weapon.Collider.center = Vector3.Lerp(_dataWeapon.MinColliderData.BoxColliderCenter,
                _dataWeapon.MaxColliderData.BoxColliderCenter, interpolate);
            _weapon.Collider.size = Vector3.Lerp(_dataWeapon.MinColliderData.BoxColliderSize,
                _dataWeapon.MaxColliderData.BoxColliderSize, interpolate);
        }

        public void AddSize(int levelSizeWeapon, int maxLevelSizeWeapon, float defaultSizeWeapon, float maxSizeWeapon)
        {
            if (_coroutineAnimationSizeWeapon != null) 
                StopCoroutine(_coroutineAnimationSizeWeapon);

            _coroutineAnimationSizeWeapon = StartCoroutine(AnimationSizeWeapon(
                levelSizeWeapon, maxLevelSizeWeapon, defaultSizeWeapon, maxSizeWeapon));
        }

        private IEnumerator AnimationSizeWeapon(int levelSizeWeapon, int maxLevelSizeWeapon, float defaultSizeWeapon,
            float maxSizeWeapon)
        {
            float pastTime = 0f;
            float interpolate = (float)levelSizeWeapon / maxLevelSizeWeapon;
            float currentSize = _weapon.SkinnedMeshRenderer.GetBlendShapeWeight(0);
            float sizeWeapon = Mathf.Lerp(defaultSizeWeapon, maxSizeWeapon, interpolate);

            Vector3 currentColliderCenter = _weapon.Collider.center;
            Vector3 currentColliderSize = _weapon.Collider.size;

            Vector3 colliderCenter = Vector3.Lerp(_dataWeapon.MinColliderData.BoxColliderCenter,
                _dataWeapon.MaxColliderData.BoxColliderCenter, interpolate);
            Vector3 colliderSize = Vector3.Lerp(_dataWeapon.MinColliderData.BoxColliderSize,
                _dataWeapon.MaxColliderData.BoxColliderSize, interpolate);
            
            while (pastTime < _durationAnimSize)
            {
                pastTime += Time.deltaTime;
                float t = pastTime / _durationAnimSize;
                _weapon.SkinnedMeshRenderer.SetBlendShapeWeight(0, 
                    Mathf.Lerp(currentSize, sizeWeapon, t));
                _weapon.Collider.center = Vector3.Lerp(currentColliderCenter, colliderCenter, t);
                _weapon.Collider.size = Vector3.Lerp(currentColliderSize, colliderSize, t);
                yield return null;
            }
            
            _weapon.SkinnedMeshRenderer.SetBlendShapeWeight(0, sizeWeapon);
            _weapon.Collider.center = colliderCenter;
            _weapon.Collider.size = colliderSize;
        }
    }
}