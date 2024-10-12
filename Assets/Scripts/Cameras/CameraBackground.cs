using Sirenix.OdinInspector;
using UnityEngine;

namespace DexiDev.Game.Cameras
{
    public class CameraBackground : SerializedMonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] private CameraOrthographicSize _cameraOrthographicSize;

        private void OnEnable()
        {
            OnOrthographicSizeChanged(_cameraOrthographicSize.ScreenSize);
            _cameraOrthographicSize.OnScreenSizeChanged += OnOrthographicSizeChanged;
        }

        private void OnDisable()
        {
            _cameraOrthographicSize.OnScreenSizeChanged -= OnOrthographicSizeChanged;
        }

        private void OnOrthographicSizeChanged(Vector2 screenSize)
        {
            float screenRatio = screenSize.x / screenSize.y;
            float targetRatio = _spriteRenderer.sprite.bounds.size.x / _spriteRenderer.sprite.bounds.size.y;

            if (screenRatio >= targetRatio)
            {
                float scaleX = _cameraOrthographicSize.Camera.orthographicSize * 2 * screenRatio / _spriteRenderer.sprite.bounds.size.x;
                transform.localScale = new Vector3(scaleX, scaleX, 1);
            }
            else
            {
                float scaleY = _cameraOrthographicSize.Camera.orthographicSize * 2 / _spriteRenderer.sprite.bounds.size.y;
                transform.localScale = new Vector3(scaleY, scaleY, 1);
            }
        }

#if UNITY_EDITOR
        [Button]
        public void UpdateSize()
        {
            OnOrthographicSizeChanged(new Vector2(Screen.width, Screen.height));
        }
#endif
    }
}