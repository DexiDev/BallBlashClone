using System;

namespace DexiDev.Game.Zones
{
    public interface ITriggerZone
    {
        public event Action OnEnter;
        public event Action OnExit;

        public bool HasControllers();
    }
}