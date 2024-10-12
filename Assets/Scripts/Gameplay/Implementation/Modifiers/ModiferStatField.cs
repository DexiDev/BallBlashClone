using DexiDev.Game.Data;
using DexiDev.Game.Rewards;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DexiDev.Game.Gameplay.Modifiers
{
    public struct ModifierStatData
    {
        public enum ModifierType
        {
            Add,
            Multiplier
        }
        
        public float Value;
        public ModifierType Type;
    }

    public class ModiferStatField : DataField<ModifierStatData>, IRewardField
    {
        [field: SerializeField, HideLabel] public IModifierStatField TypeStat { get; private set; }
        
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}