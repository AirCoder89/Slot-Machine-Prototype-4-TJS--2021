using System;
using System.Collections.Generic;
using Components;
using Helper.Addressable;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Vector2Int = Helper.Vector2Int;

namespace Core.Machine
{
    public class SlotMachine : BaseMonoBehaviour
    {
        [Header("Internal References")]
        [SerializeField] private MachineConfig config;
        [SerializeField] private AssetReference slotReference;
        [SerializeField] private RectTransform slotsHolder;
        [SerializeField] private SpinButton spinBtn;
        [SerializeField] private ViewFades rowsView;
        
        //- private variables
        private AssetsSpawner _slotsSpawner;
        private List<Slot> _slots;
        private int _size;
        private Vector2Int _dimension;
        private bool _isAutoSpin;
        private bool _canSpin;
        
        //- properties
        public Slot[,] Matrix { get; private set; }
        protected override void ReleaseReferences()
        {
            rowsView = null;
            spinBtn = null;
            slotsHolder = null;
            config = null;
            slotReference = null;
            _slotsSpawner = null;
            _slots = null;
            Matrix = null;
        }
        
        public void Initialize()
        {
            _canSpin = false;
            
            spinBtn.onClick.AddListener(Spin);
            spinBtn.onHoldClick.AddListener(HoldSpin);
            _isAutoSpin = spinBtn.IsPressed;
        }

        public void Show() => rowsView.FadeIn();
        public void Hide() => rowsView.FadeOut();
        
        private void HoldSpin(bool inStatus)
        {
            _isAutoSpin = inStatus;
        }

        private void Spin()
        {
            if(!_canSpin) return;
            _canSpin = false;
        }

        private void SpinComplete()
        {
            _canSpin = true;
        }

        private void ClearMachine()
        {
            if (_slots == null)
            {
                _slots = new List<Slot>();
                return;
            }

            foreach (var slot in _slots)
                slot.RemoveSlot();
            
            _slots.Clear();
        }
        
        public void GenerateSlots(GenerateType inType, Vector2Int inDimension)
        {
            _dimension = inDimension;
            
            ClearMachine();
            switch (inType)
            {
                case GenerateType.Asynchronous:
                    _size = _dimension.x * _dimension.y;
                    SpawnSlotAsync(0);
                    break;
                case GenerateType.Synchronous:
                    var loader = new AssetsLoader();
                    loader.Load<GameObject>(slotReference, GenerateSlotSync);
                    break;
            }
        }

        #region Synchronous Generation
            private void GenerateSlotSync(GameObject inSlotPrefab, AsyncOperationHandle<GameObject> operationHandle)
            {
                var slotSize = inSlotPrefab.GetComponent<RectTransform>().sizeDelta;
                
                Matrix = new Slot[_dimension.x, _dimension.y];
                for (var x = 0; x < _dimension.x; x++)
                {
                    for (var y = 0; y < _dimension.y; y++)
                    {
                        //- define
                        var spacing = new Vector2(config.spacing.x * y, -config.spacing.y * x);
                        var slotPosition = new Vector2(slotSize.x * y, -slotSize.y * x);
                        var location = new Vector2Int(x, y);
                        var targetSlot = Instantiate(inSlotPrefab, slotsHolder).GetComponent<Slot>();
                        
                        //- assign
                        targetSlot.rectTransform.anchoredPosition = config.padding + slotPosition + spacing;
                        targetSlot.Initialize(this, location);
                        targetSlot.name = $"[{location.ToString()}]";
                        Matrix[x, y] = targetSlot;
                    }
                }
                Addressables.Release(operationHandle); //release the slot prefab
            }
        #endregion
        
        #region Asynchronous Generation
             private void SpawnSlotAsync(int index)
            {
                if (index >= _size) AsyncGenerationCompleted();
                else
                {
                    void Next()
                    {
                        index++;
                        SpawnSlotAsync(index);
                    }
                    if(_slotsSpawner == null) _slotsSpawner = new AssetsSpawner();
                    _slotsSpawner.Instantiate<Slot>(slotReference,
                        (response =>
                        {
                            response.rectTransform.SetParent(slotsHolder);
                            _slots.Add(response);
                            Next();
                        }),
                        (error =>
                        {
                            Debug.LogWarning($"Failed to instantiate slot {error}");
                            Next();
                        }));
                }
            }

            private void AsyncGenerationCompleted()
            {
                if (_slots.Count != _size) throw new Exception($"slots List size must be equals the slot machine size {_size} ");
                var slotSize = _slots[0]?.rectTransform.sizeDelta;
                if (!slotSize.HasValue) throw new Exception($"Slot size doesn't have a value");
                
                Matrix = new Slot[_dimension.x, _dimension.y];
                for (var x = 0; x < _dimension.x; x++)
                {
                    for (var y = 0; y < _dimension.y; y++)
                    {
                        //- define
                        var spacing = new Vector2(config.spacing.x * y, -config.spacing.y * x);
                        var slotPosition = new Vector2(slotSize.Value.x * y, -slotSize.Value.y * x);
                        var location = new Vector2Int(x, y);
                        var targetSlot =  _slots[x * _dimension.y + y];
                        
                        //- assign
                        targetSlot.rectTransform.anchoredPosition = config.padding + slotPosition + spacing;
                        targetSlot.Initialize(this, location);
                        targetSlot.name = $"[{location.ToString()}]";
                        Matrix[x, y] = targetSlot;
                    }
                }
            }
        #endregion
        
    }
}