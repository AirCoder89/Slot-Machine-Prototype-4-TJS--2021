using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Helper.Addressable
{
    public class AssetsSpawner
    {
        /// <summary>
        /// Load & Instantiate Asset. this will be helpful if we deal with prefabs (ex: slots)
        /// </summary>
        /// <param name="assetReference"> target asset reference </param>
        public void Instantiate<T>(AssetReference assetReference, Action<T> onComplete, Action<string> onFailed = null)  where T : MonoBehaviour, IAsset
        {
            if (assetReference == null) throw new NullReferenceException($"AssetReference must be not null!");
            var operation = Addressables.InstantiateAsync(assetReference);
            
            operation.Completed += (response) =>
            {
                switch (operation.Status)
                {
                    case AsyncOperationStatus.Failed:
                        onFailed?.Invoke(operation.PercentComplete.ToString());
                        break;
                    
                    case AsyncOperationStatus.Succeeded:
                        var asset = response.Result.GetComponent<IAsset>();
                        asset.OnDestroyed += () => { Addressables.ReleaseInstance(response.Result); }; //release
                        onComplete?.Invoke(response.Result.GetComponent<T>());
                        break;
                    
                    default:
                        Debug.LogError("No Result > " + response.Result);
                        break;
                }
            };
        }
    }
}