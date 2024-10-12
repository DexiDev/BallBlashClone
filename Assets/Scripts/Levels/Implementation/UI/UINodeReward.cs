using System;
using DexiDev.Game.Gameplay.Modifiers;
using DexiDev.Game.Items;
using DexiDev.Game.Items.Fields;
using DexiDev.Game.Rewards;
using DexiDev.Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace DexiDev.Game.Levels.UI
{
    public class UINodeReward : UIElement
    {
        [SerializeField] private TMP_Text _valueField;
        [SerializeField] private Image _imageField;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _playAnimation = "Play";
        
        [Inject] private ItemsManager _itemsManager;

        private void OnEnable()
        {
            if (_animator != null)
            {
                _animator.SetTrigger(_playAnimation);
            }
        }

        public void SetReward(IRewardField rewardField)
        {
            if (rewardField is ItemField itemField)
            {
                var itemData = _itemsManager.GetData(itemField.Value);
                if (itemData != null)
                {
                    SetReward(itemField.Count, itemData.Icon);
                    return;
                }
            }
            else if (rewardField is ModiferStatField modiferStatField)
            {
                var value = (int)Math.Ceiling(modiferStatField.Value.Value);
                SetReward(value, modiferStatField.Icon);
                return;
            }
            
            SetReward(0, null);
        }
        
        public void SetReward(int value, Sprite icon)
        {
            _valueField.text = $"+{value}";
            _imageField.sprite = icon;
        }
    }
}