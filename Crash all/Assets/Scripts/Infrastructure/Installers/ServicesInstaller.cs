using Infrastructure.AssetManagement;
using Infrastructure.SceneLoaders;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _prefabLoadingCurtain;
        [SerializeField] private GameObject _prefabCoroutineRunner;

        public override void InstallBindings()
        {
            BindLoadingCurtain();
            BindCoroutineRunner();
            BindSceneLoader();
            BindPersistentProgressService();
            BindSaveLoadService();
            BindStaticDataService();
            BindAssetProvider();
        }

        private void BindLoadingCurtain()
        {
            LoadingCurtain loadingCurtain = Instantiate(_prefabLoadingCurtain).GetComponent<LoadingCurtain>();
            Container
                .Bind<LoadingCurtain>()
                .FromInstance(loadingCurtain)
                .AsSingle();
        }

        private void BindCoroutineRunner()
        {
            ICoroutineRunner coroutineRunner = Instantiate(_prefabCoroutineRunner).GetComponent<ICoroutineRunner>();
            Container
                .Bind<ICoroutineRunner>()
                .FromInstance(coroutineRunner)
                .AsSingle();
        }

        private void BindSceneLoader() =>
            Container
                .Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle();

        private void BindPersistentProgressService() =>
            Container
                .Bind<IPersistentProgressService>()
                .To<PersistentProgressService>()
                .AsSingle();

        private void BindSaveLoadService() =>
            Container
                .Bind<ISaveLoadService>()
                .To<SaveLoadService>()
                .AsSingle();

        private void BindStaticDataService() =>
            Container
                .Bind<IStaticDataService>()
                .To<StaticDataService>()
                .AsSingle();

        private void BindAssetProvider() =>
            Container
                .Bind<IAssetProvider>()
                .To<AssetProvider>()
                .AsSingle();
    }
}