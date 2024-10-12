using System.Collections.Generic;
using DexiDev.Game.Data.Fields;
using UnityEngine;

namespace DexiDev.Game.Gameplay.Modifiers
{
    public abstract class ModifierStatIntField : IntField, IModifierStatField
    {

        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public List<ModiferStatField> ModifierValues { get; private set; }

        public override int Value
        {
            get
            {
                var value = _value;
                if (ModifierValues != null)
                {
                    foreach (var modifierValue in ModifierValues)
                    {
                        switch (modifierValue.Value.Type)
                        {
                            case ModifierStatData.ModifierType.Add:
                                value = (int)(value + modifierValue.Value.Value);
                                break;
                            case ModifierStatData.ModifierType.Multiplier:
                                value = (int)(value * modifierValue.Value.Value);
                                break;
                        }
                    }
                }

                return value;
            }
            protected set => _value = value;
        }
        
        public void AddModifier(ModiferStatField modifierValue)
        {
            if (modifierValue == null) return;
            
            ModifierValues ??= new List<ModiferStatField>();

            ModifierValues.Add(modifierValue);
        }

        public void RemoveModifier(ModiferStatField modifierValue)
        {
            if(modifierValue == null || ModifierValues == null) return;

            ModifierValues.Remove(modifierValue);
        }
        
        public string ValueToString()
        {
            return Value.ToString();
        }
        
        public string ValueToStringUI()
        {
            return Value.ToString();
        }
    }
}