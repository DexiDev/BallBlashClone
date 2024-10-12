using DexiDev.Game.Levels.Rewards;
using DexiDev.Game.Zones;
using UnityEngine;

namespace DexiDev.Game.Gameplay.TriggerZones
{
    public class TriggerZoneReward : TriggerZone<LevelNodeReward>
    {
        protected override void Enter(Collider collider, LevelNodeReward controller)
        {
            base.Enter(collider, controller);
            controller.PickUp();
        }
    }
}