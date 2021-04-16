using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Helper.Addressable
{
    public interface IAsset
    {
        event Action OnDestroyed;
    }
}
