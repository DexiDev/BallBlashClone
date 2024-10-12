using System;
using System.Collections.Generic;
using System.Linq;
using DexiDev.Game.Assets;
using DexiDev.Game.Levels.Nodes;
using DexiDev.Game.Levels.UI;
using DexiDev.Game.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace DexiDev.Game.Levels
{
    public class LevelController : SerializedMonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private int _rowLimit;
        [SerializeField] private float _minYNodes;
        [SerializeField] private UIWinScreen _uiWinScreenContract;
        [SerializeField] private UILoseScreen _uiLoseScreenContract;
        [SerializeField] private Camera _camera;
        
        [Inject] private AssetsManager _assetsManager;
        [Inject] private LevelManager _levelManager;
        [Inject] private UIManager _uiManager;

        [ShowInInspector] private float _currentSpeed;

        private bool _isStop;
        private int _currentRow;
        private LevelData _levelData;

        public Camera Camera => _camera;
        public List<HashSet<LevelNode>> _levelNodes = new List<HashSet<LevelNode>>();
        
        public event Action OnWinLevel;
        public event Action OnLoseLevel;
        public event Action<bool> OnCompletedLevel;
        
        private void Awake()
        {
            SetData(_levelManager.CurrentLevelID);
        }
        
        private void OnEnable()
        {
            _levelManager.RegisterController(this);
        }

        private void OnDisable()
        {
            ClearNodes();
            _levelManager.UnregisterController(this);
        }
        
        public void SetData(string levelData)
        {
            SetData(_levelManager.GetData(levelData));
        }

        [Button]
        public void SetData(LevelData levelData)
        {
            _levelData = levelData;
            
            if (_levelData != null)
            {
                ClearNodes();
                _currentRow = 0;
                _levelNodes = new();
                _currentSpeed = _levelData.Speed;
                CreateNodes();
            }
        }
        
        private void CreateNodes()
        {
            while (_levelNodes.Count < _rowLimit && _currentRow < _levelData.LevelNodeFields.Length)
            {
                var yPos = _offset.y * _currentRow;
                HashSet<LevelNode> levelNodes = new HashSet<LevelNode>();
                for (int j = 0; j < _levelData.LevelNodeFields[_currentRow].Length; j++)
                {
                    Vector3 nodePosition = _container.position;
                    nodePosition.y += yPos;

                    if (j > 0)
                    {
                        int x = (int)Math.Ceiling((float)j / 2);
                        
                        if (j % 2 == 0) nodePosition.x -= _offset.x * x;
                        else nodePosition.x += _offset.x * x;
                    }

                    ILevelNodeField nodeField = _levelData.LevelNodeFields[_currentRow][j];

                    LevelNode levelNode = null;
                    if (nodeField != null)
                    {
                        levelNode = _levelManager.GetNode(nodeField, nodePosition, _container);

                        if (levelNode != null)
                        {
                            levelNode.OnReward += LevelNodeRewardHandle;
                        }
                    }

                    levelNodes.Add(levelNode);
                }
                _levelNodes.Add(levelNodes);
                _currentRow++;
            }
            
            if (_levelNodes.Count == 0) Win();
        }

        private void LevelNodeRewardHandle(LevelNode levelNode)
        {
            levelNode.OnReward -= LevelNodeRewardHandle;
            
            foreach (var levelNodes in _levelNodes)
            {
                if (levelNodes.Remove(levelNode)) break;
            }

            if (_levelNodes.FirstOrDefault() is { Count: 0 })
            {
                int skipNodes = 0;
                
                for (int i = 0; i < _levelNodes.Count; i++)
                {
                    if (_levelNodes[i].Count == 0) skipNodes++;
                    else break;
                }

                if (skipNodes > 0) _levelNodes = _levelNodes.Skip(skipNodes).ToList();
            }
            
            if (_levelNodes.Count < _rowLimit) CreateNodes();
        }

        public void ClearNodes()
        {
            if (_levelNodes == null) return;
            
            foreach (var levelNodes in _levelNodes)
            {
                foreach (var levelNode in levelNodes)
                {
                    levelNode.OnReward -= LevelNodeRewardHandle;
                    levelNode.gameObject.SetActive(false);
                }
            }
            _levelNodes.Clear();
        }

        private void Update()
        {
            if (_isStop) return;
            
            if (_levelData != null)
            {
                _container.position += Vector3.down * _currentSpeed * Time.deltaTime;
                _currentSpeed += _levelData.Acceleration * Time.deltaTime;
            }

            if (_levelNodes.Any(levelNodes => levelNodes.Any(levelNode => levelNode.transform.position.y <= _minYNodes)))
            {
                Lose();
            }
        }
        
        [Button]
        public void Win()
        {
            Debug.Log("Completed Level");
            _isStop = true;
            _levelManager.IncreaseLevel();
            _uiManager.ShowElement(_uiWinScreenContract);
            OnWinLevel?.Invoke();
            OnCompletedLevel?.Invoke(true);
        }

        [Button]
        public void Lose()
        {
            Debug.Log("Lose Level");
            _isStop = true;
            _uiManager.ShowElement(_uiLoseScreenContract);
            OnLoseLevel?.Invoke();
            OnCompletedLevel?.Invoke(false);
        }
    }
}