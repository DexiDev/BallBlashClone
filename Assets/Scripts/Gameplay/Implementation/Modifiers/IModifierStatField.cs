using System.Collections.Generic;
using DexiDev.Game.Data.Fields.Stats;

namespace DexiDev.Game.Gameplay.Modifiers
{
    public interface IModifierStatField : IStatField
    {
        public List<ModiferStatField> ModifierValues { get; }
        
        public void AddModifier(ModiferStatField modifierValue);
        
        public void RemoveModifier(ModiferStatField modifierValue);
    }
}