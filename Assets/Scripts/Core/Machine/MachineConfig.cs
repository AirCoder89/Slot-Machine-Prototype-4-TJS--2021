using AirCoder.TJ.Core;
using UnityEngine;

namespace Core.Machine
{
    [CreateAssetMenu(menuName = "SlotMachine/new config")]
    public class MachineConfig : ScriptableObject
    {
        [Header("Spinning :")] 
        public Vector2 spinningDurationRange;
        
        [Tooltip("If true user will be able to stop spinning by pressing the spin button again.")]
        public bool forceStopSpin;

        public SpinningType startSpinning;
        public SpinningType endSpinning;
        
        [Tooltip("spinning delay among each column.")]
        [Range(0, 5)] public float individuallyDelay;
        [Tooltip("spinning delay after each auto-spin.")]
        [Range(0, 5)] public float autoSpinDelay;
        
        [Header("Layout :")] 
        public Vector2 spacing;
        public Vector2 padding;

        [Header("Animation :")] 
        [Range(0, 5)] public float speed;
        [Tooltip("spinning brake ease")]
        public EaseType endSpinEase;
        public float endSpinYOffset;
        [Tooltip("spinning brake duration")]
        [Range(0, 5)]public float duration;
       

        [Header("Audio :")] 
        public bool enableAudio = true;
        [Range(0.01f, 2f)] public float endSpinPitchMultiplier = 1f;
    }
}