using DexiDev.Game.Levels.Fields;
using TMPro;
using UnityEngine;

namespace DexiDev.Game.Levels.Nodes
{
    public class LevelNodeScoring : LevelNode<LevelNodeField>
    {
        [SerializeField] private TMP_Text _valueTextField;

        protected override void SetHealth(int health)
        {
            base.SetHealth(health);
            
            _valueTextField.text = _currentHealth.ToString();
        }
    }
}