using DexiDev.Game.Data.Attributes;
using DexiDev.Game.Levels;
using UnityEngine;
using VContainer;

namespace DexiDev.Game.Loadings.Boot
{
    public class BootLoader : MonoBehaviour
    {
        [Inject] private LevelManager _levelManager;

        private void Start()
        {
            _levelManager.LoadCurrentLevel();
        }
    }
}