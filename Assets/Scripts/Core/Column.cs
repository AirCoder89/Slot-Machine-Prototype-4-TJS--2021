using System;
using System.Collections.Generic;
using AirCoder.TJ.Core.Extensions;
using Core.Machine;
using UnityEngine;

namespace Core
{
    public class Column
    {
        //getter
        private RectTransform _rectTransform
        {
            get
            {
                if (_rt == null) _rt = _gameObject.GetComponent<RectTransform>();
                return _rt;
            }
        }
        
        //- private variables
        private RectTransform _rt;
        private readonly GameObject _gameObject;
        private Vector2? _slotSize;
        private List<Slot> _slots;
        private Slot _fakeSlot;
        private bool _isSpin;
        private SlotMachine _machine;
        
        public Column(string inName, RectTransform parent, SlotMachine inMachine)
        {
            _machine = inMachine;
            _isSpin = false;
            _slotSize = null;
            _gameObject = new GameObject(inName);
            _gameObject.transform.SetParent(parent);
            _rt = _gameObject.AddComponent<RectTransform>();
            _rectTransform.anchoredPosition = Vector2.zero;
        }

        public void AttachSlot(Slot inSlot)
        {
            if(_slots == null) _slots = new List<Slot>();
            inSlot.rectTransform.SetParent(_rectTransform);
            _slots.Add(inSlot);
            if (!_slotSize.HasValue)
                _slotSize = inSlot.rectTransform.sizeDelta;
        }

        public void AttachFakeSlot(Slot inSlot, bool isAssign)
        {
            inSlot.rectTransform.SetParent(_rectTransform);
            if(isAssign) _fakeSlot = inSlot;
        }

        public void Spin()
        {
            _isSpin = true;
        }

        public void Stop(Action callback)
        {
            if(!_isSpin) return;
                _isSpin = false;
                _rectTransform.anchoredPosition = new Vector2(0f, _machine.Config.endSpinYOffset);
                _rectTransform.TweenAnchorPosition(new Vector2(_rectTransform.anchoredPosition.x, -_slotSize.Value.y),
                    _machine.Config.duration).SetEase(_machine.Config.endSpinEase).OnComplete(callback).Play();
        }

        public void Tick(float deltaTime)
        {
            if(!_isSpin) return;
            var current = _rectTransform.anchoredPosition;
            var newPos = current.y - (deltaTime * _machine.Config.speed * 2000f);
            
            if (_slotSize != null && newPos < (0f -_slotSize.Value.y))
            {
                SwapSlots();
                _rectTransform.anchoredPosition = Vector2.zero;
                return;
            }
            _rectTransform.anchoredPosition = new Vector2(0f, newPos);
        }

        private void SwapSlots()
        {
            for (var i = _slots.Count-1; i >= 1; i--)
                _slots[i].SetType(_slots[i-1].CurrentSymbol);
            _slots[0].SetType(_fakeSlot.CurrentSymbol);
            _fakeSlot.RandomSymbol();
        }
    }
}