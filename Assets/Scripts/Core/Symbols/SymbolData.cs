using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Symbols
{
    [Serializable]
    public class SymbolData
    {
        public SymbolType type;
        public AssetReferenceSprite reference;
        public Sprite sprite;
    }
}