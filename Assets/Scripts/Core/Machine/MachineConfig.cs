using AirCoder.TJ.Core;
using UnityEngine;

namespace Core.Machine
{
    [CreateAssetMenu(menuName = "SlotMachine/new config")]
    public class MachineConfig : ScriptableObject
    {
        [Header("General")] public float delayAmongSpin;
        [Header("Layout")] 
        public Vector2 spacing;
        public Vector2 padding;

        [Header("Animation")] public float speed;
        public EaseType startSpinEase;
        public EaseType endSpinEase;
        public float duration;
        public bool useBlurEffect;
    }
}