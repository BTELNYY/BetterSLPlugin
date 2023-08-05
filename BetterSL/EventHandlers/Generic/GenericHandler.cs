using System;
using BetterSL.StatusEffects;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using PluginAPI.Core;
using MEC;

namespace BetterSL.EventHandlers.Generic
{
    public class GenericHandler
    {
        [PluginEvent(ServerEventType.RoundEnd)]
        public void OnRoundEnd(RoundEndEvent ev)
        {
            DimensionBodyHandler.RagdollQueue.Clear();
            DimensionBodyHandler.ThreadStarted = false;
            DimensionBodyHandler.Handler.IsRunning = false;
            ChaosTargetHandler.ChaosTotal = 0;
            ChaosTargetHandler.ChaosAlive = 0;
            ItemSpawnHandler.ModifiedItems.Clear();
            CorrodingHandler.CorrodedPlayers.Clear();
        }

        [PluginEvent(ServerEventType.PlayerSpawn)]
        public void Debug(PlayerSpawnEvent ev)
        {
            Timing.CallDelayed(0.2f, () => 
            {
                if (ev.Role == PlayerRoles.RoleTypeId.None || ev.Role == PlayerRoles.RoleTypeId.Spectator)
                {
                    return;
                }
            });
        }
    }
}