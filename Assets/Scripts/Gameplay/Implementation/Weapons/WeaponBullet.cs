using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DexiDev.Game.Assets;
using DexiDev.Game.Assets.Assets;
using DexiDev.Game.Levels.Nodes;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace DexiDev.Game.Gameplay.Weapons
{
    public class WeaponBullet : SerializedMonoBehaviour, IAssetInstance
    {
        [SerializeField] private AssetGroupData _assetGroupData;
        [SerializeField] private float _speed;
        [SerializeField] private ParticleAsset _explodeParticleContract;
        [SerializeField] private LayerMask _layerShoot;
        
        [Inject] private AssetsManager _assetsManager;
        
        public AssetGroupData AssetGroup => _assetGroupData;
        public GameObject Instance => gameObject;
        public IAssetContract Contract { get; set; }
        
        public event Action<IAssetInstance> OnPoolable;
        public event Action<IAsset> OnReleased;
        
        private int _damage;
        private float _speedFactor = 1f;
        private RaycastHit2D[] _cachedHits = new RaycastHit2D[1];
        private CancellationTokenSource _cancellationToken;
        
        private void OnDisable()
        {
            OnPoolable?.Invoke(this);
        }

        private void OnDestroy()
        {
            _cancellationToken?.Cancel();
            OnReleased?.Invoke(this);
        }

        public void Init(float speedFactor, int damage)
        {
            _speedFactor = speedFactor;
            _damage = damage;
        }

        protected virtual void LateUpdate()
        {
            float adjustedSpeed = _speed / _speedFactor;
            
            var newPosition = transform.position + Vector3.up * adjustedSpeed * Time.deltaTime;
            var direction = (newPosition - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, newPosition);

            Debug.DrawRay(transform.position, direction * distance, Color.red, Time.deltaTime);
            if (Physics2D.RaycastNonAlloc(transform.position, direction, _cachedHits, distance, _layerShoot) > 0)
            {
                if (_cachedHits[0].transform.TryGetComponent(out LevelNode levelNode))
                {
                    transform.position = _cachedHits[0].point;
                    DelayExplode(levelNode);
                    return;
                }
            }
            
            transform.position = newPosition;
        }

        private async void DelayExplode(LevelNode levelNode)
        {
            _cancellationToken?.Cancel();
            _cancellationToken = new();

            var cancellationToken = _cancellationToken.Token;
            try
            {
                await UniTask.Yield(cancellationToken: cancellationToken);
                if (!cancellationToken.IsCancellationRequested && levelNode.IsAlive)
                {
                    levelNode.ReceiveDamage(_damage);
                    Explode();
                }
            }
            catch(OperationCanceledException){}
        }

        [Button]
        public void Explode()
        {
            if (_explodeParticleContract != null)
            {
                _assetsManager.GetAsset<ParticleAsset>(_explodeParticleContract, transform.position, Quaternion.identity, null);
            }
            
            gameObject.SetActive(false);
        }
    }
}