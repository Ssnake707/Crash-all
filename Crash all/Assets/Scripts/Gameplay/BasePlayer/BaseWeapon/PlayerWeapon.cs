using System.Collections;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.AssetManagement;
using StaticData.Weapon;
using UnityEngine;

namespace Gameplay.BasePlayer.BaseWeapon
{
    public class PlayerWeapon
    {
        private IAssetProvider _assetProvider;
        private StaticDataWeapon _dataWeapon;
        private Transform _handForWeapon;
        private Weapon _weapon;
        private Coroutine _coroutineAnimationSizeWeapon = null;
        private ICoroutineRunner _coroutineRunner;
        
        public PlayerWeapon(IAssetProvider assetProvider, ICoroutineRunner coroutineRunner, 
            StaticDataWeapon dataWeapon, Transform handForWeapon)
        {
            _assetProvider = assetProvider;
            _coroutineRunner = coroutineRunner;
            _dataWeapon = dataWeapon;
            _handForWeapon = handForWeapon;
        }

        public async Task CreateWeapon()
        {
            GameObject weaponPrefab = await _assetProvider.Load<GameObject>(_dataWeapon.AssetWeapon);
            GameObject weapon = Object.Instantiate(weaponPrefab, _handForWeapon);
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

        public void AddSize(int levelSizeWeapon, int maxLevelSizeWeapon,
            float defaultSizeWeapon, float maxSizeWeapon, float durationAnim)
        {
            if (_coroutineAnimationSizeWeapon != null) 
                _coroutineRunner.StopCoroutine(_coroutineAnimationSizeWeapon);

            _coroutineAnimationSizeWeapon = _coroutineRunner.StartCoroutine(AnimationSizeWeapon(
                levelSizeWeapon, maxLevelSizeWeapon, defaultSizeWeapon, maxSizeWeapon, durationAnim));
        }

        private IEnumerator AnimationSizeWeapon(int levelSizeWeapon, int maxLevelSizeWeapon, float defaultSizeWeapon,
            float maxSizeWeapon, float durationAnim)
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
            
            while (pastTime < durationAnim)
            {
                pastTime += Time.deltaTime;
                float t = pastTime / durationAnim;
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