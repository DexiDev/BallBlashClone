using DexiDev.Game.Data.Attributes;
using DexiDev.Game.Data.Fields;
using DexiDev.Game.Rewards;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DexiDev.Game.Items.Fields
{
    public class ItemField : IDCountField, IRewardField
    {
        [DataID(typeof(ItemsConfig))]
        [SerializeField, OnValueChanged(nameof(OnValueChanged))] protected new string _value;
        
        public override string Value
        {
            get => _value;
            protected set => _value = value;
        }
    }
}