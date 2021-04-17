using AirCoder.TJ.Core;
using AirCoder.TJ.Core.Extensions;
using Core;
using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ViewFades : BaseMonoBehaviour
    {
        //- exposed variables
        [Header("Establishing")] 
        [SerializeField] private bool hideOnStart;
        [SerializeField] private bool autoFadeIn;
        [SerializeField][Range(0f,5f)] private float duration;
        [SerializeField] private EaseType ease;
        [SerializeField] private Vector2 alphaRange = new Vector2(0f,1f);

        //- getter properties
        private CanvasGroup _canvasGroup
        {
            get
            {
                if (_group == null) _group = GetComponent<CanvasGroup>();
                return _group;
            }
        }
        
        //- private variables
        private CanvasGroup _group;
        
        protected override void ReleaseReferences()
        {
            _group = null;
        }

        public void ShowImmediately() => _canvasGroup.alpha = alphaRange.y;
        public void HideImmediately() => _canvasGroup.alpha = alphaRange.x;

        private void Awake()
        {
            if(hideOnStart) HideImmediately();
        }

        private void Start()
        {
            if(autoFadeIn) FadeIn();
        }

        [ContextMenu("Fade In")]
        public void FadeIn()
            => _canvasGroup.TweenOpacity(alphaRange.y, duration).SetEase(ease).Play();

        [ContextMenu("Fade Out")]
        public void FadeOut()
            => _canvasGroup.TweenOpacity(alphaRange.x, duration).SetEase(ease).Play();

        
    }
}
