using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DexiDev.Game.Items.Installers
{
    public class ItemsManagerInstaller : IInstaller
    {
        [SerializeField] private ItemsConfig _config;

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ItemsManager>().WithParameter(_config).AsSelf();
        }
    }
}