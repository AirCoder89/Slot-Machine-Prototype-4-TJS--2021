using System;
using System.Collections;
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
        
        //-events
        public event Action onEstablished;
        
        //- private variables
        private AssetsSpawner _slotsSpawner;
        private List<Slot> _slots;
        private int _size;
        private Vector2Int _dimension;
        private bool _isAutoSpin;
        private bool _canSpin;
        private List<Column> _columns;
        
        //- properties
        public Slot[,] Matrix { get; private set; }
        public MachineConfig Config => config;
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
            _columns = null;
        }
        
        public void Initialize()
        {
            Debug.Log($"Initialize Machine");
            spinBtn.onClick.AddListener(Spin);
            spinBtn.onHoldClick.AddListener(HoldSpin);
            spinBtn.IsInteractable = false;
            _isAutoSpin = spinBtn.IsPressed;
            rowsView.HideImmediately();
        }

        public void Show() => rowsView.FadeIn();
        public void Hide() => rowsView.FadeOut();
        
        private void HoldSpin(bool inStatus)
        {
            Debug.Log($"Hold Spin {inStatus}");
            _isAutoSpin = inStatus;
            if(_isAutoSpin) Spin();
            else StopSpinning();
        }

        private void Spin()
        {
            if(!_canSpin) return;
            _canSpin = false;
            foreach (var column in _columns)
                column.Spin();
        }

        private void StopSpinning()
        {
            foreach (var column in _columns)
                column.Stop(SpinComplete);
            _canSpin = true;
        }
        
        private void SpinComplete()
        {
            _canSpin = true;
            if (_isAutoSpin) StartCoroutine(WaitAndSpin(Config.delayAmongSpin));
        }

        private IEnumerator WaitAndSpin(float inDelay)
        {
            yield return new WaitForSeconds(inDelay);
            Spin();
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
                Matrix = new Slot[_dimension.x, _dimension.y];
                for (var x = 0; x < _dimension.x; x++)
                {
                    for (var y = 0; y < _dimension.y; y++)
                    {
                        //- define
                        var location = new Vector2Int(x, y);
                        var targetSlot = Instantiate(inSlotPrefab, slotsHolder).GetComponent<Slot>();
                        
                        //- assign
                        targetSlot.Initialize(this, location);
                        Matrix[x, y] = targetSlot;
                    }
                }
                Addressables.Release(operationHandle); //release the slot prefab
                AssignColumns();
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
                        var location = new Vector2Int(x, y);
                        var targetSlot =  _slots[x * _dimension.y + y];
                        
                        //- assign
                         targetSlot.Initialize(this, location);
                        Matrix[x, y] = targetSlot;
                    }
                }
                AssignColumns();
            }
        #endregion

        [ContextMenu("Assign Columns")]
        private void AssignColumns()
        {
            _columns = new List<Column>();
            for (var y = 0; y < _dimension.y; y++)
            {
                var column = new Column($"Col [{y}]", slotsHolder, this);
                for (var x = 0; x < _dimension.x; x++)
                    column.AttachSlot(Matrix[x,y]);
                _columns.Add(column);
            }
            AddFakeRow();
        }
        
        [ContextMenu("Add Fake Row")]
        private void AddFakeRow()
        {
            var loader = new AssetsLoader();
            loader.Load<GameObject>(slotReference, (prefab, handle) =>
            {
                for (var i = 0; i < _dimension.y; i++)
                {
                    var slot = Instantiate(prefab, slotsHolder).GetComponent<Slot>();
                    slot.Initialize(this, new Vector2Int(-1, i));
                    _columns[i].AttachFakeSlot(slot, true);
                    
                    var slot2 = Instantiate(prefab, slotsHolder).GetComponent<Slot>();
                    slot2.Initialize(this, new Vector2Int(-2, i));
                    _columns[i].AttachFakeSlot(slot2, false);
                }
                Addressables.Release(handle);
                SlotMachineEstablished();
            });
        }

        private void SlotMachineEstablished()
        {
            spinBtn.IsInteractable = true;
            _canSpin = true;
            Show();
            onEstablished?.Invoke();
        }

        public void Tick(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopSpinning();
            }
            
            foreach (var column in _columns)
                column.Tick(deltaTime);
        }

    }
    
}

