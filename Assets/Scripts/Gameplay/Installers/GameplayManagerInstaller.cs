using VContainer;
using VContainer.Unity;

namespace DexiDev.Game.Gameplay
{
    public class GameplayManagerInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayManager>().AsSelf();
        }
    }
}