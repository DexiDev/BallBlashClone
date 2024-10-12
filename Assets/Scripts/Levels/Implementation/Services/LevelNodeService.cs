using System;
using DexiDev.Game.Assets;
using DexiDev.Game.Levels.Fields;
using DexiDev.Game.Levels.Nodes;
using UnityEngine;
using VContainer;

namespace DexiDev.Game.Levels.Services
{
    public abstract class LevelNodeService<TField, TController> : ILevelNodeService where TField : LevelNodeField where TController : LevelNode<TField>
    {
        [SerializeField] private TController _nodeScoringContract;

        [Inject] private AssetsManager _assetsManager;
        
        public Type GetTypeData()
        {
            return typeof(TField);
        }

        public LevelNode GetNode(ILevelNodeField levelNodeField, Vector3 position, Transform container)
        {
            if (levelNodeField is TField levelNodeScoringField)
            {
                TController levelNodeScoring = _assetsManager.GetAsset<TController>(_nodeScoringContract, position, Quaternion.identity, container);
                levelNodeScoring.SetData(levelNodeScoringField);
                return levelNodeScoring;
            }

            return null;
        }
    }

    public class LevelNodeService : LevelNodeService<LevelNodeField, LevelNodeScoring>
    {
        
    }
}