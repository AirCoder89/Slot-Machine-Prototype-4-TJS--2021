
using Core.Machine;
using Core.Symbols;
using UnityEngine;
using Vector2Int = Helper.Vector2Int;

namespace Core
{
    public class MachineController : BaseMonoBehaviour
    {
        //- singleton
        private static MachineController _instance;
        
        //- static dependencies
        public static SlotMachine Machine => _instance.slotMachine;
        public static SymbolsMap SymbolsMap => _instance.symbolsMap;
        
        //- exposed fields
        [SerializeField] private SlotMachine slotMachine;
        [SerializeField] private SymbolsMap symbolsMap;
        [SerializeField] private GenerateType generateType;
        [SerializeField] private Vector2Int dimension;
        
        //- private variables

        protected override void ReleaseReferences()
        {
            symbolsMap.ReleaseReferences();
            symbolsMap = null;
            slotMachine = null;
        }

        private void Awake()
        {
            if(_instance != null) return;
            _instance = this;
        }

        private void Start()
        {
            //- entry point!
            Debug.Log($"Start Controller");
            slotMachine.Initialize();
            StartLoadingSymbols();
        }

        private void StartLoadingSymbols()
        {
            Debug.Log($"StartLoading Symbols");
            symbolsMap.LoadAllSymbols((() =>
            {
                Debug.Log($"Loading Symbols Completed !!");
                slotMachine.GenerateSlots(generateType, dimension);
            }));
        }
    }
}
