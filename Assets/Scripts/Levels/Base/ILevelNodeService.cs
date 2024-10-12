using System;
using DexiDev.Game.Levels.Nodes;
using UnityEngine;

namespace DexiDev.Game.Levels
{
    public interface ILevelNodeService
    {
        public Type GetTypeData();

        public LevelNode GetNode(ILevelNodeField levelNodeField, Vector3 position, Transform container);
    }
}