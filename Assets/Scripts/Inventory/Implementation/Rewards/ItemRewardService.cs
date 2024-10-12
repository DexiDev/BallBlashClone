using System;
using DexiDev.Game.Items.Fields;
using DexiDev.Game.Rewards;
using VContainer;

namespace DexiDev.Game.Inventory.Rewards
{
    public class ItemRewardService : IRewardService
    {
        public event Action<IRewardField> OnReward;
        
        private InventoryManager _inventoryManager;
        
        [Inject]
        private void Install(InventoryManager inventoryManager)
        {
            _inventoryManager = inventoryManager;
        }
        
        public Type GetTypeData()
        {
            return typeof(ItemField);
        }

        public void Reward(IRewardField rewardField)
        {
            if (rewardField is ItemField itemField)
            {
                _inventoryManager.Add(itemField.Value, itemField.Count);
                OnReward?.Invoke(rewardField);
            }
        }

        public IRewardField[] GetRewards(IRewardField rewardField)
        {
            if (rewardField is ItemField itemField)
            {
                return new IRewardField[] { itemField };
            }

            return null;
        }
    }
}