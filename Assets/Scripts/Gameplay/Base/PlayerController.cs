using System;
using DexiDev.Game.Data;
using DexiDev.Game.Data.Fields;
using DexiDev.Game.Levels;
using DexiDev.Game.Levels.Rewards;
using UnityEngine;
using VContainer;

namespace DexiDev.Game.Gameplay
{
    public class PlayerController : DataController
    {
        [SerializeField] private float _speedMoving = 5f;
        [SerializeField] private BoxCollider2D _boxCollider;
        
        [Inject] private GameplayManager _gameplayManager;
        [Inject] private LevelManager _levelManager;

        private DirectionField _directionField;
        private IsBattleStateField _isBattleStateField;

        private LevelController _levelController;

        private Vector2 _minMaxPosition;
        
        private void Awake()
        {
            _directionField = GetDataField<DirectionField>(true);
            _isBattleStateField = GetDataField<IsBattleStateField>(true);
            _isBattleStateField.SetValue(true);
            
            _gameplayManager.SetPlayerController(this);
        }

        private void OnEnable()
        {
            OnLevelControllerChangedHandler(_levelManager.LevelController);
            _levelManager.OnLevelControllerChanged += OnLevelControllerChangedHandler;
        }

        private void OnDisable()
        {
            _levelManager.OnLevelControllerChanged -= OnLevelControllerChangedHandler;
            OnLevelControllerChangedHandler(null);
        }


        private void InitMinMaxPosition()
        {
            var camera = _levelController.Camera;
            
            float cameraHeight = camera.orthographicSize;
            float cameraWidth = cameraHeight * camera.aspect;
            
            float halfPlayerWidth = _boxCollider.bounds.extents.x;
            
            _minMaxPosition.x = camera.transform.position.x - cameraWidth + halfPlayerWidth; 
            _minMaxPosition.y = camera.transform.position.x + cameraWidth - halfPlayerWidth;
        }
        
        private void OnLevelControllerChangedHandler(LevelController levelController)
        {
            if (_levelController != null)
            {
                _levelController.OnCompletedLevel -= OnCompletedLevelHandler;
            }

            _levelController = levelController;
            
            if (_levelController != null)
            {
                _levelController.OnCompletedLevel += OnCompletedLevelHandler;
                InitMinMaxPosition();
            }
        }

        private void OnCompletedLevelHandler(bool isWin)
        {
            _isBattleStateField.SetValue(false);
        }

        private void OnDestroy()
        {
            if (_gameplayManager.PlayerController == this)
            {
                _gameplayManager.SetPlayerController(null);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out LevelNodeReward levelNodeReward))
            {
                levelNodeReward.PickUp();
            }
        }

        public void Move(Vector3 moveDirection)
        {
            var newPosition = transform.position + moveDirection * _speedMoving;
            
            newPosition.x = Mathf.Clamp(newPosition.x, _minMaxPosition.x, _minMaxPosition.y);

            transform.position = newPosition;
        }
    }
}