using System;
using DexiDev.Game.Assets;
using DexiDev.Game.Assets.Assets;
using DexiDev.Game.Items;
using DexiDev.Game.Items.Fields;
using DexiDev.Game.Levels.Fields;
using DexiDev.Game.Levels.UI;
using DexiDev.Game.Rewards;
using DexiDev.Game.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace DexiDev.Game.Levels.Rewards
{
    public class LevelNodeReward : SerializedMonoBehaviour, IAssetInstance
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private AssetGroupData _assetGroupData;
        [SerializeField] private ParticleAsset _pickUpParticleContract;
        [SerializeField] private UINodeReward _uiNodeRewardContract;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _collisionTorque = 50f;
        
        [Inject] private AssetsManager _assetsManager;
        [Inject] private RewardManager _rewardManager;
        [Inject] private UIManager _uiManager;
        [Inject] private LevelManager _levelManager;
        
        private LevelNodeField _levelNodeField;
        
        public IAssetContract Contract { get; set; }

        public GameObject Instance => gameObject;
        public AssetGroupData AssetGroup => _assetGroupData;
        
        public event Action<IAssetInstance> OnPoolable;
        public event Action<IAsset> OnReleased;

        private Sprite _defaultSprite;

        private void Awake()
        {
            _defaultSprite = _spriteRenderer.sprite;
        }

        private void OnEnable()
        {
            _rigidbody.AddTorque(_collisionTorque);
        }

        private void OnDisable()
        {
            OnPoolable?.Invoke(this);
        }

        private void OnDestroy()
        {
            OnReleased?.Invoke(this);
        }
        
        public void SetData(LevelNodeField levelNodeField)
        {
            _levelNodeField = levelNodeField;

            if (levelNodeField is LevelNodeCustomField levelNodeCustomField)
            {
                _spriteRenderer.sprite = levelNodeCustomField.Data.SpriteReward;
            }
            else _spriteRenderer.sprite = _defaultSprite;
        }

        [Button]
        public void PickUp()
        {
            if (!gameObject.activeSelf) return;
            
            _rewardManager.Reward(_levelNodeField.Reward);

            if(_pickUpParticleContract != null)
            {
                _assetsManager.GetAsset<ParticleAsset>(_pickUpParticleContract, transform.position, Quaternion.identity, null);
            }

            if (_uiNodeRewardContract != null)
            {
                UINodeReward uiNodeReward = _uiManager.ShowElement(_uiNodeRewardContract);
                
                Camera camera = _levelManager?.LevelController?.Camera;

                if (camera == null) camera = Camera.main;
                
                var point = camera.WorldToScreenPoint(transform.position);
                
                uiNodeReward.transform.position = point;
                
                if (uiNodeReward != null)
                {
                    uiNodeReward.SetReward(_levelNodeField.Reward);
                }
            }
            
            gameObject.SetActive(false);
        }
    }
}