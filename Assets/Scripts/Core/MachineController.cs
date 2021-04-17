
using Core.Audio;
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
        public static AudioSystem Audio => _instance.audioSystem;
        
        //- exposed fields
        [SerializeField] private SlotMachine slotMachine;
        [SerializeField] private SymbolsMap symbolsMap;
        [SerializeField] private AudioSystem audioSystem;
        [SerializeField] private GenerateType generateType;
        [SerializeField] private Vector2Int dimension;
        
        //- private variables
        private bool _isRun;
        
        protected override void ReleaseReferences()
        {
            slotMachine.onEstablished -= Launch;
            symbolsMap.ReleaseReferences();
            symbolsMap = null;
            slotMachine = null;
            audioSystem = null;
        }

        private void Awake()
        {
            if(_instance != null) return;
            _instance = this;
        }

        private void Start()
        {
            //- entry point!
            slotMachine.onEstablished += Launch;
            slotMachine.Initialize();
            audioSystem.Initialize();
            StartLoadingSymbols();
        }

        private void StartLoadingSymbols()
        {
            symbolsMap.LoadAllSymbols(() => { slotMachine.GenerateSlots(generateType, dimension); });
        }

        private void Launch()
        {
            //- this will be called after the slot machine established
            _isRun = true;
        }
        
        private void Update()
        {
            if(!_isRun) return;
            slotMachine.Tick(Time.deltaTime);
            audioSystem.Tick();
        }
    }
}
