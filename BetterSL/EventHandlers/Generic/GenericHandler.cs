using System;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using BetterSL.EventHandlers.Scp079;

namespace BetterSL.EventHandlers.Generic
{
    public class GenericHandler
    {
        [PluginEvent(ServerEventType.RoundEnd)]
        public static void OnRoundEnd()
        {
            DimensionBodyHandler.RagdollQueue.Clear();
            DimensionBodyHandler.ThreadStarted = false;
            DimensionBodyHandler.Handler.IsRunning = false;
            ChaosTargetHandler.ChaosTotal = 0;
            ChaosTargetHandler.ChaosAlive = 0;
        }
    }
}
