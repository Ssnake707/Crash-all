using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.AssetManagement;
using StaticData.Weapon;
using UI.MainMenu;
using UI.MainMenu.Interface;
using UnityEngine;

namespace Gameplay.BasePlayer
{
    public abstract class PlayerMediator : MonoBehaviour, IMainMenuModel, ICoroutineRunner
    {
        protected MainMenuAdapter MainMenuAdapter;

        public void SetMainMenuAdapter(MainMenuAdapter adapter) =>
            MainMenuAdapter = adapter;

        public abstract Task InitPlayer(IAssetProvider assetProvider, StaticDataWeapon dataWeapon);
        public abstract void SetRotatingSpeed(int levelRotatingSpeed, int maxLevelRotatingSpeed, bool isEffects);
        public abstract void SetSizeWeapon(int levelSizeWeapon, int maxLevelSizeWeapon, bool isEffects);
        public virtual bool CanMove { get; private set; }
        public abstract void PlayerMove(float speed);
        public abstract void PlayerRotating();
        public abstract void SetPosition(Vector3 position);
        public abstract void PlayerWin();
        public abstract void PlayerStartGame();
        public abstract void PlayerResetAnimation();
        protected abstract Task CreateWeapon();

        protected virtual void SetCanMove(bool canMove) =>
            CanMove = canMove;
    }
}