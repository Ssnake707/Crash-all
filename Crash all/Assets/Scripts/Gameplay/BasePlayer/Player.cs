using System.Collections;
using System.Threading.Tasks;
using Gameplay.BasePlayer.BaseWeapon;
using Gameplay.BasePlayer.Interface;
using Infrastructure.AssetManagement;
using StaticData.Player;
using StaticData.Weapon;
using UnityEngine;

namespace Gameplay.BasePlayer
{
    public class Player : PlayerMediator
    {
        public StaticDataPlayerSettings PlayerSettings => _playerSettings;
        
        [SerializeField] private StaticDataPlayerSettings _playerSettings;
        [SerializeField] private Transform _handForWeapon;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private Transform _transformPlayer;
        [SerializeField] private Animator _animator;

        private IPlayerMovement _playerMovement;
        private IPlayerAnimation _playerAnimation;
        private bool _isCanMove;
        private PlayerWeapon _playerWeapon;

        public override async Task InitPlayer(IAssetProvider assetProvider, StaticDataWeapon dataWeapon)
        {
            _playerWeapon = new PlayerWeapon(assetProvider, this, dataWeapon, _handForWeapon);
            _playerMovement = new PlayerMovement(this, _playerSettings, _transformPlayer, _rigidbody, _centerOfMass);
            _playerAnimation = new PlayerAnimation(this, _playerSettings, _animator);
            await CreateWeapon();
        }

        public override void SetRotatingSpeed(int levelRotatingSpeed, int maxLevelRotatingSpeed, bool isEffects)
        {
            _playerMovement.SetMaxAngularVelocity(levelRotatingSpeed, maxLevelRotatingSpeed);
            if (!isEffects) return;
            // Use vfx upgrade
        }

        public override void SetSizeWeapon(int levelSizeWeapon, int maxLevelSizeWeapon, bool isEffects)
        {
            if (isEffects)
            {
                _playerWeapon.AddSize(levelSizeWeapon, maxLevelSizeWeapon, 
                    _playerSettings.DefaultSizeWeapon, _playerSettings.MaxSizeWeapon, _playerSettings.DurationAnimSize);
            }
            else
            {
                _playerWeapon.SetSize(levelSizeWeapon, maxLevelSizeWeapon, 
                    _playerSettings.DefaultSizeWeapon, _playerSettings.MaxSizeWeapon);
            }
        }

        public override void PlayerMove(float speed) => 
            _playerAnimation.PlayerMove(speed);

        public override void PlayerRotating() => 
            _playerAnimation.PlayerRotating();

        public override void SetPosition(Vector3 position) => 
            StartCoroutine(WaitLateUpdateAndSetPosition(position));


        public override void PlayerStartGame() => 
            SetCanMove(true);

        public override void PlayerResetAnimation()
        {
            _playerAnimation.PlayerDance(false);
            _playerAnimation.PlayerMove(0f);
        }

        public override void PlayerWin()
        {
            SetCanMove(false);
            _playerAnimation.PlayerDance(true);
        }

        protected override async Task CreateWeapon() => 
            await _playerWeapon.CreateWeapon();

        private IEnumerator WaitLateUpdateAndSetPosition(Vector3 position)
        {
            yield return new WaitForFixedUpdate();
            _transformPlayer.position = position;
        }

        private void FixedUpdate() => 
            _playerMovement.FixedUpdate();
    }
}