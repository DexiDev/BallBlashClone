using System;
using DexiDev.Game.Data.Services;
using DexiDev.Game.Levels.Fields;
using DexiDev.Game.Levels.Nodes;
using UnityEngine;
using VContainer;

namespace DexiDev.Game.Levels.Services
{
    public class LevelNodeRandomService : RandomFieldService<ILevelNodeField>, ILevelNodeService
    {
        [Inject] private LevelManager _levelManager;
        
        public Type GetTypeData()
        {
            return typeof(LevelNodeRandomField);
        }

        public LevelNode GetNode(ILevelNodeField levelNodeField, Vector3 position, Transform container)
        {
            if (levelNodeField is LevelNodeRandomField levelNodeRandomField)
            {
                ILevelNodeField nodeField = GetRandomField(levelNodeRandomField);

                return _levelManager.GetNode(nodeField, position, container);
            }

            return null;
        }
    }
}