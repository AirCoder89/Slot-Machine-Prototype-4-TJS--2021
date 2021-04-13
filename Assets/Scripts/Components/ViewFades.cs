using AirCoder.TJ.Core;
using AirCoder.TJ.Core.Extensions;
using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ViewFades : MonoBehaviour
    {
        //- exposed variables
        [Header("Establishing")] 
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
        
        [ContextMenu("Fade In")]
        public void FadeIn()
            => _canvasGroup.TweenOpacity(alphaRange.y, duration).SetEase(ease).Play();

        [ContextMenu("Fade Out")]
        public void FadeOut()
            => _canvasGroup.TweenOpacity(alphaRange.x, duration).SetEase(ease).Play();
    }
}
