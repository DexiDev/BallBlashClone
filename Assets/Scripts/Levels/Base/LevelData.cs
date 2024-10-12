using DexiDev.Game.Data;
using DexiDev.Game.Data.Attributes;
using DexiDev.Game.Loadings;
using UnityEngine;

namespace DexiDev.Game.Levels
{
    [CreateAssetMenu(menuName = "Data/Levels/Level Data", fileName = "Level Data")]
    public class LevelData : DataScriptable
    {
        [field: SerializeField] public ILevelNodeField[][] LevelNodeFields { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Acceleration { get; private set; }
        [field: SerializeField, DataID(typeof(LoadingConfig))] public string LoadingID { get; private set; } 
    }
}