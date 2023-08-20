using System;

namespace Data
{
    [Serializable]
    public class DataPlayers
    {
        public int Coins;

        public event Action OnChangeCoins;

        public void AddCoins(int amount)
        {
            Coins += amount;
            OnChangeCoins?.Invoke();
        }
    }
}