using System;
using System.Collections.Generic;
using System.Linq;
using DexiDev.Game.Inventory.Rewards;
using DexiDev.Game.Rewards.Services;
using Sirenix.Utilities;
using VContainer;
using VContainer.Unity;

namespace DexiDev.Game.Rewards
{
    public class RewardManager : IInitializable
    {
        private IRewardService[] _services = {
            new ItemRewardService(),
            new RewardListService(),
            new RewardRandomFieldService()
        };

        private Dictionary<Type, IRewardService> _servicesTypes = new();

        private IObjectResolver _objectResolver;
        
        private bool _isInitialize;
        
        public event Action<IRewardField> OnReward;

        [Inject]
        private void Install(IObjectResolver objectResolver, IRewardService[] rewardService)
        {
            _services = rewardService;
            _objectResolver = objectResolver;
        }

        public void Initialize()
        {
            if (_isInitialize) return;
            
            _isInitialize = true;
            
            _services.ForEach(_objectResolver.Inject);
            
            _servicesTypes = _services.ToDictionary(key => key.GetTypeData(), value => value);

            _services.ForEach(service => service.OnReward += OnReward);
        }

        public void Reward(IRewardField rewardField)
        {
            if (rewardField == null) return;
            
            if(!_isInitialize) Initialize();
            
            if (_servicesTypes.TryGetValue(rewardField.GetType(), out var service))
            {
                service.Reward(rewardField);
            }
        }

        public IRewardField[] GetRewards(IRewardField rewardField)
        {
            if (rewardField != null)
            {
                if (!_isInitialize) Initialize();

                if (_servicesTypes.TryGetValue(rewardField.GetType(), out var service))
                {
                    return service.GetRewards(rewardField);
                }
            }

            return null;
        }
    }
}