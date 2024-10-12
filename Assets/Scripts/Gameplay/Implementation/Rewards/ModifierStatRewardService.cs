using System;
using DexiDev.Game.Gameplay.Modifiers;
using DexiDev.Game.Rewards;
using VContainer;

namespace DexiDev.Game.Gameplay.Rewards
{
    public class ModifierStatRewardService : IRewardService
    {
        [Inject] private GameplayManager _gameplayManager;
        
        public event Action<IRewardField> OnReward;
        
        public Type GetTypeData()
        {
            return typeof(ModiferStatField);
        }

        public void Reward(IRewardField rewardField)
        {
            if (rewardField is ModiferStatField modiferStatField)
            {
                if (modiferStatField.TypeStat == null) return;
                
                var playerController = _gameplayManager.PlayerController;

                if (playerController != null)
                {
                    var modifierStatField = playerController.GetDataField(modiferStatField.TypeStat);
                    if (modiferStatField != null)
                    {
                        modifierStatField.AddModifier(modiferStatField);
                        OnReward?.Invoke(modiferStatField);
                    }
                }   
            }
        }

        public IRewardField[] GetRewards(IRewardField rewardField)
        {
            if (rewardField is ModiferStatField modiferStatField)
            {
                return new IRewardField[] { modiferStatField };
            }

            return null;
        }
    }
}