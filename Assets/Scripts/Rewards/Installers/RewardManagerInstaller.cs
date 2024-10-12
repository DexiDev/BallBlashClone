using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DexiDev.Game.Rewards.Installers
{
    public class RewardManagerInstaller : IInstaller
    {
        [SerializeField] private IRewardService[] _rewardServices;
        
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<RewardManager>().WithParameter(_rewardServices).AsSelf();
        }
    }
}