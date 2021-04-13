using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Components
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Outline))]
    public class SpinButton : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IPointerClickHandler
    {
        //- exposed variables
        [Header("Establishing:")] 
        [SerializeField] private bool isPressed;
        [SerializeField] private Color pressedColorState;
        [SerializeField][Range(0.01f, 5f)] private float holdClickDuration;
        [Tooltip("If true onClick event will be triggered beside onHoldClick.")][SerializeField] 
        private bool fireOnClickIfHold;
        [Tooltip("If true user can cancel holding click by a simple click.")][SerializeField] 
        private bool cancelHoldIfClick;
        
        [Header("Events:")]
        public UnityEvent onClick;
        public UnityEvent<bool> onHoldClick;

        //- getter
        private Image _background
        {
            get
            {
                if (_bg == null) _bg = GetComponent<Image>();
                return _bg;
            }
        }
        public Outline _outline
        {
            get
            {
                if (_line == null) _line = GetComponent<Outline>();
                return _line;
            }
        }
        
        //- private variables
        private Image _bg;
        private Outline _line;
        private float _timeCounter;
        private bool _isMouseDown;
        private bool _isHoldComplete;
        
        private void Start()
        {
            _timeCounter = 0f;
            _isMouseDown = false;
            _isHoldComplete = false;
            UpdateShape();
            
            onClick.AddListener(TestClick);
            onHoldClick.AddListener(TestHoldClick);
        }

        public void TestClick() => Debug.Log($"Click !");
        public void TestHoldClick(bool inState) => Debug.Log($"Hold Click {inState}");
        private void UpdateShape()
        {
            _background.color = isPressed ? pressedColorState : Color.white;
            _outline.enabled = isPressed;
        }

        private void Update()
        {
            if (!_isMouseDown) return;
            _timeCounter += Time.deltaTime;
            if (!(_timeCounter >= holdClickDuration) || _isHoldComplete) return;
               OnHoldClick();
        }

        private void OnHoldClick()
        {
            _timeCounter = 0f;
            _isHoldComplete = true;
            isPressed = !isPressed;
            onHoldClick?.Invoke(isPressed);
            UpdateShape();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _timeCounter = 0f;
            _isMouseDown = true;
            _isHoldComplete = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isMouseDown = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isHoldComplete && fireOnClickIfHold)  onClick?.Invoke();
            else if (isPressed && !_isHoldComplete && cancelHoldIfClick)
            {
                OnHoldClick();
                onClick?.Invoke();
            }
            else if(!_isHoldComplete) onClick?.Invoke();
        }
    }
}
