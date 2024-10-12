using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DexiDev.Game.Levels.Installers
{
    public class LevelManagerInstaller : IInstaller
    {
        [SerializeField] private LevelConfig _config;
        [SerializeField] private ILevelNodeService[] _levelNodeService;
        
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LevelManager>().WithParameter(_config).WithParameter(_levelNodeService).AsSelf();
        }
    }
}