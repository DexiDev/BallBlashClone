using UnityEngine;

namespace DexiDev.Game.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UIScreen : UIElement
    {
        [SerializeField] protected RectTransform _root;
        
        protected override void Awake()
        {
            _rectTransform = _root;
        }
    }
}