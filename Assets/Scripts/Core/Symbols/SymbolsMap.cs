using System;
using System.Collections.Generic;
using Helper.Addressable;
using UnityEngine;

namespace Core.Symbols
{
    [CreateAssetMenu(menuName = "SlotMachine/Symbols/new symbols map")]
    public class SymbolsMap : ScriptableObject
    {
        public List<SymbolData> symbols;
        
        private Dictionary<SymbolType, SymbolData> _map;
        private Dictionary<SymbolType, SymbolData> _symbolsMap
        {
            get
            {
                if (_map != null) return _map;
                    _map = new Dictionary<SymbolType, SymbolData>();
                    if (symbols == null || symbols.Count == 0) return _map;
                    symbols.ForEach(symbol => _map.Add(symbol.type, symbol));
                    return _map;
            }
        }

        private AssetsLoader _loader;
        private AssetsLoader _assetsLoader
        {
            get
            {
                if (_loader == null) _loader = new AssetsLoader();
                return _loader;
            }
        }
        private bool _isLoaded;
        private Action _callback;
        
        /// <summary>
        /// release unnecessary references once we have finished from this map
        /// </summary>
        public void ReleaseReferences()
        {
            _isLoaded = false;
            _callback = null;
            _map = null;
            _loader = null;
        }
        
        /// <summary>
        /// use it to check if the sprite are correctly loaded
        /// </summary>
        /// <param name="inType"></param>
        /// <returns></returns>
        public bool HasSprite(SymbolType inType)
        {
            if (_symbolsMap.Count == 0 || !_symbolsMap.ContainsKey(inType)) return false;
            return _symbolsMap[inType].sprite != null;
        }

       /// <summary>
       /// return symbol data (type, reference, sprite)
       /// </summary>
       /// <param name="inType"></param>
       /// <returns></returns>
        public SymbolData GetData(SymbolType inType)
        {
            if (_symbolsMap.Count == 0) return null;
            return _symbolsMap[inType];
        }

       /// <summary>
       /// Load all symbols sprites Async
       /// </summary>
       /// <param name="callback"> triggered once the loading is completed </param>
        public void LoadAllSymbols(Action callback)
        {
            if(_isLoaded) return;
            _isLoaded = true;
            _callback = callback;
            LoadSprite(0);
        }

       private void LoadSprite(int index)
        {
            void Next()
            {
                index++;
                LoadSprite(index);
            }
            
            if (index >= symbols.Count)
            {
                //Load sprites completed !
                _callback?.Invoke();
            }
            else
            {
                _assetsLoader.LoadSprite(symbols[index].reference, 
                sprite =>
                {
                    //- on Completed
                  symbols[index].sprite = sprite;
                  Next();
                },
                (error =>
                {
                    //- on Failed
                    Debug.LogWarning( $"Loading Sprite Failed ! : {error}");
                    Next();
                }));
            }
        }
    }
}