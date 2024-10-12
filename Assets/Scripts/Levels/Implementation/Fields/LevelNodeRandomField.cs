using DexiDev.Game.Data.Fields;
using DexiDev.Game.Rewards;

namespace DexiDev.Game.Levels.Fields
{
    public class LevelNodeRandomField : RandomField<ILevelNodeField>, ILevelNodeField
    {
        public IRewardField Reward => null;
    }
}