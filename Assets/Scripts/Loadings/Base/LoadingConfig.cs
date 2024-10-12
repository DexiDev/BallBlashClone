using DexiDev.Game.Data;
using DexiDev.Game.Loadings.UI;
using UnityEngine;

namespace DexiDev.Game.Loadings
{
    [CreateAssetMenu(menuName = "Data/Loading/Loading Config", fileName = "Loading Config")]
    public class LoadingConfig : DataConfig<LoadingData>
    {
        [SerializeField] public UILoadingScreen LoadingScreen { get; private set; }
        [SerializeField] public LoadingData MetaLoadingData { get; private set; }
    }
}