using DexiDev.Game.Levels.Fields;
using UnityEngine;

namespace DexiDev.Game.Levels.Nodes
{
    public class LevelNodeCustom: LevelNode<LevelNodeCustomField>
    {
        [SerializeField] private SpriteRenderer _iconRenderer;
        
        public override void SetData(LevelNodeCustomField dataField)
        {
            base.SetData(dataField);
            _iconRenderer.sprite = dataField.Data.SpriteNode;
        }
    }
}