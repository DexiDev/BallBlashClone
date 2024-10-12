using System;
using DexiDev.Game.Assets;
using DexiDev.Game.Levels.Fields;
using DexiDev.Game.Levels.Rewards;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace DexiDev.Game.Levels.Nodes
{
    public abstract class LevelNode : SerializedMonoBehaviour, IAssetInstance
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected AssetGroupData _assetGroupData;
        [SerializeField] protected Collider2D _collider;
        [SerializeField] protected LevelNodeReward _nodeRewardContract;
        
        [Inject] protected AssetsManager _assetsManager;
        
        protected int _currentHealth;
        
        public int Health => _currentHealth;
        
        public bool IsAlive => _currentHealth > 0;
        
        public IAssetContract Contract { get; set; }
        public GameObject Instance => gameObject;
        public AssetGroupData AssetGroup => _assetGroupData;
        
        public event Action<IAssetInstance> OnPoolable;
        public event Action<IAsset> OnReleased;
        public event Action<int> OnReceiveDamage;
        public event Action<LevelNode> OnReward;
        
        private void OnDisable()
        {
            OnPoolable?.Invoke(this);
        }

        private void OnDestroy()
        {
            OnReleased?.Invoke(this);
        }

        [Button]
        public virtual void ReceiveDamage(int damage)
        {
            if (!IsAlive) return;
            
            SetHealth(_currentHealth - damage);
            
            OnReceiveDamage?.Invoke(damage);
            
            if (_currentHealth <= 0)
            {
                Reward();
            }
        }

        protected virtual void SetHealth(int health)
        {
            _currentHealth = health;
        }
        
        [Button]
        public virtual void Reward()
        {
            if (_nodeRewardContract != null)
            {
                var levelNodeReward = _assetsManager.GetAsset<LevelNodeReward>(_nodeRewardContract, transform.position, Quaternion.identity, null);
                ShowReward(levelNodeReward);
            }
            
            gameObject.SetActive(false);
            OnReward?.Invoke(this);
        }

        protected abstract void ShowReward(LevelNodeReward levelNodeReward);
    }
    
    public abstract class LevelNode<T> : LevelNode where T : LevelNodeField
    {
        protected T _dataField;

        public virtual void SetData(T dataField)
        {
            _dataField = dataField;
            SetHealth(dataField.Value);
        }

        protected override void ShowReward(LevelNodeReward levelNodeReward)
        {
            levelNodeReward.SetData(_dataField);
        }
    }
}