using Gameplay.BasePlayer.Interface;
using StaticData.Player;
using UnityEngine;

namespace Gameplay.BasePlayer
{
    public class Player : PlayerMediator
    {
        public StaticDataPlayerSettings PlayerSettings => _playerSettings;
        public override bool CanMove => _isCanMove;
        
        [SerializeField] private StaticDataPlayerSettings _playerSettings;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private Transform _transformPlayer;
        [SerializeField] private Animator _animator;
        
        private IPlayerMovement _playerMovement;
        private IPlayerAnimation _playerAnimation;
        private bool _isCanMove;

        private void Awake()
        {
            _playerMovement = new PlayerMovement(this, _playerSettings, _transformPlayer, _rigidbody, _centerOfMass);
            _playerAnimation = new PlayerAnimation(this, _playerSettings, _animator);
            _isCanMove = true;
        }

        public override void PlayerMove(float speed) => 
            _playerAnimation.PlayerMove(speed);

        public override void PlayerRotating() => 
            _playerAnimation.PlayerRotating();

        public override void SetPosition(Vector3 position) => 
            transform.position = position;

        private void FixedUpdate() => 
            _playerMovement.FixedUpdate();
    }
}