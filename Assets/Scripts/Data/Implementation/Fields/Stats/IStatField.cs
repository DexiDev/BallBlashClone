using UnityEngine;

namespace DexiDev.Game.Data.Fields.Stats
{
    public interface IStatField : IDataField, IValueToString
    {
        Sprite Icon { get; }
        string Name { get; }

        string ValueToStringUI();
    }
}