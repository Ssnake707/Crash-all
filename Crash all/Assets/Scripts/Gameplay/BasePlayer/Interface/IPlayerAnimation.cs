namespace Gameplay.BasePlayer.Interface
{
    public interface IPlayerAnimation
    {
        public void PlayerMove(float speed);
        public void PlayerRotating();
        void PlayerDance(bool isDancing);
    }
}