using System.Collections;
using Gameplay.BasePlayer.Interface;
using StaticData.Player;
using UnityEngine;

namespace Gameplay.BasePlayer
{
    public class Player : PlayerMediator
    {
        public StaticDataPlayerSettings PlayerSettings => _playerSettings;
        
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

        private IEnumerator WaitLateUpdateAndSetPosition(Vector3 position)
        {
            yield return new WaitForFixedUpdate();
            _transformPlayer.position = position;
        }

        private void FixedUpdate() => 
            _playerMovement.FixedUpdate();
    }
}