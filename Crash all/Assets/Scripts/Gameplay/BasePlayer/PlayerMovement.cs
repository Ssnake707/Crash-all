using Gameplay.BasePlayer.Interface;
using StaticData.BasePlayer;
using UnityEngine;

namespace Gameplay.BasePlayer
{
    public class PlayerMovement : IPlayerMovement
    {
        private readonly PlayerMediator _playerMediator;
        private readonly StaticDataPlayerSettings _playerSettings;
        private readonly Transform _transformPlayer;
        private readonly Rigidbody _rigidbody;

        public PlayerMovement(PlayerMediator playerMediator, StaticDataPlayerSettings playerSettings,
            Transform transformPlayer, Rigidbody rigidbody, Transform centerOfMass)
        {
            _playerMediator = playerMediator;
            _playerSettings = playerSettings;
            _transformPlayer = transformPlayer;
            _rigidbody = rigidbody;
            _rigidbody.centerOfMass = centerOfMass.position;
            _rigidbody.maxAngularVelocity = _playerSettings.DefaultMaxAngularVelocity;
        }

        public void FixedUpdate()
        {
            if (!_playerMediator.CanMove) return;
            float horizontal = SimpleInput.GetAxis("Horizontal");
            float vertical = SimpleInput.GetAxis("Vertical");
            float speed = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            if (speed == 0f)
            {
                _playerMediator.PlayerRotating();
                Rotating();
            }
            else
            {
                _playerMediator.PlayerMove(speed);
                Move(horizontal, vertical);
            }
        }

        private void Move(float horizontal, float vertical)
        {
            _rigidbody.angularVelocity = Vector3.zero;
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
            _transformPlayer.rotation = Quaternion.Slerp(_transformPlayer.rotation,
                Quaternion.Euler(Vector3.up *
                                 (Mathf.Rad2Deg * Mathf.Atan2(horizontal, vertical))),
                _playerSettings.SpeedTurn * Time.fixedDeltaTime);
        }

        private void Rotating()
        {
            _rigidbody.AddTorque(_transformPlayer.up * (_playerSettings.ForceRotating * Time.fixedDeltaTime), ForceMode.Acceleration);
            
            // Old rotating
            /* Quaternion q = Quaternion.Euler(new Vector3(0f, _playerSettings.SpeedRotating, 0f) * Time.fixedDeltaTime);
             _rigidbody.MoveRotation(_rigidbody.rotation * q);*/
        }
    }
}