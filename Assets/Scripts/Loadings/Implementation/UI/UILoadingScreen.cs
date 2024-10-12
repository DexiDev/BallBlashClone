using System;
using DexiDev.Game.Assets;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DexiDev.Game.Loadings.UI
{
    public class UILoadingScreen : SerializedMonoBehaviour, IAssetInstance
    {
        [SerializeField] private Image _progressBar;
        [field: SerializeField] public AssetGroupData AssetGroup { get; private set; }
        
        private int _countTask = 0;
        private int _currentTask = 0;
        
        public IAssetContract Contract { get; set; }
        public GameObject Instance => gameObject;
        public event Action<IAsset> OnReleased;
        public event Action<IAssetInstance> OnPoolable;
        
        private void OnEnable()
        {
            _countTask = 0;
            _currentTask = 0;
        }

        public void AddTask(int count = 1)
        {
            _countTask += count;
        }

        public void CompletedTask(int count = 1)
        {
            _currentTask += count;
        }

        public void SetProgress(float taskProgress)
        {
            var progress = ((float)_currentTask / _countTask) + (taskProgress / _countTask);
            _progressBar.fillAmount = progress;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        protected virtual void OnDisable()
        {
            OnPoolable?.Invoke(this);
        }

        protected virtual void OnDestroy()
        {
            OnReleased?.Invoke(this);
        }
    }
}