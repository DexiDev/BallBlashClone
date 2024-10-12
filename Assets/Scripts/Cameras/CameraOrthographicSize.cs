using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DexiDev.Game.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class CameraOrthographicSize : SerializedMonoBehaviour
    {
        [SerializeField, OnValueChanged(nameof(UpdateScreenSize))] private Camera _camera;
        [SerializeField, OnValueChanged(nameof(UpdateScreenSize))] private Vector2 _referenceResolution = new Vector2(1920, 1080);
        [SerializeField, OnValueChanged(nameof(UpdateScreenSize))] private float _referenceOrthographicSize = 5f;

        private Vector2 _screenSize;

        public Camera Camera => _camera;
        public Vector2 ScreenSize => _screenSize;

        public event Action<Vector2> OnScreenSizeChanged;

        private void Awake()
        {
            if(_camera == null) _camera = GetComponent<Camera>();
        }

        private void Start()
        {
            UpdateScreenSize();
        }

        private void Update()
        {
            if (_screenSize.x != Screen.width || _screenSize.y != Screen.height)
            {
                UpdateScreenSize();
            }
        }

        [Button]
        private void UpdateScreenSize()
        {
            if (_camera == null)
            {
                Debug.LogError("Camera is not assigned!");
                return;
            }

            _screenSize = new Vector2(Screen.width, Screen.height);

            float screenAspect = _screenSize.x / _screenSize.y;
            float referenceAspect = _referenceResolution.x / _referenceResolution.y;

            if (screenAspect >= referenceAspect)
            {
                _camera.orthographicSize = _referenceOrthographicSize;
            }
            else
            {
                float differenceInSize = referenceAspect / screenAspect;
                _camera.orthographicSize = _referenceOrthographicSize * differenceInSize;
            }

            OnScreenSizeChanged?.Invoke(_screenSize);
        }
    }
}