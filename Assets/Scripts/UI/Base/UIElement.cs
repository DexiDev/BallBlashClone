using System;
using DexiDev.Game.Assets;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace DexiDev.Game.UI
{
    public class UIElement : SerializedMonoBehaviour, IAssetInstance
    {
        [SerializeField] private UIElementType _uiElementType;
        [SerializeField] private AssetGroupData _assetGroupData;
        
        protected RectTransform _rectTransform;
        
        public IAssetContract Contract { get; set; }
        public GameObject Instance => gameObject;
        public AssetGroupData AssetGroup => _assetGroupData;
        public RectTransform RectTransform => _rectTransform;
        public UIElementType UIElementType => _uiElementType;
        
        public event Action<IAssetInstance> OnPoolable;
        public event Action<IAsset> OnReleased;

        protected virtual void Awake()
        {
            _rectTransform = transform as RectTransform;
        }

        protected virtual void OnDisable()
        {
            OnPoolable?.Invoke(this);
        }
        
        public virtual void OnShow(){}

        public virtual void OnHide()
        {
            if(!gameObject.IsDestroyed()) gameObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            OnReleased?.Invoke(this);
        }
    }
}