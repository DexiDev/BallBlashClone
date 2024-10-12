using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DexiDev.Game.Data
{
    public abstract class DataScriptable : SerializedScriptableObject
    {
        [field: SerializeField, NotNull] public virtual string ID { get; protected set; }
    }
}