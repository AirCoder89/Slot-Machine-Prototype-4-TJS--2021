using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Helper.Addressable
{
    public class AssetsLoader
    {
        /// <summary>
        /// Load Sprites Async by Unity Addressable System
        /// </summary>
        /// <param name="spriteReference"> target sprite reference </param>
        /// <param name="onComplete"> on completed callback </param>
        /// <param name="onFailed"> on error callback </param>
        public void LoadSprite(AssetReferenceSprite spriteReference,Action<Sprite> onComplete, Action<string> onFailed = null)
        {
            spriteReference.LoadAssetAsync()
                .Completed += response =>
            { 
                if (response.Status == AsyncOperationStatus.Failed) onFailed?.Invoke(response.PercentComplete.ToString());
                else if (response.Status == AsyncOperationStatus.Succeeded) onComplete?.Invoke(response.Result);
                else Debug.LogError("None Result > " + response.Result);
            };
        }
        
        /// <summary>
        /// Load Any kind of asset including sprites. but in order to keep things organised and more efficient we separate this methods
        /// so we can use it to load other assets such as : sounds, visual effects etc ..
        /// </summary>
        /// <param name="assetReference"> target asset reference </param>
        /// <param name="onComplete"> on completed callback </param>
        /// <param name="onFailed"> on error callback </param>
        /// <typeparam name="T"> target asset type </typeparam>
        public void Load<T>(AssetReference assetReference, Action<T, AsyncOperationHandle<T>> onComplete, Action<string> onFailed = null)
        {
            if (assetReference == null) throw new NullReferenceException($"AssetReference must be not null!");
            var operation = assetReference.LoadAssetAsync<T>();

            Debug.Log($"Load Prefab : {operation.IsValid()}");
                
            operation.Completed += (response) =>
            {
                if (operation.Status == AsyncOperationStatus.Failed) onFailed?.Invoke(operation.PercentComplete.ToString());
                else if (operation.Status == AsyncOperationStatus.Succeeded) onComplete?.Invoke(response.Result, operation);
                else Debug.LogError("None Result > " + response.Result);
            };
        }
    }
}