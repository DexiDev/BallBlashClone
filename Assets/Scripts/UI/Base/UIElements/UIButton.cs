using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace DexiDev.Game.UI.UIElements
{
    public class UIButton : UIElement
    {
        [SerializeField] protected Button _button;

        public Button Button => _button;
        
        public event Action OnClick;
        
        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(OnClickHandle);
        }

        protected override void OnDisable()
        {
            _button.onClick.RemoveListener(OnClickHandle);
            base.OnDisable();
        }

        protected virtual void OnClickHandle()
        {
            OnClick?.Invoke();
        }
    }
}