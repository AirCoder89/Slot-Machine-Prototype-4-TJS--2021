using System;
using AirCoder.TJ.Core;
using AirCoder.TJ.Core.Extensions;
using UnityEngine;

namespace AirCoder.Examples.Example2.Scripts
{
    [RequireComponent(typeof(RectTransform))]
    public class UIPanel : MonoBehaviour
    {
        [Header("Appearance settings")]
        [SerializeField] private Vector2 startPosition = Vector2.zero;
        [SerializeField] private Vector2 startScale = Vector2.one;
        [SerializeField] private float duration;
        [SerializeField] private EaseType ease;
        
        private RectTransform _rt;
        private RectTransform rectTransform
        {
            get
            {
                if (_rt == null) _rt = GetComponent<RectTransform>();
                return _rt;
            }
        }

        private Vector2 _originalPos;
        private Vector2 _originalScale;

        public virtual void Initialize()
        {
            _originalPos = rectTransform.anchoredPosition;
            _originalScale = rectTransform.localScale;
            rectTransform.anchoredPosition = startPosition;
            rectTransform.localScale = startScale;
        }

        public virtual void OpenPanelImmediately()
        {
            rectTransform.anchoredPosition = _originalPos;
            rectTransform.localScale = _originalScale;
        }

        public void OpenPanel() => OpenPanel(null);
        public virtual void OpenPanel(Action callBack = null)
        {
            rectTransform.TweenAnchorPosition(_originalPos, duration).SetEase(ease).Play();
            rectTransform.TweenScale(_originalScale, duration).SetEase(ease).OnComplete(
                () =>
                {
                    callBack?.Invoke();
                }).Play();
        }
        
        public virtual void ClosePanelImmediately()
        {
            rectTransform.anchoredPosition = startPosition;
            rectTransform.localScale = startScale;
        }

        public void ClosePanel() => ClosePanel(null);
        public virtual void ClosePanel(Action callBack = null)
        {
            rectTransform.TweenAnchorPosition(this.startPosition, this.duration).SetEase(this.ease).Play();
            rectTransform.TweenScale(this.startScale, this.duration).SetEase(this.ease).OnComplete(
                () =>
                {
                    callBack?.Invoke();
                }
            ).Play();
        }



    }
}
