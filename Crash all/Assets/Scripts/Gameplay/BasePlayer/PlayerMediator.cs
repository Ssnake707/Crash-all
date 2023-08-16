using UnityEngine;

namespace Gameplay.BasePlayer
{
    public abstract class PlayerMediator : MonoBehaviour
    {
        public abstract bool CanMove { get; }
        public abstract void PlayerMove(float speed);
        public abstract void PlayerRotating();

        public abstract void SetPosition(Vector3 position);
    }
}