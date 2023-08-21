using UnityEngine;

namespace Gameplay.BasePlayer
{
    public abstract class PlayerMediator : MonoBehaviour
    {
        public virtual bool CanMove { get; private set; }
        public abstract void PlayerMove(float speed);
        public abstract void PlayerRotating();
        public abstract void SetPosition(Vector3 position);
        public abstract void PlayerWin();
        public abstract void PlayerStartGame();

        protected virtual void SetCanMove(bool canMove) => 
            CanMove = canMove;
    }
}