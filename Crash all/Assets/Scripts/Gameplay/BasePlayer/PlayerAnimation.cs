using Gameplay.BasePlayer.Interface;
using StaticData.Player;
using UnityEngine;

namespace Gameplay.BasePlayer
{
    public class PlayerAnimation : IPlayerAnimation
    {
        private readonly PlayerMediator _playerMediator;
        private readonly StaticDataPlayerSettings _playerSettings;
        private readonly Animator _animator;
        private readonly int _hashAnimParameterMove;
        private readonly int _hashAnimParameterSpeed;
        private readonly int _hashAnimParameterIsDancing;
        
        private bool _isMoving;

        public PlayerAnimation(PlayerMediator playerMediator, StaticDataPlayerSettings playerSettings,
            Animator animator)
        {
            _playerSettings = playerSettings;
            _playerMediator = playerMediator;
            _animator = animator;
            _hashAnimParameterMove = Animator.StringToHash(_playerSettings.AnimParameterMove);
            _hashAnimParameterSpeed = Animator.StringToHash(_playerSettings.AnimParameterSpeed);
            _hashAnimParameterIsDancing = Animator.StringToHash(_playerSettings.AnimParameterIsDancing);
        }

        public void PlayerMove(float speed)
        {
            if (!_isMoving)
            {
                _animator.SetBool(_hashAnimParameterMove, true);
                _isMoving = true;
            } 
            _animator.SetFloat(_hashAnimParameterSpeed, speed);
        }

        public void PlayerRotating()
        {
            if (!_isMoving) return;
            
            _animator.SetBool(_hashAnimParameterMove, false);
            _isMoving = false;
        }

        public void PlayerDance(bool isDancing) => 
            _animator.SetBool(_hashAnimParameterIsDancing, isDancing);
    }
}