using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DexiDev.Game.Assets.Assets
{
    [RequireComponent(typeof(ParticleSystem))]
    [DisallowMultipleComponent]
    public class ParticleAsset : SerializedMonoBehaviour, IAssetInstance
    {
        [SerializeField] private ParticleSystem _particle;
        [field: SerializeField] public AssetGroupData AssetGroup { get; private set; }
        
        public IAssetContract Contract { get; set; }
        
        public event Action<IAssetInstance> OnPoolable;
        public event Action<IAsset> OnReleased;

        public GameObject Instance => gameObject;

        private void Awake()
        {
            if(_particle == null) _particle = GetComponent<ParticleSystem>();
        }

        private void OnDisable()
        {
            OnPoolable?.Invoke(this);
        }

        protected virtual void OnDestroy()
        {
            OnReleased?.Invoke(this);
        }

        [Button]
        public void Play()
        {
            _particle.Play();
        }

        [Button]
        public void Stop()
        {
            _particle.Stop();
        }
    }
}