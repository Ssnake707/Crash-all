namespace Gameplay.BasePlayer.Interface
{
    public interface IPlayerMovement
    {
        public void FixedUpdate();
        void SetMaxAngularVelocity(int currentLevel, int maxLevel);
    }
}