using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.AssetManagement
{
    public interface IAssetProvider
    {
        Task<GameObject> Instantiate(string address, Vector3 at);
        Task<GameObject> Instantiate(string address);
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
        void CleanUp();
        void InitializeAddressables();
    }
}