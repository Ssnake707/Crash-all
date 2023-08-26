using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StaticData.Infrastructure;
using StaticData.Progression;
using StaticData.Weapon;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataScenes = "Scenes";
        private const string StaticDataLevels = "DataLevels";
        private const string StaticDataPriceRotatingSpeed = "Price Rotating speed";
        private const string StaticDataPriceSizeWeapon = "Price Size weapon";
        private const string StaticDataWeapons = "Weapon";

        public StaticDataScenes Scenes { get; private set; }
        public StaticDataLevels DataLevels { get; private set; }
        public BaseProgression DataPriceRotatingSpeed { get; private set; }
        public BaseProgression DataPriceSizeWeapon { get; private set; }
        public Dictionary<int, StaticDataWeapon> DataWeapons { get; private set; }

        public async Task LoadAsync()
        {
            //Example load 1 static data
            /*
            await LoadAssetAsync<StaticDataTest>(Test,
                completed => _staticDataTest = completed.Result);
            */
            // example load identical type static data with use label
            /*
            await LoadAssetsAsync<StaticDataWeapon>(label,
                completed => _dataWeapons = completed.Result.ToDictionary(
                    x => x.Id, x => x)); 
            */
            
            await LoadAssetsAsync<StaticDataWeapon>(StaticDataWeapons,
                completed => DataWeapons = completed.Result.ToDictionary(
                    x => x.Id, x => x));

            await LoadAssetAsync<StaticDataScenes>(StaticDataScenes,
                completed => Scenes = completed.Result);
            
            await LoadAssetAsync<StaticDataLevels>(StaticDataLevels,
                completed => DataLevels = completed.Result);
            
            await LoadAssetAsync<BaseProgression>(StaticDataPriceRotatingSpeed,
                completed => DataPriceRotatingSpeed = completed.Result);
            
            await LoadAssetAsync<BaseProgression>(StaticDataPriceSizeWeapon,
                completed => DataPriceSizeWeapon = completed.Result);
        }

        private Task<T> LoadAssetAsync<T>(string address, Action<AsyncOperationHandle<T>> onCompleted) where T : class
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            handle.Completed += complete => onCompleted?.Invoke(complete);
            return handle.Task;
        }

        private Task<IList<T>> LoadAssetsAsync<T>(string label, Action<AsyncOperationHandle<IList<T>>> onCompleted)
            where T : class
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);
            handle.Completed += complete => onCompleted?.Invoke(complete);
            return handle.Task;
        }
    }
}