using System;
using Core.Symbols;
using Helper.Addressable;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Vector2Int = Helper.Vector2Int;

namespace Core.Machine
{
    public class Slot : BaseMonoBehaviour, IAsset
    {
        [SerializeField] private Image symbolHolder;
        
        public SymbolType CurrentSymbol { get; private set; }
        private RectTransform _rt;
        public RectTransform rectTransform
        {
            get
            {
                if (_rt == null) _rt = GetComponent<RectTransform>();
                return _rt;
            }
        }
        
        public event Action OnDestroyed;

        public Vector2Int Location => _location;

        private SlotMachine _parentMachine;
        private Vector2Int _location;
        
        protected override void ReleaseReferences()
        {
            _rt = null;
            _parentMachine = null;
            symbolHolder = null;
            OnDestroyed?.Invoke();
            OnDestroyed = null;
        }

        public void Initialize(SlotMachine inMachine, Vector2Int inLocation)
        {
            _parentMachine = inMachine;
            _location = inLocation;
            RandomSymbol();
            UpdatePosition();
            name = $"Slot [{_location.ToString()}]";
        }

        public void RandomSymbol()
        {
            CurrentSymbol = (SymbolType) UnityEngine.Random.Range(0, MachineController.SymbolsMap.symbols.Count);
            if (MachineController.SymbolsMap.HasSprite(CurrentSymbol))
                symbolHolder.sprite = MachineController.SymbolsMap.GetData(CurrentSymbol).sprite;
        }

        public void SetType(SymbolType inType)
        {
            CurrentSymbol = inType;
            if (MachineController.SymbolsMap.HasSprite(CurrentSymbol))
                symbolHolder.sprite = MachineController.SymbolsMap.GetData(CurrentSymbol).sprite;
        }
        
        public void RemoveSlot()
        {
            Destroy(gameObject);
        }
        
        public void UpdatePosition()
            => rectTransform.anchoredPosition = LocationToPosition(Location, rectTransform.sizeDelta);
        
        private Vector2 LocationToPosition(Vector2Int inLocation, Vector2 inSlotSize)
        {
            var spacing = new Vector2(_parentMachine.Config.spacing.x * inLocation.y, -_parentMachine.Config.spacing.y * inLocation.x);
            var slotPosition = new Vector2(inSlotSize.x * inLocation.y, -inSlotSize.y * inLocation.x);
            return _parentMachine.Config.padding + slotPosition + spacing;
        }
    }
}