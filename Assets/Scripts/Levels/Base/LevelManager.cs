using System;
using System.Collections.Generic;
using System.Linq;
using DexiDev.Game.Data;
using DexiDev.Game.Levels.Nodes;
using DexiDev.Game.Levels.Services;
using DexiDev.Game.Loadings;
using Sirenix.Utilities;
using UnityEngine;
using VContainer;

namespace DexiDev.Game.Levels
{
    public class LevelManager : DataManager<LevelData, LevelConfig>
    {
        private ILevelNodeService[] _services = {
            new LevelNodeRandomService(),
            new LevelNodeService()
        };

        private Dictionary<Type, ILevelNodeService> _servicesTypes = new();
        private IObjectResolver _objectResolver;
        private LoadingManager _loadingManager;
        
        private int _currentLevelLoop;
        private int _currentLevelIndex;
        private LevelController _levelController;

        public int CurrentLevelLoop => _currentLevelLoop;
        public string CurrentLevelID => _config.Datas[_currentLevelIndex]?.ID;
        public LevelController LevelController => _levelController;

        public event Action<int> OnLevelChanged;
        public event Action<int> OnLevelLoopChanged;
        public event Action<LevelController> OnLevelControllerChanged;
        
        [Inject]
        private void Install(IObjectResolver objectResolver, LoadingManager loadingManager, ILevelNodeService[] levelNodeServices)
        {
            _objectResolver = objectResolver;
            _loadingManager = loadingManager;
            _services = levelNodeServices;
        }

        protected override void Initialized()
        {
            base.Initialized();
            
            _services.ForEach(_objectResolver.Inject);
            
            _servicesTypes = _services.ToDictionary(key => key.GetTypeData(), value => value);
        }
        
        
        public void RegisterController(LevelController levelController)
        {
            SetLevelController(levelController);
        }

        public void UnregisterController(LevelController levelController)
        {
            if (_levelController == levelController)
                SetLevelController(null);
        }

        private void SetLevelController(LevelController levelController)
        {
            _levelController = levelController;
            OnLevelControllerChanged?.Invoke(levelController);
        }
        
        public void IncreaseLevel()
        {
            _currentLevelLoop++;
            _currentLevelIndex++;

            if (_currentLevelIndex >= _datas.Count) _currentLevelIndex = 0;
            
            OnLevelChanged?.Invoke(_currentLevelIndex);
            OnLevelLoopChanged?.Invoke(_currentLevelLoop);
        }

        public string[] GetAvailableLevels()
        {
            return _datas.Keys.ToArray();
        }
        
        public void LoadCurrentLevel()
        {
            LoadLevel(CurrentLevelID);
        }

        public void LoadLevel(string levelID)
        {
            var levelData = GetData(levelID);
            if (levelData != null)
            {
                for (int i = 0; i < _config.Datas.Length; i++)
                {
                    if (_config.Datas[i] == levelData)
                    {
                        _currentLevelIndex = i;
                        break;
                    }
                }
                
                LoadLevel(levelData);
            }
        }

        private void LoadLevel(LevelData levelData)
        {
            if (levelData != null)
            {
                _loadingManager.Load(levelData.LoadingID);   
            }
        }
        
        public LevelNode GetNode(ILevelNodeField levelNodeField, Vector3 position, Transform container)
        {
            if (levelNodeField != null)
            {
                if (!_isInitialize) Initialize();

                if (_servicesTypes.TryGetValue(levelNodeField.GetType(), out var service))
                {
                    return service.GetNode(levelNodeField, position, container);
                }
            }

            return null;
        }
    }
}