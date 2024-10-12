using DexiDev.Game.Levels.Datas;
using UnityEngine;

namespace DexiDev.Game.Levels.Fields
{
    public class LevelNodeCustomField : LevelNodeField
    {
        [field: SerializeField] public LevelNodeCustomData Data { get; private set; }
        
    }
}