using VContainer;
using VContainer.Unity;

namespace DexiDev.Game.Inventory.Installers
{
    public class InventoryManagerInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InventoryManager>().AsSelf();
        }
    }
}