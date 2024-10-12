using System;
using DexiDev.Game.Levels.Fields;
using DexiDev.Game.Rewards;
using VContainer;

namespace DexiDev.Game.Levels.Rewards
{
    public class LevelNodeRewardService : IRewardService
    {
        [Inject] private RewardManager _rewardManager;
        
        public event Action<IRewardField> OnReward;
        
        public Type GetTypeData()
        {
            return typeof(ILevelNodeField);
        }

        public void Reward(IRewardField rewardField)
        {
            if (rewardField is LevelNodeField levelNode)
            {
                _rewardManager.Reward(levelNode.Reward);
                OnReward?.Invoke(levelNode);
            }
        }

        public IRewardField[] GetRewards(IRewardField rewardField)
        {
            if (rewardField is LevelNodeField levelNode)
            {
                return new IRewardField[] { levelNode };
            }

            return null;
        }
    }
}