using System;
using AirCoder.TJ.Core;
using AirCoder.TJ.Core.Extensions;
using Core;
using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(RectTransform))]
    public class ViewTweener : BaseMonoBehaviour
    {
        [SerializeField] private bool closeOnStart;
        [SerializeField] private bool autoOpen;
        [SerializeField] private Vector2 startPosition = Vector2.zero;
        [SerializeField] private Vector2 startScale = Vector2.one;
        [SerializeField] [Range(0f,5f)] private float duration;
        [SerializeField] private EaseType ease;

        private Vector2 _targetPosition;
        private Vector2 _targetScale;
        private bool _isInitialized;
        
        private RectTransform _rt;
        private RectTransform _rectTransform
        {
            get
            {
                if (_rt == null) _rt = GetComponent<RectTransform>();
                return _rt;
            }
        }
        
        protected override void ReleaseReferences()
        {
            _rt = null;
        }

        private void Awake()
        {
            Initialize();
            if(closeOnStart) CloseImmediately();
        }
        [ContextMenu("Initialize")]
        private void Initialize()
        {
            _targetPosition = _rectTransform.anchoredPosition;
            _targetScale = _rectTransform.localScale;
            _isInitialized = true;
        }

        private void Start()
        {
            if(autoOpen) Open();
        }

        [ContextMenu("OpenImmediately")]
        public void OpenImmediately()
        {
            if(!_isInitialized) Initialize();
            _rectTransform.anchoredPosition = _targetPosition;
            _rectTransform.localScale = _targetScale;
        }

        [ContextMenu("Open")]
        private void Open() => Open(null);
        
        public void Open(Action callback = null)
        {
            if(!_isInitialized) Initialize();
            _rectTransform.TweenAnchorPosition(_targetPosition, duration).SetEase(ease).Play();
            _rectTransform.TweenScale(_targetScale, duration).SetEase(ease)
                .OnComplete(() => { callback?.Invoke(); }).Play();
        }
        [ContextMenu("CloseImmediately")]
        public void CloseImmediately()
        {
            if(!_isInitialized) Initialize();
            _rectTransform.anchoredPosition = startPosition;
            _rectTransform.localScale = startScale;
        }
        
        [ContextMenu("Close")]
        private void Close() => Close(null);
        
        public void Close(Action callback = null)
        {
            if(!_isInitialized) Initialize();
            _rectTransform.TweenAnchorPosition(startPosition, duration).SetEase(ease).Play();
            _rectTransform.TweenScale(startScale, duration).SetEase(ease)
                .OnComplete(() => { callback?.Invoke(); }).Play();
        }
    }
}