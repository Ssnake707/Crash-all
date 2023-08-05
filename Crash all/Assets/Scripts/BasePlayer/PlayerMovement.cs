using BasePlayer.Interface;
using StaticData.BasePlayer;
using UnityEngine;

namespace BasePlayer
{
    public class PlayerMovement : IPlayerMovement
    {
        private readonly PlayerMediator _playerMediator;
        private readonly StaticDataPlayerSettings _playerSettings;
        private readonly Transform _transformPlayer;
        private readonly Rigidbody _rigidbody;

        public PlayerMovement(PlayerMediator playerMediator, StaticDataPlayerSettings playerSettings,
            Transform transformPlayer, Rigidbody rigidbody)
        {
            _playerMediator = playerMediator;
            _playerSettings = playerSettings;
            _transformPlayer = transformPlayer;
            _rigidbody = rigidbody;
        }

        public void Tick()
        {
            if (!_playerMediator.CanMove) return;
            Move(SimpleInput.GetAxis("Horizontal"), SimpleInput.GetAxis("Vertical"));
        }

        private void Move(float horizontal, float vertical)
        {
            MoveForward(horizontal, vertical);
            Rotate(horizontal, vertical);
        }

        private void MoveForward(float horizontal, float vertical)
        {
            horizontal *= _playerSettings.Speed;
            vertical *= _playerSettings.Speed;
            _rigidbody.velocity = Vector3.forward * vertical + Vector3.right * horizontal +
                                  Vector3.up * _rigidbody.velocity.y;
        }

        private void Rotate(float horizontal, float vertical)
        {
            if (horizontal == 0f && vertical == 0f) return;
            _transformPlayer.rotation = Quaternion.Slerp(_transformPlayer.rotation,
                Quaternion.Euler(Vector3.up *
                                 (Mathf.Rad2Deg * Mathf.Atan2(horizontal, vertical))),
                _playerSettings.SpeedTurn * Time.deltaTime);
        }
    }
}