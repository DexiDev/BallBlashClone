using UnityEngine;

namespace DexiDev.Game.Levels.Datas
{
    [CreateAssetMenu(menuName = "Data/Levels/Level Node Custom Data", fileName = "Level Node Custom Data")]
    public class LevelNodeCustomData : ScriptableObject
    {
        [field: SerializeField] public Sprite SpriteNode { get; set; }
        [field: SerializeField] public Sprite SpriteReward { get; set; } 
    }
}