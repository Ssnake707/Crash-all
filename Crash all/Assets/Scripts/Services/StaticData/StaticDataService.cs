using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaticData.Infrastructure;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataScenes = "Scenes";
        
        public StaticDataScenes Scenes { get; private set; }
        
        public async Task LoadAsync()
        {
            //Example load 1 static data
            /*
            await LoadAssetAsync<StaticDataTest>(Test, 
                completed => _staticDataTest = completed.Result);
            */
            // example load identical type static data with use label
            /*
             await LoadAssetsAsync<StaticDataTest>(Label, objects =>
            {
                _dictionaryTests = objects.Result.ToDictionary(x => x.Id, x => x);
            });
            */
            
            await LoadAssetAsync<StaticDataScenes>(StaticDataScenes, 
                completed => Scenes = completed.Result);
        }

        private Task<T> LoadAssetAsync<T>(string address, Action<AsyncOperationHandle<T>> onCompleted) where T : class
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            handle.Completed += complete => onCompleted?.Invoke(complete);
            return handle.Task;
        }
        
        private Task<IList<T>> LoadAssetsAsync<T>(string label, Action<AsyncOperationHandle<IList<T>>> onCompleted) where T : class
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);
            handle.Completed += complete => onCompleted?.Invoke(complete);
            return handle.Task;
        }
    }
}