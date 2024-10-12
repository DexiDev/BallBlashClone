using DexiDev.Game.Data.Fields;
using DexiDev.Game.Rewards;
using UnityEngine;

namespace DexiDev.Game.Levels.Fields
{
    public class LevelNodeField : IntField, ILevelNodeField, IRewardField
    {
        [field: SerializeField] public IRewardField Reward { get; private set; }
    }
}