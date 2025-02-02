using DexiDev.Game.Data;
using UnityEngine;

namespace DexiDev.Game.Assets
{
    [CreateAssetMenu(menuName = "Data/Assets/Assets Group Data", fileName = "Assets Group Data")]
    public class AssetGroupData : DataScriptable
    {
        [field: SerializeField] public int PoolLimit { get; private set; }
    }
}