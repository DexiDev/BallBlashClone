using System;
using System.Linq;
using DexiDev.Game.Data.Services;
using DexiDev.Game.Rewards.Data;
using VContainer;

namespace DexiDev.Game.Rewards.Services
{
    public class RewardRandomFieldService : RandomFieldService<IRewardField>, IRewardService
    {
        public event Action<IRewardField> OnReward;

        private RewardManager _rewardManager;

        [Inject]
        private void Install(RewardManager rewardManager)
        {
            _rewardManager = rewardManager;
        }
        
        public Type GetTypeData()
        {
            return typeof(RewardRandomField);
        }

        public void Reward(IRewardField rewardField)
        {
            if (rewardField is RewardRandomField rewardRandomField)
            {
                var randomRewardField = GetRandomField(rewardRandomField);  
                _rewardManager.Reward(randomRewardField);
                OnReward?.Invoke(rewardRandomField);
            }
        }

        public IRewardField[] GetRewards(IRewardField rewardField)
        {
            if (rewardField is RewardRandomField rewardRandomField)
            {
                var rewardFields = rewardRandomField.Value?.SelectMany(randomData => _rewardManager.GetRewards(randomData.Field) ?? Enumerable.Empty<IRewardField>());
                if (rewardFields != null)
                {
                    rewardFields = rewardFields.Where(reward => reward != null);
                    if (rewardFields.Any())
                    {
                        return rewardFields.ToArray();
                    }
                }
            }

            return null;
        }
    }
}