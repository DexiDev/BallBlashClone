using System;
using UnityEngine;

namespace DexiDev.Game.Assets
{
    public interface IAssetInstance : IAsset<GameObject>, IAssetContract
    {
        AssetGroupData AssetGroup { get; }
        
        event Action<IAssetInstance> OnPoolable;
    }
}