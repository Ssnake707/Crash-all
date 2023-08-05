using System;
using BasePlayer.Interface;
using StaticData.BasePlayer;
using UnityEngine;

namespace BasePlayer
{
    public class Player : PlayerMediator
    {
        public StaticDataPlayerSettings PlayerSettings => _playerSettings;
        public override bool CanMove => _isCanMove;

        [SerializeField] private StaticDataPlayerSettings _playerSettings;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _transformPlayer;

        private IPlayerMovement _playerMovement;
        private IPlayerAnimation _playerAnimation;
        private bool _isCanMove;

        private void Awake()
        {
            _playerMovement = new PlayerMovement(this, _playerSettings, _transformPlayer, _rigidbody);
            _playerAnimation = new PlayerAnimation();
            _isCanMove = true;
        }

        private void Update()
        {
            _playerMovement.Tick();
        }
    }
}