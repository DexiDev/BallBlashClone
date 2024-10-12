using DexiDev.Game.UI;
using DexiDev.Game.UI.Screens;
using DexiDev.Game.UI.UIElements;
using UnityEngine;
using VContainer;

namespace DexiDev.Game.Levels.UI
{
    public class UIWinScreen : UIAnimatedScreen
    {
        [SerializeField] private UIButton _loadButton;
        
        [Inject] private LevelManager _levelManager;

        private void OnEnable()
        {
            _loadButton.OnClick += OnClickHandler;
        }
        
        protected override void OnDisable()
        {
            _loadButton.OnClick -= OnClickHandler;
            base.OnDisable();
        }

        private void OnClickHandler()
        {
            _levelManager.LoadCurrentLevel();
        }
    }
}