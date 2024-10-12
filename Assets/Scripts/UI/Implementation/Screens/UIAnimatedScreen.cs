using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DexiDev.Game.UI.Screens
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class UIAnimatedScreen : UIScreen
    {
        [SerializeField] private float _durationAnimActive = 0.35f;
        
        private CanvasGroup _canvasGroup;
        private CancellationTokenSource _cancellationTokenAnimation;

        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _cancellationTokenAnimation?.Cancel();
        }

        public override async void OnShow()
        {
            _root.anchoredPosition = new Vector2(_root.anchoredPosition.x, -Screen.height);
            _canvasGroup.alpha = 0f;
            
            _cancellationTokenAnimation?.Cancel();
            _cancellationTokenAnimation = new();
            
            await SetActiveAnimation(true, _cancellationTokenAnimation.Token);
            base.OnShow();
        }

        public override async void OnHide()
        {
            _cancellationTokenAnimation?.Cancel();
            _cancellationTokenAnimation = new();
            
            await SetActiveAnimation(false, _cancellationTokenAnimation.Token);
            base.OnHide();
        }

        private async UniTask SetActiveAnimation(bool isActive, CancellationToken token)
        {
            Vector2 startPosition = _root.anchoredPosition;
            Vector2 endPosition = new Vector2(_root.anchoredPosition.x, isActive ? 0 : -Screen.height);

            float startAlpha = _canvasGroup.alpha;
            float endAlpha = isActive ? 1f : 0f;
            
            float duration = _durationAnimActive;
            float time = duration;
            while (time > 0 && !token.IsCancellationRequested)
            {
                var step = Mathf.SmoothStep(1f, 0f, time / duration);
                _root.anchoredPosition = Vector2.Lerp(startPosition, endPosition, step);
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, step);
                time -= Time.deltaTime;
                await UniTask.Yield();
            }

            if (!token.IsCancellationRequested)
            {
                _root.anchoredPosition = endPosition;
                _canvasGroup.alpha = endAlpha;
            }
        }
    }
}